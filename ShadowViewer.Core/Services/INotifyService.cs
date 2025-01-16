namespace ShadowViewer.Services;

/// <summary>
/// 通知服务基类
/// </summary>
public interface INotifyService
{
    /// <summary>
    /// 通知事件
    /// </summary>
    event EventHandler<TipPopupEventArgs>? TipPopupEvent;

    /// <summary>
    /// 发送通知给主窗体,将会在主窗体显示通知
    /// </summary>
    /// <param name="sender">发送者</param>
    /// <param name="message">发送信息</param>
    /// <param name="level">通知等级</param>
    /// <param name="displaySeconds">通知时间</param>
    /// <param name="position">通知位置</param>
    /// <example>
    /// NotifyService.NotifyTip(this, "加载成功", InfoBarSeverity.Success);
    /// </example>
    void NotifyTip(object sender, string message,
        InfoBarSeverity level = InfoBarSeverity.Informational,
        double displaySeconds = 2,
        TipPopupPosition position = TipPopupPosition.Center);

    /// <summary>
    /// 发送通知给主窗体,将会在主窗体显示通知
    /// </summary>
    /// <param name="sender">发送者</param>
    /// <param name="tipPopup">通知控件</param>
    /// <param name="displaySeconds">通知时间</param>
    /// <param name="position">通知位置</param>
    void NotifyTip(object sender, InfoBar tipPopup,
        double displaySeconds = 2,
        TipPopupPosition position = TipPopupPosition.Center);
}