
using GodotMonoGeneral.Logic.ECS;
using GodotMonoGeneral.Utils;

namespace GodotMonoGeneral.Logic;

/// <summary>
/// 逻辑层门面类。
/// </summary>
public class LogicFacade
{
    private LogicFacade() { }


    private static readonly InventorySystem inventory = SingletonFactory.GetSingleton<InventorySystem>();


    /// <summary>
    /// 为实体创建新仓库/背包。
    /// </summary>
    /// <param name="entityId">实体id</param>
    /// <param name="name">仓库/背包名称</param>
    /// <param name="capacity">容量</param>
    public static void CreateInventory(int entityId, string name, int capacity = 0)
    {
        inventory.CreateInventory(entityId, name, capacity);
    }

    /// <summary>
    /// 添加一个物品到指定背包/仓库。
    /// </summary>
    /// <param name="entityId">实体id</param>
    /// <param name="inventoryName">仓库/背包名称</param>
    /// <param name="itemId">物品id</param>
    /// <param name="count">数量</param>
    public static void AddItemToInventory(int entityId, string inventoryName, int itemId, int count = 1)
    {
        
    }

}
