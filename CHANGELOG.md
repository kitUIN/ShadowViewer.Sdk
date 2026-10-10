## 4.0.1（暂定，未发布）

- 新增 `IBackupService` 和 `IBackupSerializer`：带独立快照版本的压缩二进制备份，支持 SQLite、DataTable、JSON 和二进制内容自动识别及插件扩展格式。
- 新增 SQLite WAL 快照与恢复、DataTable 二进制结构/行数据编码、SHA-256 完整性校验及导入大小限制。
- 在 SDK 初始化时注册默认备份服务，并增加独立备份回归检查与调用示例。

## 4.0.0

- 用 EF Core 8.0.31 + SQLite 替代 SqlSugarCore，保留既有 SQLite 文件。
- 公开 `ShadowDbContext`、`IDbContextFactory` 的 DryIoc 注册、独立 ID 生成器和安全升级入口。
- SDK 数据表使用版本化 EF migrations，升级前备份，旧库在事务中接管；拒绝用旧组件打开新迁移历史。
- 破坏性 API 变更：`AShadowViewerPlugin.Db` 改为 `DbFactory`，插件需要适配并重新编译。

### Feature ⭐

* 🌟允许插件提供自定义响应器
* 🥰PicViewResponder 采用上下文模式
