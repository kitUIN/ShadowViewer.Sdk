using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;
using ShadowViewer.Core.Extensions;

namespace ShadowViewer.Core.Models;

/// <summary>
/// 树形文件结构
/// </summary>
public class ShadowTreeNode
{
    /// <summary>
    /// 文件(夹)名
    /// </summary>
    public string Name { get; init; } = "";

    /// <summary>
    /// 路径
    /// </summary>
    public string Path { get; init; } = "";

    /// <summary>
    /// 深度,当前节点距离根节点的层级数量
    /// </summary>
    public int Depth { get; init; }

    /// <summary>
    /// 高度,当前节点距离最小子节点的层级数量
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 存储大小
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// 是否是文件夹
    /// </summary>
    public bool IsDirectory { get; init; }

    /// <summary>
    /// 是否是图片
    /// </summary>
    public bool IsPic => Name.IsPic();

    /// <summary>
    /// 子节点
    /// </summary>
    public List<ShadowTreeNode> Children { get; } = [];

    /// <summary>
    /// 文件夹转为树形结构
    /// </summary>
    /// <param name="folder">选择的文件夹</param>
    /// <returns>树形结构顶层节点</returns>
    public static ShadowTreeNode FromFolder(StorageFolder folder)
    {
        return FromFolder(folder.Path);
    }

    /// <summary>
    /// 文件夹路径转为树形结构
    /// </summary>
    /// <param name="folderPath">文件夹路径</param>
    /// <returns>树形结构顶层节点</returns>
    public static ShadowTreeNode FromFolder(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            throw new Exception($"folderPath {folderPath} Is not a directory");
        }

        var currentDir = new DirectoryInfo(folderPath);
        var rootNode = new ShadowTreeNode
        {
            Name = currentDir.Name,
            Path = folderPath,
            IsDirectory = true,
            Depth = 0
        };

        ProcessFolder(currentDir, rootNode);

        return rootNode;
    }

    /// <summary>
    /// 递归处理文件夹
    /// </summary>
    /// <param name="currentDir"></param>
    /// <param name="parentNode"></param>
    private static void ProcessFolder(DirectoryInfo currentDir, ShadowTreeNode parentNode)
    {
        foreach (var file in currentDir.EnumerateFiles()
                     .Where(x => x.Extension.IsPic()))
        {
            var fileNode = new ShadowTreeNode
            {
                Name = file.Name,
                Path = file.FullName,
                Size = file.Length,
                Depth = parentNode.Depth + 1,
                Count = 1,
                IsDirectory = false
            };

            parentNode.Size += file.Length;
            parentNode.Count++;
            parentNode.Children.Add(fileNode);
        }

        var directories = currentDir.GetDirectories();
        foreach (var dir in directories)
        {
            var dirNode = new ShadowTreeNode
            {
                Name = dir.Name,
                Path = dir.FullName,
                IsDirectory = true,
                Depth = parentNode.Depth + 1
            };
            parentNode.Children.Add(dirNode);
            ProcessFolder(dir, dirNode);
            parentNode.Size += dirNode.Size;
            parentNode.Count++;
        }

        parentNode.Height = parentNode.Children.Count == 0 ? 0 : parentNode.Children.Max(child => child.Height) + 1;
    }

    /// <summary>
    /// 展平深度为{depth}的树形结构
    /// </summary>
    /// <param name="depth">深度,0为当前层</param>
    /// <returns>平铺列表</returns>
    public List<ShadowTreeNode> GetFilesByDepth(int depth = 1)
    {
        if (Depth == depth) return [this];
        var result = new List<ShadowTreeNode>();
        foreach (var child in Children)
        {
            result.AddRange(child.GetFilesByDepth(depth));
        }

        return result;
    }

    /// <summary>
    /// 展平高度的树形结构
    /// </summary>
    /// <param name="height">高度,0为最小节点层</param>
    /// <returns>平铺列表</returns>
    public List<ShadowTreeNode> GetFilesByHeight(int height)
    {
        if (Height == height) return [this];
        var result = new List<ShadowTreeNode>();
        foreach (var child in Children)
        {
            result.AddRange(child.GetFilesByHeight(height));
        }

        return result;
    }
}