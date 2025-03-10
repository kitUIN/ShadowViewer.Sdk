using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ShadowViewer.Core.Args;

/// <summary>
/// 
/// </summary>
public record ShowSingleFilePickerArgs(
    FileOpenPicker Picker,
    TaskCompletionSource<StorageFile?> ResultSource);

/// <summary>
/// 
/// </summary>
public record ShowMultiFilePickerArgs(
    FileOpenPicker Picker,
    TaskCompletionSource<IReadOnlyList<StorageFile>?> ResultSource);

/// <summary>
/// 
/// </summary>
public record ShowSingleFolderPickerArgs(
    FolderPicker Picker,
    TaskCompletionSource<StorageFolder?> ResultSource);