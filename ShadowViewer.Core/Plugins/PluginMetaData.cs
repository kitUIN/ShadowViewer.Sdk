using DryIoc;
using ShadowPluginLoader.MetaAttributes;
using ShadowPluginLoader.WinUI;
using ShadowPluginLoader.WinUI.Models;

namespace ShadowViewer.Plugins;

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
    public string Logo
    {
        get => logo;
        init
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.StartsWith("/") && value != "/")
                {
                    logo = value.AssetPath();
                }
                else if (value.StartsWith("ms-appx:///"))
                {
                    logo = value.Replace("ms-appx://", "").AssetPath();
                }
                else if (value.StartsWith("ms-appx://"))
                {
                    logo = value.Replace("ms-appx://", "/").AssetPath();
                }
                else
                {
                    logo = value;
                }
            }
        }
    }
    private string logo;

}