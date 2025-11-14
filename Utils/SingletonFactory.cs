namespace GodotMonoGeneral.Utils;

/// <summary>
/// 单例工厂。负责单例的创建。
/// </summary>
public static class SingletonFactory
{
    public static T GetSingleton<T>() where T : new()
    {
        return Factory<T>.Instance;
    }

    class Factory<T> where T : new()
    {
        static T instance;
        public static T Instance => instance ??= new T();
    }

}
