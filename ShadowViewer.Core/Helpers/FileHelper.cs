namespace ShadowViewer.Helpers;
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
    /// <param name="element"></param>
    /// <param name="accessToken"></param>
    /// <param name="settingsIdentifier"></param>
    /// <returns></returns>
    public static async Task<StorageFolder?> SelectFolderAsync(UIElement element, string accessToken = "", string settingsIdentifier = "")
    {
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(WindowHelper.GetWindow(element));
        var openPicker = new FolderPicker();
        if (!string.IsNullOrEmpty(settingsIdentifier)) openPicker.SettingsIdentifier = settingsIdentifier;
        else openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
        openPicker.FileTypeFilter.Add("*");
        var folder = await openPicker.PickSingleFolderAsync();
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
    public static async Task<StorageFile?> SelectFileAsync(Window window, string settingsIdentifier, PickerViewMode mode, params string[] filter)
    {
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        var openPicker = new FileOpenPicker();
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
        if (!string.IsNullOrEmpty(settingsIdentifier)) openPicker.SettingsIdentifier = settingsIdentifier;
        else openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
        openPicker.ViewMode = mode;
        foreach (var filterItem in filter) openPicker.FileTypeFilter.Add(filterItem);
        var file = await openPicker.PickSingleFileAsync();
        if (file == null) return null;
        Log.ForContext<FileOpenPicker>().Information("选择了文件:{Path}", file.Path);
        return file;
    }
    /// <summary>
    /// 选择单个文件
    /// </summary>
    public static async Task<StorageFile?> SelectFileAsync(UIElement element, string settingsIdentifier, PickerViewMode mode, params string[] filter)
    {
        if (WindowHelper.GetWindow(element) is { } window)
        {
            return await SelectFileAsync(window, settingsIdentifier, mode, filter);
        }
        return null;
    }

    /// <summary>
    /// 选择单个文件
    /// </summary>
    public static async Task<StorageFile?> SelectFileAsync(XamlRoot xamlRoot, string settingsIdentifier, PickerViewMode mode, params string[] filter)
    {
        if (WindowHelper.GetWindow(xamlRoot) is { } window)
        {
            return await SelectFileAsync(window, settingsIdentifier, mode, filter);
        }
        return null;
    }

    /// <summary>
    /// 选择多个文件
    /// </summary>
    public static async Task<IReadOnlyList<IStorageItem>> SelectMultipleFileAsync(
        Window window, string settingsIdentifier, PickerViewMode mode, params string[] filter)
    {
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        var openPicker = new FileOpenPicker();
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
        if (!string.IsNullOrEmpty(settingsIdentifier)) openPicker.SettingsIdentifier = settingsIdentifier;
        else openPicker.SuggestedStartLocation = PickerLocationId.Downloads;
        openPicker.ViewMode = mode;
        foreach (var filterItem in filter) openPicker.FileTypeFilter.Add(filterItem);
        var files = await openPicker.PickMultipleFilesAsync();
        if (files == null) return [];
        Log.ForContext<FileOpenPicker>().Information("选择了{Count}个文件:{Path}", files.Count, files.Select(x => x.Path));
        return files;
    }
    /// <summary>
    /// 选择多个文件
    /// </summary>
    public static async Task<IReadOnlyList<IStorageItem>> SelectMultipleFileAsync(
        UIElement element, string settingsIdentifier, PickerViewMode mode, params string[] filter)
    {
        if (WindowHelper.GetWindow(element) is { } window)
        {
            return await SelectMultipleFileAsync(window, settingsIdentifier, mode, filter);
        }
        return [];
    }

    /// <summary>
    /// 选择多个文件
    /// </summary>
    public static async Task<IReadOnlyList<IStorageItem>> SelectMultipleFileAsync(
        XamlRoot xamlRoot, string settingsIdentifier, PickerViewMode mode, params string[] filter)
    {
        if (WindowHelper.GetWindow(xamlRoot) is { } window)
        {
            return await SelectMultipleFileAsync(window, settingsIdentifier, mode, filter);
        }
        return [];
    }
}