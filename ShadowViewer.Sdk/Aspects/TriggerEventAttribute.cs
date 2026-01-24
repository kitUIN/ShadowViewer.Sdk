#pragma warning disable CS0067 // 事件从未使用过
namespace ShadowViewer.Sdk.Aspects;

using Metalama.Framework.Aspects;
using Metalama.Framework.Code;
using Microsoft.UI.Xaml;
using System;
using System.Linq;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Metalama.Framework.Aspects.TypeAspect" />
public class TriggerEventAttribute : TypeAspect
{
    /// <summary>
    /// 
    /// </summary>
    [Introduce(Name = "OnCreatedHook")]
    public static event EventHandler? Created;

    /// <summary>
    /// 
    /// </summary>
    [Introduce(Name = "OnLoadedHook")]
    public static event RoutedEventHandler? Loaded;

    /// <summary>
    /// </summary>
    /// <param name="builder"></param>
    /// <inheritdoc />
    public override void BuildAspect(IAspectBuilder<INamedType> builder)
    {
        var constructors = builder.Target.Constructors.Where(c => !c.IsStatic);

        foreach (var constructor in constructors)
        {
            builder.With(constructor).Override(nameof(this.ConstructorTemplate));
        }
    }


    [Template]
    private void ConstructorTemplate()
    {
        meta.Proceed();
        meta.This.Loaded -= new RoutedEventHandler(RunRoutedEvent);
        meta.This.Loaded += new RoutedEventHandler(RunRoutedEvent);
        meta.ThisType.OnCreatedHook?.Invoke(meta.This, EventArgs.Empty);
        return;

        static void RunRoutedEvent(object sender, RoutedEventArgs args)
        {
            meta.ThisType.OnLoadedHook?.Invoke(sender, args);
        }
    }
}