using System;

namespace ShadowViewer.Core.Args;

/// <summary>
/// 尝试选中左侧导航栏项
/// </summary>
/// <param name="Id"></param>
public record TrySelectItemEventArgs(string? Id);