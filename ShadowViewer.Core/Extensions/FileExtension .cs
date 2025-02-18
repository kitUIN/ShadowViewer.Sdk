using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Windows.Storage;
using Windows.System;
using Microsoft.UI.Xaml;
using Serilog;
using ShadowViewer.Core.Helpers;

namespace ShadowViewer.Core.Extensions;

/// <summary>
/// 文件相关拓展
/// </summary>
public static class FileExtension
{
    /// <summary>
    /// 是否是图片
    /// </summary> 
    public static bool IsPic(this StorageFile file)
    {
        return FileHelper.Pngs.ContainsIgnoreCase(file.FileType);
    }

    /// <summary>
    /// 是否是图片
    /// </summary> 
    public static bool IsPic(this string file)
    {
        var extension = Path.GetExtension(file);
        return FileHelper.Pngs.ContainsIgnoreCase(extension);
    }

    /// <summary>
    /// 是否是图片
    /// </summary> 
    public static bool IsPic(this FileInfo file)
    {
        return FileHelper.Pngs.ContainsIgnoreCase(file.Extension);
    }

    /// <summary>
    /// 列表内是否包含字符串,忽略大小写
    /// </summary>
    /// <param name="l"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static bool ContainsIgnoreCase(this string[] l, string value)
    {
        return l.Any(s => s.Equals(value, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 是否是压缩文件
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static bool IsZip(this StorageFile file)
    {
        return FileHelper.Zips.ContainsIgnoreCase(file.FileType);
    }

    public static bool IsFile(this string path)
    {
        return File.Exists(path);
    }

    public static bool IsFolder(this string path)
    {
        return Directory.Exists(path);
    }
}