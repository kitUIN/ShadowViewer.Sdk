using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using Serilog;
using ShadowViewer.Core.Args;
using ShadowViewer.Core.Helpers;

namespace ShadowViewer.Core.Controls;

/// <summary>
/// 用于显示ContentDialog/FilePicker/FolderPicker
/// </summary>
public partial class ShadowPage : Page
{
    /// <inheritdoc />
    public ShadowPage()
    {
        WeakReferenceMessenger.Default.Register<ShowDialogMessageArgs>(this, async void (r, m) =>
        {
            try
            {
                m.ContentDialog.XamlRoot = this.XamlRoot;
                var result = await m.ContentDialog.ShowAsync();
                m.ResultSource.SetResult(result);
            }
            catch (Exception e)
            {
                Log.Error("ShowDialog: {e}", e);
            }
        });
        WeakReferenceMessenger.Default.Register<ShowSingleFilePickerArgs>(this, async void (r, m) =>
        {
            try
            {
                if (WindowHelper.GetWindow(this) is not { } window) return;
                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                WinRT.Interop.InitializeWithWindow.Initialize(m.Picker, hWnd);
                var result = await m.Picker.PickSingleFileAsync();
                m.ResultSource.SetResult(result);
            }
            catch (Exception e)
            {
                Log.Error("ShowSingleFilePicker: {e}", e);
            }
        });
        WeakReferenceMessenger.Default.Register<ShowMultiFilePickerArgs>(this, async void (r, m) =>
        {
            try
            {
                if (WindowHelper.GetWindow(this) is not { } window) return;
                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                WinRT.Interop.InitializeWithWindow.Initialize(m.Picker, hWnd);
                var result = await m.Picker.PickMultipleFilesAsync();
                m.ResultSource.SetResult(result);
            }
            catch (Exception e)
            {
                Log.Error("ShowSingleFilePicker: {e}", e);
            }
        });
        WeakReferenceMessenger.Default.Register<ShowSingleFolderPickerArgs>(this, async void (r, m) =>
        {
            try
            {
                if (WindowHelper.GetWindow(this) is not { } window) return;
                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                WinRT.Interop.InitializeWithWindow.Initialize(m.Picker, hWnd);
                var result = await m.Picker.PickSingleFolderAsync();
                m.ResultSource.SetResult(result);
            }
            catch (Exception e)
            {
                Log.Error("ShowSingleFilePicker: {e}", e);
            }
        });
    }

     
}