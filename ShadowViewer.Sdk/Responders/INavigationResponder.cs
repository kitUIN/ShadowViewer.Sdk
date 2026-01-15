using System.Collections.Generic;
using ShadowViewer.Sdk.Models.Interfaces;
using ShadowViewer.Sdk.Utils;

namespace ShadowViewer.Sdk.Responders;
/// <summary>
/// 导航触发器基类
/// </summary>
public interface INavigationResponder : IResponder
{

    /// <summary>
    /// 添加到导航栏
    /// </summary>
    IEnumerable<IShadowNavigationItem> NavigationViewMenuItems { get; }

    /// <summary>
    /// 添加到导航栏尾部
    /// </summary>
    IEnumerable<IShadowNavigationItem> NavigationViewFooterItems { get; }

    /// <summary>
    /// 注册
    /// </summary>
    void Register();
}