using System;

namespace ShadowViewer.Sdk.Models.Interfaces;
/// <summary>
/// 历史记录基类
/// </summary>
public interface IHistory
{
    /// <summary>
    /// 名称
    /// </summary>
    string Title { get; set; }
    /// <summary>
    /// ID
    /// </summary>
    long Id { get; set; }
    /// <summary>
    /// 图片
    /// </summary>
    string Thumb { get; set; }
    /// <summary>
    /// 阅读时间
    /// </summary>
    DateTime LastReadDateTime { get; set; }
    /// <summary>
    /// 附加信息
    /// </summary>
    string? Extra { get; set; }

    /// <summary>
    /// 来源插件
    /// </summary>
    string PluginId { get; }
}