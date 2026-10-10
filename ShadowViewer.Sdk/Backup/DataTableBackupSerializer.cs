using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowViewer.Sdk.Backup;

/// <summary>Binary table schema and current rows with explicit, portable CLR type codes.</summary>
public sealed class DataTableBackupSerializer : IBackupSerializer
{
    private static readonly byte[] Header = Encoding.ASCII.GetBytes("SVTB\x01");
    // Array indices are persisted type codes. Never reorder these types within format version 1.
    private static readonly Type[] Types =
    [
        typeof(bool), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint),
        typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(char), typeof(string),
        typeof(DateTime), typeof(DateTimeOffset), typeof(TimeSpan), typeof(Guid), typeof(byte[])
    ];

    /// <inheritdoc />
    public string FormatId => "table";

    /// <inheritdoc />
    public bool CanSerialize(object value) => value is DataTable || (value is byte[] bytes && CanDeserialize(bytes));

    /// <inheritdoc />
    public bool CanDeserialize(ReadOnlyMemory<byte> payload) => payload.Span.StartsWith(Header);

    /// <inheritdoc />
    public Task<byte[]> SerializeAsync(object value, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (value is byte[] bytes) return Task.FromResult((byte[])bytes.Clone());
        var table = (DataTable)value;
        if ((table.Columns.Count == 0 && table.Rows.Count != 0) || table.Columns.Count > 4096 || table.Constraints.Count > 4096)
            throw new NotSupportedException("Table backups require at most 4096 columns/constraints and columns for nonempty tables.");
        if (table.Columns.Cast<DataColumn>().Any(x => !Types.Contains(x.DataType)) ||
            table.Constraints.Cast<Constraint>().Any(x => x is not UniqueConstraint) ||
            table.Columns.Cast<DataColumn>().Any(x => !string.IsNullOrEmpty(x.Expression)))
            throw new NotSupportedException("Table backups support primitive columns and unique/primary keys. Use SQLite snapshots for relations or expressions.");
        using var output = new MemoryStream();
        using var writer = new BinaryWriter(output, Encoding.UTF8, leaveOpen: true);
        writer.Write(Header);
        writer.Write(table.TableName);
        writer.Write(table.Namespace);
        writer.Write(table.Prefix);
        writer.Write(table.CaseSensitive);
        writer.Write(table.Locale.Name);
        writer.Write(table.Columns.Count);
        foreach (DataColumn column in table.Columns)
        {
            cancellationToken.ThrowIfCancellationRequested();
            writer.Write(column.ColumnName);
            var typeCode = Array.IndexOf(Types, column.DataType);
            writer.Write((byte)typeCode);
            writer.Write(column.Caption);
            writer.Write(column.Namespace);
            writer.Write(column.Prefix);
            writer.Write(column.AllowDBNull);
            writer.Write(column.AutoIncrement);
            writer.Write(column.AutoIncrementSeed);
            writer.Write(column.AutoIncrementStep);
            writer.Write(column.ReadOnly);
            writer.Write(column.MaxLength);
            writer.Write((int)column.DateTimeMode);
            WriteValue(writer, typeCode, column.DefaultValue);
        }
        writer.Write(table.Constraints.Count);
        foreach (UniqueConstraint constraint in table.Constraints)
        {
            writer.Write(constraint.ConstraintName);
            writer.Write(constraint.IsPrimaryKey);
            writer.Write(constraint.Columns.Length);
            foreach (var column in constraint.Columns) writer.Write(column.Ordinal);
        }
        var rows = table.Rows.Cast<DataRow>().Where(x => x.RowState != DataRowState.Deleted).ToArray();
        writer.Write(rows.Length);
        foreach (var row in rows)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (DataColumn column in table.Columns)
                WriteValue(writer, Array.IndexOf(Types, column.DataType), row[column]);
        }
        return Task.FromResult(output.ToArray());
    }

    /// <inheritdoc />
    public Task<object> DeserializeAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!CanDeserialize(payload)) throw new InvalidDataException("Invalid table backup header.");
        using var input = new MemoryStream(payload.ToArray(), writable: false);
        using var reader = new BinaryReader(input, Encoding.UTF8);
        input.Position = Header.Length;
        var table = new DataTable(reader.ReadString())
        {
            Namespace = reader.ReadString(), Prefix = reader.ReadString(), CaseSensitive = reader.ReadBoolean(),
            Locale = CultureInfo.GetCultureInfo(reader.ReadString())
        };
        var columnCount = ReadCount(reader, 4096);
        var readOnly = new bool[columnCount];
        var typeCodes = new int[columnCount];
        for (var index = 0; index < columnCount; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var name = reader.ReadString();
            var code = reader.ReadByte();
            if (code >= Types.Length) throw new InvalidDataException("Unknown table column type.");
            typeCodes[index] = code;
            var column = new DataColumn(name, Types[code])
            {
                Caption = reader.ReadString(), Namespace = reader.ReadString(), Prefix = reader.ReadString(),
                AllowDBNull = reader.ReadBoolean()
            };
            var autoIncrement = reader.ReadBoolean();
            column.AutoIncrementSeed = reader.ReadInt64();
            column.AutoIncrementStep = reader.ReadInt64();
            readOnly[index] = reader.ReadBoolean();
            var maxLength = reader.ReadInt32();
            if (column.DataType == typeof(string)) column.MaxLength = maxLength;
            var dateTimeMode = (DataSetDateTime)reader.ReadInt32();
            if (column.DataType == typeof(DateTime)) column.DateTimeMode = dateTimeMode;
            column.DefaultValue = ReadValue(reader, code);
            column.AutoIncrement = autoIncrement;
            table.Columns.Add(column);
        }
        var constraintCount = ReadCount(reader, 4096);
        var constraints = new UniqueConstraint[constraintCount];
        for (var index = 0; index < constraintCount; index++)
        {
            var name = reader.ReadString();
            var primary = reader.ReadBoolean();
            var columns = new DataColumn[ReadCount(reader, columnCount)];
            for (var column = 0; column < columns.Length; column++)
            {
                var ordinal = reader.ReadInt32();
                if (ordinal < 0 || ordinal >= columnCount) throw new InvalidDataException("Invalid constraint column.");
                columns[column] = table.Columns[ordinal];
            }
            constraints[index] = new UniqueConstraint(name, columns, primary);
        }
        // Every cell consumes at least a null marker. Check bounds before allocating rows.
        var rowCount = ReadCount(reader, columnCount == 0 ? 0 : (int)Math.Min(int.MaxValue, (input.Length - input.Position) / columnCount));
        table.BeginLoadData();
        for (var row = 0; row < rowCount; row++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var values = new object[columnCount];
            for (var column = 0; column < columnCount; column++) values[column] = ReadValue(reader, typeCodes[column]);
            table.Rows.Add(values);
        }
        table.EndLoadData();
        foreach (var constraint in constraints) table.Constraints.Add(constraint);
        for (var index = 0; index < columnCount; index++) table.Columns[index].ReadOnly = readOnly[index];
        table.AcceptChanges();
        if (input.Position != input.Length) throw new InvalidDataException("Unexpected trailing table data.");
        return Task.FromResult<object>(table);
    }

    private static int ReadCount(BinaryReader reader, int maximum)
    {
        var count = reader.ReadInt32();
        if (count < 0 || count > maximum) throw new InvalidDataException("Invalid table item count.");
        return count;
    }

    private static void WriteValue(BinaryWriter writer, int code, object value)
    {
        writer.Write(value != DBNull.Value);
        if (value == DBNull.Value) return;
        switch (code)
        {
            case 0: writer.Write((bool)value); break;
            case 1: writer.Write((byte)value); break;
            case 2: writer.Write((sbyte)value); break;
            case 3: writer.Write((short)value); break;
            case 4: writer.Write((ushort)value); break;
            case 5: writer.Write((int)value); break;
            case 6: writer.Write((uint)value); break;
            case 7: writer.Write((long)value); break;
            case 8: writer.Write((ulong)value); break;
            case 9: writer.Write((float)value); break;
            case 10: writer.Write((double)value); break;
            case 11: writer.Write((decimal)value); break;
            case 12: writer.Write((ushort)(char)value); break;
            case 13: writer.Write((string)value); break;
            case 14: writer.Write(((DateTime)value).ToBinary()); break;
            case 15:
                var date = (DateTimeOffset)value;
                writer.Write(date.Ticks);
                writer.Write(date.Offset.Ticks);
                break;
            case 16: writer.Write(((TimeSpan)value).Ticks); break;
            case 17: writer.Write(((Guid)value).ToByteArray()); break;
            case 18:
                var bytes = (byte[])value;
                writer.Write(bytes.Length);
                writer.Write(bytes);
                break;
            default: throw new InvalidDataException("Unknown table value type.");
        }
    }

    private static object ReadValue(BinaryReader reader, int code)
    {
        if (!reader.ReadBoolean()) return DBNull.Value;
        return code switch
        {
            0 => reader.ReadBoolean(), 1 => reader.ReadByte(), 2 => reader.ReadSByte(), 3 => reader.ReadInt16(),
            4 => reader.ReadUInt16(), 5 => reader.ReadInt32(), 6 => reader.ReadUInt32(), 7 => reader.ReadInt64(),
            8 => reader.ReadUInt64(), 9 => reader.ReadSingle(), 10 => reader.ReadDouble(), 11 => reader.ReadDecimal(),
            12 => (char)reader.ReadUInt16(), 13 => reader.ReadString(), 14 => DateTime.FromBinary(reader.ReadInt64()),
            15 => new DateTimeOffset(reader.ReadInt64(), TimeSpan.FromTicks(reader.ReadInt64())),
            16 => TimeSpan.FromTicks(reader.ReadInt64()), 17 => new Guid(ReadBytes(reader, 16)),
            18 => ReadBytes(reader, ReadCount(reader, (int)(reader.BaseStream.Length - reader.BaseStream.Position))),
            _ => throw new InvalidDataException("Unknown table value type.")
        };
    }

    private static byte[] ReadBytes(BinaryReader reader, int count)
    {
        var bytes = reader.ReadBytes(count);
        if (bytes.Length != count) throw new EndOfStreamException("Truncated table value.");
        return bytes;
    }
}
