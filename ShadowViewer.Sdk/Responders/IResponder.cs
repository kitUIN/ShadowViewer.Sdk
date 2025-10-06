namespace ShadowViewer.Sdk.Responders;

/// <summary>
/// 触发器基类
/// </summary>
public interface IResponder
{
    /// <summary>
    /// 插件ID
    /// </summary>
    string Id { get; }
}