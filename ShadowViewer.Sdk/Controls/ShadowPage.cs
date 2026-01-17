using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using Serilog;
using ShadowViewer.Sdk.Args;
using ShadowViewer.Sdk.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ShadowViewer.Sdk.Controls;

/// <summary>
/// 用于显示ContentDialog/FilePicker/FolderPicker
/// </summary>
public partial class ShadowPage : Page
{
    private static readonly SemaphoreSlim DialogSemaphore = new(1, 1);

    /// <inheritdoc />
    public ShadowPage()
    {
        WeakReferenceMessenger.Default.Register<ShowDialogMessageArgs>(this, async void (_, m) =>
        {
            try
            {
                m.ContentDialog.XamlRoot = this.XamlRoot;
                var result = await ShowQueuedDialogAsync(m.ContentDialog);
                m.ResultSource.SetResult(result);
            }
            catch (Exception e)
            {
                Log.Error("ShowDialog: {e}", e);
            }
        });
        WeakReferenceMessenger.Default.Register<ShowSinglePickerArgs>(this, async void (_, m) =>
        {
            try
            {
                if (WindowHelper.GetWindow(this) is not { } window) return;
                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                WinRT.Interop.InitializeWithWindow.Initialize(m.Picker, hWnd);
                IStorageItem? result = m.Picker switch
                {
                    FileSavePicker saver => await saver.PickSaveFileAsync(),
                    FileOpenPicker opener => await opener.PickSingleFileAsync(),
                    FolderPicker folder => await folder.PickSingleFolderAsync(),
                    _ => throw new NotSupportedException($"Unsupported picker type: {m.Picker.GetType()}")
                };
                m.ResultSource.SetResult(result);
            }
            catch (Exception e)
            {
                Log.Error("ShowSingleFilePicker: {e}", e);
            }
        });
        WeakReferenceMessenger.Default.Register<ShowMultiPickerArgs>(this, async void (_, m) =>
        {
            try
            {
                if (WindowHelper.GetWindow(this) is not { } window) return;
                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                WinRT.Interop.InitializeWithWindow.Initialize(m.Picker, hWnd);
                IReadOnlyList<IStorageItem>? result = m.Picker switch
                {
                    FileOpenPicker opener => await opener.PickMultipleFilesAsync(),
                    _ => throw new NotSupportedException($"Unsupported picker type: {m.Picker.GetType()}")
                };
                m.ResultSource.SetResult(result);
            }
            catch (Exception e)
            {
                Log.Error("ShowSingleFilePicker: {e}", e);
            }
        });
    }
    /// <summary>
    /// Shows the queued dialog asynchronous.
    /// </summary>
    /// <param name="dialog">The dialog.</param>
    /// <returns></returns>
    private static async Task<ContentDialogResult> ShowQueuedDialogAsync(ContentDialog dialog)
    {
        await DialogSemaphore.WaitAsync();

        try
        {
            return await dialog.ShowAsync();
        }
        finally
        {
            DialogSemaphore.Release();
        }
    }
}