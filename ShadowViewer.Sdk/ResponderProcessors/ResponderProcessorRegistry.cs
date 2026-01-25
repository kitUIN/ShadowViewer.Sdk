using System.Collections.Generic;

namespace ShadowViewer.Sdk.ResponderProcessors;

/// <summary>
/// 
/// </summary>
public static class ResponderProcessorRegistry
{
    private static readonly Dictionary<string, IResponderProcessor> Processors = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="processor"></param>
    public static void Register(IResponderProcessor processor)
    {
        foreach (var name in processor.SupportResponderName)
        {
            Processors[name] = processor;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="processor"></param>
    /// <returns></returns>
    public static bool TryGetProcessor(string name, out IResponderProcessor? processor)
        => Processors.TryGetValue(name, out processor);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static IResponderProcessor? GetProcessor(string name)
        => Processors[name];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool ContainProcessorName(string name) => Processors.ContainsKey(name);
}