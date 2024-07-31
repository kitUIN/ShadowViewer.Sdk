namespace ShadowViewer.Services;



/// <summary>
/// 通知服务类
/// </summary>
internal class NotifyService(ILogger logger) : INotifyService
{
    /// <summary>
    /// 日志
    /// </summary>
    private ILogger Logger { get; } = logger;

    /// <inheritdoc />
    public event EventHandler<TipPopupEventArgs>? TipPopupEvent;

    /// <inheritdoc />
    public void NotifyTip(object sender, string message,
        InfoBarSeverity level = InfoBarSeverity.Informational, double displaySeconds = 2)
    {
        TipPopupEvent?.Invoke(sender,new TipPopupEventArgs(message, level, displaySeconds));
        Logger.Debug("触发事件{EventName},Message={Message},Level={Level},Time={Time}",
            nameof(TipPopupEvent), message, level, displaySeconds);
    }
}
