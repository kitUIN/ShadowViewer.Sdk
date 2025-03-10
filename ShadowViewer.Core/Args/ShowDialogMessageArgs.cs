using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace ShadowViewer.Core.Args;

/// <summary>
/// 
/// </summary>
public record ShowDialogMessageArgs(
    ContentDialog ContentDialog,
    TaskCompletionSource<ContentDialogResult> ResultSource);