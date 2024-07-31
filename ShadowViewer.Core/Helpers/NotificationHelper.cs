
namespace ShadowViewer.Helpers;
/// <summary>
/// 通知帮助类
/// </summary>
public static class NotificationHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="dialog"></param>
    /// <example>
    /// NotificationHelper.Dialog(this, ContentDialogHelper.CreateHttpDialog(BikaHttpStatus.Unknown, exception.ToString()));
    /// </example>
    public static void Dialog(object sender,ContentDialog dialog)
    {
        DiFactory.Services.Resolve<ICallableService>().TopGrid(sender, dialog, TopGridMode.ContentDialog);
    }
}