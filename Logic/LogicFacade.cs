
using GodotMonoGeneral.Logic.ECS;
using GodotMonoGeneral.Utils;

namespace GodotMonoGeneral.Logic;

/// <summary>
/// 逻辑层门面类。
/// </summary>
public class LogicFacade
{
    private LogicFacade() { }

    private static InventorySystem Inventory => SingletonFactory.GetSingleton<InventorySystem>();

    /// <summary>
    /// 获取指定仓库/背包下指定的格子数据。
    /// </summary>
    /// <param name="inventoryId">指定仓库/背包id</param>
    /// <param name="index">指定索引</param>
    /// <param name="slotId">返回格子id</param>
    /// <param name="slotData">返回格子数据</param>
    /// <returns>查询结果是否存在</returns>
    public static bool TryGetSlotData(int inventoryId, int index, out int slotId, out SlotData slotData)
    {
        return Inventory.TryGetSlotData(inventoryId, index, out slotId, out slotData);
    }

    /// <summary>
    /// 为实体创建新仓库/背包。
    /// </summary>
    /// <param name="entityId">实体id</param>
    /// <param name="name">仓库/背包名称</param>
    /// <return>新仓库/背包的实体id</return>
    public static int CreateInventory(int entityId, string name)
    {
        return Inventory.CreateInventory(entityId, name);
    }

    /// <summary>
    /// 为仓库/背包添加格子。
    /// </summary>
    /// <param name="inventoryId">仓库/背包id</param>
    /// <param name="count">格子数量</param>
    /// <param name="capacity">格子的最大容量</param>
    /// <return>格子的id数组</return>
    public static int[] CreateSlotsToInventory(int inventoryId, int count = 1, int capacity = 0)
    {
        return Inventory.CreateSlotsToInventory(inventoryId, count, capacity);
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
