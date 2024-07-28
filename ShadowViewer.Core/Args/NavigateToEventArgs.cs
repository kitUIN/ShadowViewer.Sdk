namespace ShadowViewer.Args;

/// <summary>
/// 跳转页面事件
/// </summary>
public class NavigateToEventArgs(Type page, object? parameter, bool force) : EventArgs
{
    public object? Parameter { get; } = parameter;
    public Type Page { get; } = page;
    public bool Force { get; } = force;
    /// <summary>
    /// ToString
    /// </summary>
    public new string ToString() => $"{{NavigateToEventArgs,Page={Page},Force={force},Parameter={Parameter}}}";
}