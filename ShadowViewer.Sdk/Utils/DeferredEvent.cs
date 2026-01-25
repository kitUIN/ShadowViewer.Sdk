using System;
using System.Collections.Generic;

namespace ShadowViewer.Sdk.Utils;

/// <summary>
/// 提供一种“延迟触发”事件模型：
/// 当没有订阅者时，事件参数会被缓存；
/// 当订阅者出现后，会自动触发所有缓存的事件，并在之后正常触发。
/// </summary>
/// <typeparam name="TEventArgs">事件参数类型。</typeparam>
public class DeferredEvent<TEventArgs>
{
    /// <summary>
    /// 用于保证事件订阅、取消订阅与事件缓存的线程安全。
    /// </summary>
    private readonly object @lock = new();

    /// <summary>
    /// 实际的事件处理器委托链。
    /// </summary>
    private EventHandler<TEventArgs>? handler;

    /// <summary>
    /// 在没有订阅者时缓存的事件参数队列。
    /// </summary>
    private readonly Queue<TEventArgs> pending = new();

    /// <summary>
    /// 订阅事件。
    /// 如果存在缓存的事件，将在订阅时立即按顺序触发这些事件。
    /// </summary>
    /// <param name="eventHandler">事件处理器。</param>
    public void Subscribe(EventHandler<TEventArgs>? eventHandler)
    {
        if (eventHandler == null) return;
        lock (@lock)
        {
            this.handler += eventHandler;

            // 订阅时触发所有缓存的事件
            while (pending.Count > 0)
            {
                var args = pending.Dequeue();
                eventHandler(this, args);
            }
        }
    }

    /// <summary>
    /// 取消订阅事件。
    /// </summary>
    /// <param name="eventHandler">要移除的事件处理器。</param>
    public void Unsubscribe(EventHandler<TEventArgs>? eventHandler)
    {
        if (eventHandler == null) return;
        lock (@lock)
        {
            this.handler -= eventHandler;
        }
    }

    /// <summary>
    /// 触发事件。
    /// 如果当前没有订阅者，则事件参数会被缓存，等待未来订阅者出现后再触发。
    /// </summary>
    /// <param name="sender">事件发送者。</param>
    /// <param name="args">事件参数。</param>
    public void Raise(object sender, TEventArgs args)
    {
        EventHandler<TEventArgs>? h;

        lock (@lock)
        {
            if (handler == null)
            {
                // 没有订阅者 → 缓存事件
                pending.Enqueue(args);
                return;
            }

            h = handler;
        }

        // 有订阅者 → 立即触发
        h?.Invoke(sender, args);
    }

    /// <summary>
    /// 使用 += 订阅事件。
    /// </summary> 
    public static DeferredEvent<TEventArgs> operator +(DeferredEvent<TEventArgs> evt, EventHandler<TEventArgs> handler)
    {
        evt.Subscribe(handler);
        return evt;
    }

    /// <summary>
    /// 使用 -= 取消订阅事件。
    /// </summary> 
    public static DeferredEvent<TEventArgs> operator -(DeferredEvent<TEventArgs> evt, EventHandler<TEventArgs> handler)
    {
        evt.Unsubscribe(handler);
        return evt;
    }
}