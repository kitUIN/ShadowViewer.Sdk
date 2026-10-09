using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ShadowViewer.Sdk.Database;

public sealed class DatabaseIdValueGenerator : ValueGenerator<long>
{
    public override bool GeneratesTemporaryValues => false;
    public override long Next(EntityEntry entry) => DatabaseIds.Next();
}
