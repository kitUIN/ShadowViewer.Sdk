using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Sdk.Args;

namespace ShadowViewer.Sdk.Helpers;
/// <summary>
/// Dialog帮助类
/// </summary>
public class DialogHelper
{

    /// <summary>
    /// 显示Dialog
    /// </summary> 
    public static async Task<ContentDialogResult> ShowDialog(ContentDialog dialog)
    {
        var resultSource = new TaskCompletionSource<ContentDialogResult>();
        WeakReferenceMessenger.Default.Send(new ShowDialogMessageArgs(dialog, resultSource));
        return await resultSource.Task;
    }
}