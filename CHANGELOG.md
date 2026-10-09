## 4.0.0

- 用 EF Core 8.0.31 + SQLite 替代 SqlSugarCore，保留既有 SQLite 文件。
- 公开 `ShadowDbContext`、`IDbContextFactory` 的 DryIoc 注册、独立 ID 生成器和安全升级入口。
- SDK 数据表使用版本化 EF migrations，升级前备份，旧库在事务中接管；拒绝用旧组件打开新迁移历史。
- 破坏性 API 变更：`AShadowViewerPlugin.Db` 改为 `DbFactory`，插件需要适配并重新编译。

### Feature ⭐

* 🌟允许插件提供自定义响应器
* 🥰PicViewResponder 采用上下文模式
