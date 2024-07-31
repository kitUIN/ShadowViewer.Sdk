namespace ShadowViewer.Args;

/// <summary>
/// 小弹窗通知Args
/// </summary>
/// <param name="text">通知内容</param>
/// <param name="level">警告等级</param>
/// <param name="displaySeconds">显示时长</param>
public class TipPopupEventArgs(string text = "", 
    InfoBarSeverity level = InfoBarSeverity.Informational,
    double displaySeconds = 2)
{
    /// <summary>
    /// 通知内容
    /// </summary>
    public string Text { get; } = text;

    /// <summary>
    /// 警告等级
    /// </summary>
    public InfoBarSeverity Level { get; } = level;

    /// <summary>
    /// 显示时长
    /// </summary>
    public double DisplaySeconds { get; } = displaySeconds;
}