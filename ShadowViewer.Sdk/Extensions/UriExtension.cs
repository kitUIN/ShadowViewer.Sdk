using Microsoft.UI.Xaml;
using Microsoft.VisualBasic.FileIO;
using Serilog;
using ShadowViewer.Sdk.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Windows.Storage;
using Windows.System;

namespace ShadowViewer.Sdk.Extensions;

/// <summary>
/// 
/// </summary>
public static class UriExtension
{
    private static ILogger Logger { get; } = Log.ForContext<FileHelper>();

    /// <summary>
    /// 从浏览器打开
    /// </summary>
    public static async void LaunchUriAsync(this Uri uri)
    {
        try
        {
            await Launcher.LaunchUriAsync(uri);
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while launching the URI.");
        }
    }

    /// <summary>
    /// 从资源管理器打开文件夹
    /// </summary>
    public static async void LaunchFolderAsync(this StorageFolder folder)
    {
        try
        {
            await Launcher.LaunchFolderAsync(folder);
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while launching the folder.");
        }
    }

    /// <summary>
    /// 从资源管理器打开文件夹
    /// </summary>
    public static async void LaunchFolderAsync(this string path)
    {
        try
        {
            var folder = await path.ToStorageFolder();
            folder.LaunchFolderAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while launching the folder.");
        }
    }

    /// <summary>
    /// 从资源管理器打开文件
    /// </summary>
    public static async void LaunchFolderAsync(this Uri uri)
    {
        try
        {
            var folder = await uri.DecodePath().ToStorageFolder();
            folder.LaunchFolderAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while launching the folder.");
        }
    }

    /// <summary>
    /// 从资源管理器打开文件
    /// </summary>
    public static async void LaunchFileAsync(this StorageFile folder)
    {
        try
        {
            await Launcher.LaunchFileAsync(folder);
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while launching the file.");
        }
    }

    /// <summary>
    /// 从资源管理器打开文件
    /// </summary>
    public static async void LaunchFileAsync(this Uri uri)
    {
        try
        {
            var file = await uri.GetFile();
            file.LaunchFileAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while launching the file.");
        }
    }

    /// <summary>
    /// 从资源管理器打开文件
    /// </summary>
    public static async void LaunchFileAsync(this string uri)
    {
        try
        {
            var file = await uri.GetFile();
            file.LaunchFileAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while launching the file.");
        }
    }

    /// <summary>
    /// Join
    /// </summary> 
    public static string JoinToString(this ObservableCollection<string> tags, string separator = ",")
    {
        return string.Join(separator, tags);
    }

    /// <summary>
    /// 获取文件
    /// </summary>
    public static async Task<StorageFile> GetFile(this Uri uri)
    {
        return await uri.DecodePath().GetFile();
    }

    /// <summary>
    /// 获取文件
    /// </summary>
    public static async Task<StorageFile> GetFile(this string path)
    {
        return await StorageFile.GetFileFromPathAsync(path);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string DecodePath(this StorageFile file)
    {
        return HttpUtility.UrlDecode(file.Path);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static string DecodePath(this Uri uri)
    {
        return HttpUtility.UrlDecode(uri.AbsolutePath);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    public static string DecodeUri(this Uri uri)
    {
        return HttpUtility.UrlDecode(uri.AbsoluteUri);
    }

    /// <summary>
    /// 从url获取StorageFolder,若没有则创建文件夹
    /// </summary>
    public static async Task<StorageFolder> ToStorageFolder(this string path)
    {
        path.CreateDirectory();
        return await StorageFolder.GetFolderFromPathAsync(path);
    }

    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="path"></param>
    public static void CreateDirectory(this string path)
    {
        if (Directory.Exists(path)) return;
        Directory.CreateDirectory(path);
        Logger.Information("文件夹 {Dir} 不存在, 新建", path);
    }

    /// <summary>
    /// 删除文件夹
    /// </summary>
    public static void DeleteDirectory(this string targetDir, bool recursive = true, bool recycleBin = false)
    {
        if (!Directory.Exists(targetDir))
        {
            return;
        }

        try
        {
            if (recycleBin)
            {
                FileSystem.DeleteDirectory(targetDir, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            }
            else
            {
                Directory.Delete(targetDir, recursive);
            }

            Logger.Information("删除文件夹 {Dir}", targetDir);
        }
        catch (IOException ex)
        {
            Logger.Error(ex, "删除文件夹 {Dir} 失败", targetDir);
        }
        catch (UnauthorizedAccessException ex)
        {
            // 如果遇到只读文件，可以在这里处理
            Logger.Error(ex, "删除文件夹 {Dir} 权限不足", targetDir);
        }
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    public static void CreateFile(this string path)
    {
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
            Logger.Information("文件夹 {Dir} 不存在, 新建", dir);
        }

        if (File.Exists(path)) return;
        using (File.Create(path))
        {
        } // 确保释放句柄

        Logger.Information("文件 {File} 不存在, 新建", path);
    }

    /// <summary>
    /// 从url获取StorageFile,若没有则创建文件
    /// </summary>
    public static async Task<StorageFile> ToStorageFile(this string path)
    {
        path.CreateFile();
        return await StorageFile.GetFileFromPathAsync(path);
    }

    /// <summary>
    /// true -> Visible <br/>
    /// false -> Collapsed
    /// </summary>
    public static Visibility ToVisibility(this bool flag)
    {
        return flag ? Visibility.Visible : Visibility.Collapsed;
    }
}