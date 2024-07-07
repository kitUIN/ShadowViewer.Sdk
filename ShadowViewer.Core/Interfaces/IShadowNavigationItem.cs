namespace ShadowViewer.Interfaces;

/// <summary>
/// 导航项
/// </summary>
public interface IShadowNavigationItem
{
    /// <summary>
    /// 内容
    /// </summary>
    object? Content { get; }

    /// <summary>
    /// 图标
    /// </summary>
    IconElement? Icon { get; }

    /// <summary>
    /// 跳转的标识符
    /// </summary>
    public string? Id { get;  }

    /// <summary>
    /// 所属的插件Id
    /// </summary>
    public string PluginId { get;  }
}