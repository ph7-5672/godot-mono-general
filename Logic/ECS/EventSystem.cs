using GodotMonoGeneral.Utils;

namespace GodotMonoGeneral.Logic.ECS;

/// <summary>
/// 事件系统。
/// </summary>
public class EventSystem
{
    private ECSWorld World => SingletonFactory.GetSingleton<ECSWorld>();
    /// <summary>
    /// 事件实体，用以挂载事件组件。
    /// </summary>
    private int eventEntity = -1;
    /// <summary>
    /// 懒加载事件实体。
    /// </summary>
    private void CreateEventEntity()
    {
        if (eventEntity == -1)
        {
            eventEntity = World.CreateEntity();
        }
    }
    /// <summary>
    /// 触发事件。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventArgs"></param>
    public void RaiseEvent<T>(T eventArgs) where T : struct
    {
        CreateEventEntity();
        World.AddComponent(eventEntity, ref eventArgs);
    }

    /// <summary>
    /// 释放所有事件。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void ReleaseEvents()
    {
        CreateEventEntity();
        World.DeleteComponents(eventEntity);
    }

    /// <summary>
    /// 尝试获取事件。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool TryGetEvent<T>(out T eventArgs) where T : struct
    {
        CreateEventEntity();
        if (!World.HasComponent<T>(eventEntity))
        {
            eventArgs = default;
            return false;
        }
        eventArgs = World.GetComponent<T>(eventEntity);
        return true;
    }

}

/// <summary>
/// 保存游戏事件。
/// </summary>
/// <param name="index"></param>
public struct SaveGameEvent(int index)
{
    public int index = index;
}
/// <summary>
/// 加载存档事件。
/// </summary>
/// <param name="index"></param>
public struct LoadSaveEvent(int index)
{
    public int index = index;
}

