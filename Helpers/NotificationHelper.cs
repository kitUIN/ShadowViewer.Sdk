using ShadowViewer.Controls;

namespace ShadowViewer.Helpers;
/// <summary>
/// 通知帮助类
/// </summary>
public static class NotificationHelper
{
    /// <summary>
    /// 发送通知给主窗体,将会在主窗体显示通知
    /// </summary>
    /// <param name="sender">发送者</param>
    /// <param name="message">发送信息</param>
    /// <param name="level">通知等级</param>
    /// <example>
    /// NotificationHelper.Notify(this, "加载成功", InfoBarSeverity.Success);
    /// </example>
    public static void Notify(object sender,string message,InfoBarSeverity level)
    {
        DiFactory.Services.Resolve<CallableService>().TopGrid(sender, 
            new TipPopup(message,level), TopGridMode.Tip);
    }
}