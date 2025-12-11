using System;
using System.Collections.Generic;

namespace GodotMonoGeneral.Utils;

public class EventBus
{
     private readonly Dictionary<string, EventHandler> handlerMap = [];

    /// <summary>
    /// 订阅事件。
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <param name="handler">处理函数</param>
    /// <param name="oneshot">是否只触发一次</param>
    public static void Subscribe(string eventName, Delegate @delegate, bool oneshot = false)
    {
        var handlerMap = SingletonFactory.GetSingleton<EventBus>().handlerMap;
        if (!handlerMap.TryGetValue(eventName, out var handler))
        {
            handler = new EventHandler(@delegate, oneshot);
            handlerMap.Add(eventName, handler);
        }
        handler.delegates = Delegate.Combine(handler.delegates, @delegate);
    }
    /// <summary>
    /// 取消订阅事件。
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <param name="handler">处理函数</param>
    public static void Unsubscribe(string eventName, Delegate @delegate)
    {
        var handlerMap = SingletonFactory.GetSingleton<EventBus>().handlerMap;
        if (handlerMap.TryGetValue(eventName, out var handler))
        {
            handler.delegates = Delegate.Remove(handler.delegates, @delegate);
        }
    }
    /// <summary>
    /// 触发事件。
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <param name="args">事件参数</param>
    public static void Raise(string eventName, params object[] args)
    {
        var handlerMap = SingletonFactory.GetSingleton<EventBus>().handlerMap;
        if (handlerMap.TryGetValue(eventName, out var handler))
        {
            handler.delegates.DynamicInvoke(args);
        }
    }
}

public class EventHandler(Delegate delegates, bool oneshot)
{
    public Delegate delegates = delegates;
    public bool oneshot = oneshot;
}