using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace ShadowViewer.Sdk.Args;

/// <summary>
/// 
/// </summary>
public record ShowDialogMessageArgs(
    ContentDialog ContentDialog,
    TaskCompletionSource<ContentDialogResult> ResultSource);