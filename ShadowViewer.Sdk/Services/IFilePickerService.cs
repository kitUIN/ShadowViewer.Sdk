using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ShadowViewer.Sdk.Services;

/// <summary>
/// 文件和文件夹选择服务接口
/// </summary>
public interface IFilePickerService
{
    /// <summary>
    /// 选择单个文件
    /// </summary>
    /// <param name="fileTypeFilter">文件类型过滤器，例如 [".jpg", ".png"]</param>
    /// <param name="suggestedStartLocation">建议的起始位置</param>
    /// <param name="viewMode">视图模式</param>
    /// <param name="settingsIdentifier">设置标识符，用于记住上次使用的位置</param>
    /// <returns>选择的文件，如果取消则返回null</returns>
    /// <example>
    /// var file = await FilePickerService.PickSingleFileAsync([".txt", ".log"]);
    /// </example>
    Task<StorageFile?> PickSingleFileAsync(
        IList<string>? fileTypeFilter = null,
        PickerLocationId suggestedStartLocation = PickerLocationId.DocumentsLibrary,
        PickerViewMode viewMode = PickerViewMode.List,
        string? settingsIdentifier = null);

    /// <summary>
    /// 选择多个文件
    /// </summary>
    /// <param name="fileTypeFilter">文件类型过滤器，例如 [".jpg", ".png"]</param>
    /// <param name="suggestedStartLocation">建议的起始位置</param>
    /// <param name="viewMode">视图模式</param>
    /// <param name="settingsIdentifier">设置标识符，用于记住上次使用的位置</param>
    /// <returns>选择的文件列表，如果取消则返回空列表</returns>
    /// <example>
    /// var files = await FilePickerService.PickMultipleFilesAsync([".jpg", ".png"]);
    /// </example>
    Task<IReadOnlyList<StorageFile>> PickMultipleFilesAsync(
        IList<string>? fileTypeFilter = null,
        PickerLocationId suggestedStartLocation = PickerLocationId.DocumentsLibrary,
        PickerViewMode viewMode = PickerViewMode.List,
        string? settingsIdentifier = null);

    /// <summary>
    /// 选择文件夹
    /// </summary>
    /// <param name="suggestedStartLocation">建议的起始位置</param>
    /// <param name="viewMode">视图模式</param>
    /// <param name="settingsIdentifier">设置标识符，用于记住上次使用的位置</param>
    /// <returns>选择的文件夹，如果取消则返回null</returns>
    /// <example>
    /// var folder = await FilePickerService.PickFolderAsync();
    /// </example>
    Task<StorageFolder?> PickFolderAsync(
        PickerLocationId suggestedStartLocation = PickerLocationId.DocumentsLibrary,
        PickerViewMode viewMode = PickerViewMode.List,
        string? settingsIdentifier = null);

    /// <summary>
    /// 保存文件
    /// </summary>
    /// <param name="suggestedFileName">建议的文件名</param>
    /// <param name="fileTypeChoices">文件类型选择，例如 { { "文本文件", [".txt"] }, { "所有文件", ["*"] } }</param>
    /// <param name="suggestedStartLocation">建议的起始位置</param>
    /// <param name="settingsIdentifier">设置标识符，用于记住上次使用的位置</param>
    /// <returns>保存的文件，如果取消则返回null</returns>
    /// <example>
    /// var file = await FilePickerService.PickSaveFileAsync(
    ///     "output.txt",
    ///     new Dictionary&lt;string, IList&lt;string&gt;&gt; { { "文本文件", new[] { ".txt" } } });
    /// </example>
    Task<StorageFile?> PickSaveFileAsync(
        string? suggestedFileName = null,
        IDictionary<string, IList<string>>? fileTypeChoices = null,
        PickerLocationId suggestedStartLocation = PickerLocationId.DocumentsLibrary,
        string? settingsIdentifier = null);
}
