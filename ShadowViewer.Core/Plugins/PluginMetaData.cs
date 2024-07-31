using DryIoc;
using ShadowPluginLoader.MetaAttributes;
using ShadowPluginLoader.WinUI;
using ShadowPluginLoader.WinUI.Models;

namespace ShadowViewer.Plugins;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

/// <summary>
/// 插件元数据
/// </summary>
[ExportMeta]
public class PluginMetaData : AbstractPluginMetaData
{
    /// <summary>
    /// 介绍
    /// </summary>
    public string Description { get; init; }
    /// <summary>
    /// 作者
    /// </summary>
    public string Authors { get; init; }

    /// <summary>
    /// 项目地址
    /// </summary>
    public string WebUri { get; init; }

    /// <summary>
    /// 图标<br/>
    /// 1.本地文件,以ms-appx://开头<br/>
    /// 2.FontIcon,以font://开头<br/>
    /// 3.FluentRegularIcon,以fluent://regular开头
    /// 4.FluentFilledIcon,以fluent://filled
    /// <example><br/>
    /// 1.ms-appx:///Assets/Icons/Logo.png<br/>
    /// 2.font://\uE714<br/>
    /// 3.fluent://regular/\uE714
    /// 4.fluent://filled/\uE714
    /// </example>
    /// </summary>
    public string Logo { get; init; }

}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
