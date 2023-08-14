namespace ShadowViewer.Plugins;

/// <summary>
/// 插件元数据
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class PluginMetaData : Attribute
{
    public string Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string Author { get; }
    public string Version { get; }
    public string WebUri { get; }
    public string Logo { get; }
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
        Logo = logo;
        MinVersion = requireVersion;
    }
}