using System;
using System.Collections.Generic;

namespace GodotMonoGeneral.Utils;

/// <summary>
/// 单例工厂。负责单例的创建。
/// </summary>
public static class SingletonFactory
{
    /// <summary>
    /// 实例字典。
    /// </summary>
    private static readonly Dictionary<string, object> instances = [];

    /// <summary>
    /// 获取指定类型的单例。
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="id">实例唯一标识，当一个类只有一个实例时采用默认值</param>
    /// <returns></returns>
    public static T GetSingleton<T>(string id = "") where T : new()
    {
        var type = typeof(T).AssemblyQualifiedName;
        var key = $"{type}{id}";
        if (!instances.TryGetValue(key, out var instance))
        {
            instance = new T();
            instances.Add(key, instance);
        }
        return (T)instance;
    }

    /// <summary>
    /// 获取指定类型名称的类型单例。
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static object GetSingleton(string type, string id = "")
    {
        var key = $"{type}{id}";
        if (!instances.TryGetValue(key, out var instance))
        {
            var t = Type.GetType(type);
            instance = Activator.CreateInstance(t);
            instances.Add(key, instance);
        }
        return instance;
    }

    /// <summary>
    /// 自定义实例化方法获取单例。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instantiate"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static T GetSingleton<T>(Func<T> instantiate, string id = "")
    {
        var type = typeof(T).AssemblyQualifiedName;
        var key = $"{type}{id}";
        if (!instances.TryGetValue(key, out var instance))
        {
            instance = instantiate();
            instances.Add(key, instance);
        }
        return (T) instance;
    }



}
