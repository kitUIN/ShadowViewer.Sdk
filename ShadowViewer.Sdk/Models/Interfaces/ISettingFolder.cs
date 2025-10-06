namespace ShadowViewer.Sdk.Models.Interfaces;

/// <summary>
/// 设置文件夹基类
/// </summary>
public interface ISettingFolder : IPluginId
{
    /// <summary>
    /// 名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 介绍
    /// </summary>
    string Description { get; }
    /// <summary>
    /// 路径
    /// </summary>
    string Path { get; set; }
    /// <summary>
    /// 能否打开文件夹
    /// </summary>
    bool CanOpen { get; }
    /// <summary>
    /// 能否修改
    /// </summary>
    bool CanChange { get; }
}