namespace ShadowViewer.Plugins;

/// <summary>
/// 插件元数据
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class PluginMetaData : Attribute
{
    /// <summary>
    /// 标识符(大小写不敏感)
    /// </summary>
    public string Id { get; }
    /// <summary>
    /// 显示的名称
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// 介绍
    /// </summary>
    public string Description { get; }
    /// <summary>
    /// 作者
    /// </summary>
    public string Author { get; }
    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; }
    /// <summary>
    /// 项目地址
    /// </summary>
    public string WebUri { get; }

    /// <summary>
    /// 图标
    /// </summary>
    public string Logo { get; } = "/";
    /// <summary>
    /// 支持的插件管理器版本,该版本即为ShadowViewer.Core的发行版
    /// </summary>
    public int MinVersion { get; }

    public PluginMetaData(string id, string name, string description, string author, string version, string webUri,
        string logo, int requireVersion)
    {
        Id = id;
        Name = name;
        Description = description;
        Author = author;
        Version = version;
        WebUri = webUri;
        if(!string.IsNullOrEmpty(logo))
            Logo = logo;
        MinVersion = requireVersion;
    }
}