using System;
using System.Collections.Generic;

namespace GodotMonoGeneral.Utils;

/// <summary>
/// 单例工厂。负责单例的创建。
/// </summary>
public static class SingletonFactory
{

    private static readonly Dictionary<string, object> instances = [];

    public static T GetSingleton<T>() where T : new()
    {
        var type = typeof(T).AssemblyQualifiedName;
        if (!instances.TryGetValue(type, out var instance))
        {
            instance = new T();
            instances.Add(type, instance);
        }
        return (T)instance;
    }

    public static object GetSingleton(string type)
    {
        if (!instances.TryGetValue(type, out var instance))
        {
            var t = Type.GetType(type);
            instance = Activator.CreateInstance(t);
            instances.Add(type, instance);
        }
        return instance;
    }

}
