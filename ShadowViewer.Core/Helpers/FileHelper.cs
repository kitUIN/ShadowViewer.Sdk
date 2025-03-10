using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml;
using Serilog;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using ShadowViewer.Core.Args;

namespace ShadowViewer.Core.Helpers;
/// <summary>
/// 文件帮助类
/// </summary>
public class FileHelper
{
    /// <summary>
    /// 常见图片后缀
    /// </summary>
    public static string[] Pngs => [".png", ".jpg", ".jpeg", ".bmp"];
    /// <summary>
    /// 常见压缩包后缀
    /// </summary>
    public static string[] Zips => [".zip", ".rar", ".7z", ".tar"];

    /// <summary>
    /// 选择文件夹
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="settingsIdentifier"></param>
    /// <returns></returns>
    public static async Task<StorageFolder?> SelectFolderAsync(string accessToken = "", string settingsIdentifier = "")
    {
        var openPicker = new FolderPicker();
        if (!string.IsNullOrEmpty(settingsIdentifier)) openPicker.SettingsIdentifier = settingsIdentifier;
        else openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
        openPicker.FileTypeFilter.Add("*");
        var resultSource = new TaskCompletionSource<StorageFolder?>();
        WeakReferenceMessenger.Default.Send(new ShowSingleFolderPickerArgs(openPicker, resultSource));
        var folder = await resultSource.Task;
        if (folder == null) return null;
        if (accessToken != "")
        {
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(accessToken, folder);
        }
        Log.ForContext<FolderPicker>().Information("选择了文件夹:{Path}", folder.Path);
        return folder;
    }

    /// <summary>
    /// 选择单个文件
    /// </summary>
    public static async Task<StorageFile?> SelectFileAsync(string settingsIdentifier, PickerViewMode mode, params string[] filter)
    {
        var openPicker = new FileOpenPicker();
        if (!string.IsNullOrEmpty(settingsIdentifier)) openPicker.SettingsIdentifier = settingsIdentifier;
        else openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
        openPicker.ViewMode = mode;
        foreach (var filterItem in filter) openPicker.FileTypeFilter.Add(filterItem);
        var resultSource = new TaskCompletionSource<StorageFile?>();
        WeakReferenceMessenger.Default.Send(new ShowSingleFilePickerArgs(openPicker, resultSource));
        var file = await resultSource.Task;
        if (file == null) return null;
        Log.ForContext<FileOpenPicker>().Information("选择了文件:{Path}", file.Path);
        return file;
    }

    /// <summary>
    /// 选择多个文件
    /// </summary>
    public static async Task<IReadOnlyList<IStorageItem>> SelectMultipleFileAsync(
        string settingsIdentifier, PickerViewMode mode, params string[] filter)
    {
        var openPicker = new FileOpenPicker();
        if (!string.IsNullOrEmpty(settingsIdentifier)) openPicker.SettingsIdentifier = settingsIdentifier;
        else openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
        openPicker.ViewMode = mode;
        foreach (var filterItem in filter) openPicker.FileTypeFilter.Add(filterItem);
        var resultSource = new TaskCompletionSource<IReadOnlyList<StorageFile>?>();
        WeakReferenceMessenger.Default.Send(new ShowMultiFilePickerArgs(openPicker, resultSource));
        var files = await resultSource.Task;
        if (files == null) return [];
        Log.ForContext<FileOpenPicker>().Information("选择了{Count}个文件:{Path}", files.Count, files.Select(x => x.Path));
        return files;
    }
}