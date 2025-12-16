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
            handler = new EventHandler([@delegate], [oneshot]);
            handlerMap.Add(eventName, handler);
        }
        handler.Add(@delegate, oneshot);
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
            handler.Remove(@delegate);
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
            var toRemove = new List<Delegate>();
            for (int i = 0; i < handler.delegates.Count; i++)
            {
                var invocation = handler.delegates[i];
                var oneshot = handler.oneshots[i];
                invocation.DynamicInvoke(args);
                if (oneshot) // 只触发一次。
                {
                    toRemove.Add(invocation);// 触发后移除。
                }
            }
            foreach (var invocation in toRemove) // 统一移除。
            {
                handler.Remove(invocation);
            }
        }
    }
}

public class EventHandler(List<Delegate> delegates, List<bool> oneshots)
{
    public List<Delegate> delegates = delegates;
    public List<bool> oneshots = oneshots;
    public int Count => oneshots.Count;
    public void Add(Delegate @delegate, bool oneshot)
    {
        delegates.Add(@delegate);
        oneshots.Add(oneshot);
    }
    public void Remove(Delegate @delegate)
    {
        var index = delegates.IndexOf(@delegate);
        if (index == -1)
        {
            return;
        }
        delegates.RemoveAt(index);
        oneshots.RemoveAt(index);
    }
}