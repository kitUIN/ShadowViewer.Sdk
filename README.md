<div align="center">
  
# ShadowViewer.Core

ShadowViewer核心功能组件,为本体与插件开发提供支持


</div>
<p align="center">
   <a href="https://github.com/kitUIN/ShadowViewer.Core/releases">
    <img src="https://img.shields.io/github/v/release/kitUIN/ShadowViewer.Core?color=blueviolet&include_prereleases" alt="release">
  </a>
  <a href="https://raw.githubusercontent.com/kitUIN/ShadowViewer.Core/master/LICENSE.txt">
    <img src="https://img.shields.io/github/license/kitUIN/ShadowViewer.Core" alt="license">
  </a>
</p>

## 备份导出与导入

此功能的 SDK 包版本暂定为 `4.0.1`（未发布），与备份文件中的 `Version`、`FormatVersion` 分别管理。

`ShadowViewer.Sdk.Backup.IBackupService` 提供统一的流式调用接口，默认实现为 `BackupService`，
在 `DiHelper.Init()` 中注册。备份是 ZIP/Deflate 压缩的二进制文件，不依赖文件扩展名识别内容。

```csharp
using Microsoft.Data.Sqlite;
using ShadowViewer.Sdk.Backup;
using System.Data;
using System.Text.Json;

IBackupService backupService = new BackupService();
using var connection = new SqliteConnection("Data Source=ShadowViewer.sqlite");
using var settings = JsonDocument.Parse("{\"readingMode\":2}");

// Version 由调用方分配；下次可以传入 "002"。每个备份都是独立的完整快照。
await using (var output = File.Create("library-001.svbackup"))
{
    await backupService.ExportAsync(output, "001",
    [
        new BackupEntry("database", connection),
        new BackupEntry("settings", settings)
    ]);
}

await using var input = File.OpenRead("library-001.svbackup");
var backup = await backupService.ImportAsync(input);
Console.WriteLine(backup.Version); // "001"
var restoredSettings = (JsonElement)backup.Entries.Single(x => x.Name == "settings").Data;

// 恢复前暂停数据库访问并释放所有 DbContext；恢复会整体替换目标数据库。
var database = (SqliteBackup)backup.Entries.Single(x => x.Name == "database").Data;
using var destination = new SqliteConnection("Data Source=Restored.sqlite");
await database.RestoreAsync(destination);
```

| 导出数据 | 自动识别方式 | 导入结果 |
| --- | --- | --- |
| `SqliteConnection`、`SqliteBackup`、SQLite `byte[]` | 运行时类型 / SQLite 文件头 | `SqliteBackup`，通过 `RestoreAsync` 恢复 |
| `DataTable`、已序列化的 table `byte[]` | 运行时类型 / `SVTB` 二进制文件头 | `DataTable` |
| `JsonDocument`、`JsonElement`、`JsonNode`、有效 JSON 字符串或 UTF-8 `byte[]` | 运行时类型 / JSON 内容，支持 UTF-8 BOM | 独立的 `JsonElement` |
| 其他 `byte[]` | 二进制兜底编码 | 原始 `byte[]` |

SQLite 通过 `BackupDatabase` 获取一致性快照，包含已提交的 WAL 数据、所有表的结构与内容、
索引、触发器、外键和 EF migration 历史。传入数据库连接时不要并发使用该连接，也不要保留活动事务。
不能通过直接复制正在使用的 `.sqlite` 文件来保证 WAL 数据完整，应传入连接。

DataTable 使用固定类型编码和二进制行值，保留表名、命名空间、区域、大小写设置，
以及列名、类型、标题、可空性、默认值、只读属性、字符串长度、日期模式、自增参数、唯一约束和主键。
支持布尔、整数、浮点数、decimal、char、string、DateTime、DateTimeOffset、TimeSpan、Guid 和 byte[]。
仅导出表结构时传入 `table.Clone()`；包含行时直接传入 `table`；已删除的行不导出。
跨表关系、表达式列和自定义 CLR 类型应使用 SQLite 快照或自定义序列化器，不会被静默转换。

备份版本 `Version` 是不透明字符串，保留 `001` 的前导零；调用方负责版本递增或日期命名。
容器另有 `FormatVersion=1`，table/binary 编码也分别具有格式标识。
SDK 可读取不同备份版本的同一格式，遇到未知文件格式或序列化器会拒绝导入。
恢复旧数据库后，可按已有流程调用 `DatabaseUpgrade.Initialize` 执行 EF 迁移；
JSON 和其他业务数据的版本迁移由调用方根据 `backup.Version` 处理。

包内 manifest 记录备份版本、UTC 创建时间、每个数据项的名称、格式、未压缩大小和 SHA-256。
导入检查这些信息及 SQLite 完整性后返回数据，不直接修改应用状态；调用方决定如何应用 JSON、表格和数据库。
文件格式由内容识别，并与 manifest 校验，不根据扩展名猜测。未知 ZIP 文件不是有效备份。
SHA-256 用于检测损坏，不提供加密或身份认证。

可通过 `new BackupService(additionalSerializers: [...])` 注册 `IBackupSerializer`，
扩展格式的 `FormatId` 必须稳定且唯一，自定义序列化器优先于内置序列化器。
同一个包中名称必须唯一，名称只作为逻辑标识，不作为提取路径。
默认最多 4096 个数据项、单项 256 MiB、未压缩总量 1 GiB，可通过 `BackupLimits` 调整。
当前实现按数据项在内存中序列化；接口支持 `CancellationToken`，但 SQLite 原生快照/恢复调用期间不能中断。
导入流必须支持 seek，导出使用新的空流；调用方拥有流并负责释放。
失败的导出可能留下不完整文件，调用方应丢弃该文件后重新导出。

运行独立回归检查（不依赖 WinUI）：

```powershell
dotnet run --project tests/Backup.Tests/Backup.Tests.csproj
```
 

