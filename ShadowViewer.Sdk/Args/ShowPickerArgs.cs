using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace ShadowViewer.Sdk.Args;

/// <summary>
/// 
/// </summary>
public record ShowSinglePickerArgs(
    object Picker,
    TaskCompletionSource<IStorageItem?> ResultSource);
/// <summary>
/// 
/// </summary>
public record ShowMultiPickerArgs(
    object Picker,
    TaskCompletionSource<IReadOnlyList<IStorageItem>?> ResultSource);
