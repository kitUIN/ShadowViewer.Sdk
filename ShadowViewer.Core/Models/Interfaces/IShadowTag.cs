using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Core.Models.Interfaces;

/// <summary>
/// 标签基类
/// </summary>
public interface IShadowTag
{
    /// <summary>
    /// 标签名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 背景颜色，例如 #7cbbe2
    /// </summary>
    public string BackgroundHex { get; set; }

    /// <summary>
    /// 字体颜色，例如 #7cbbe2
    /// </summary>
    public string ForegroundHex { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 所属插件Id
    /// </summary>
    public string PluginId { get; set; }

    /// <summary>
    /// 标签类型,0为类别标签，1为自定义标签
    /// </summary>
    public int TagType { get; set; }

    /// <summary>
    /// 能否修改
    /// </summary>
    public bool AllowModify { get; }
}