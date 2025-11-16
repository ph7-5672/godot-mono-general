using GodotMonoGeneral.Utils;

namespace GodotMonoGeneral.Logic.ECS;

public class InventorySystem
{
    private static ECSWorld World => SingletonFactory.GetSingleton<ECSWorld>();

    public int CreateInventory(int ownerId, string name)
    {
        var inventoryId = World.CreateEntity(); // 新实体。
        var inventoryData = new InventoryData // 组件。
        {
            ownerId = ownerId,
            name = name,
            slotCount = 0
        };
        World.AddComponent(inventoryId, ref inventoryData); // 绑定组件。
        return inventoryId;
    }

    public int[] CreateSlotsToInventory(int inventoryId, int count, int capacity)
    {
        if (!World.HasComponent<InventoryData>(inventoryId))
        {
            return [];
        }
        var result = new int[count];
        var inventory = World.GetComponent<InventoryData>(inventoryId);
        for (int i = 0; i < count; ++i)
        {
            var slotId = World.CreateEntity();
            var slotData = new SlotData()
            {
                inventoryId = inventoryId,
                itemId = -1,
                count = 0,
                capacity = capacity
            };
            World.AddComponent(slotId, ref slotData);
            result[i] = slotId;
        }
        inventory.slotCount += count;
        return result;
    }


}

/// <summary>
/// 仓库/背包数据。
/// </summary>
public struct InventoryData
{
    /// <summary>
    /// 所属实体id。
    /// </summary>
    public int ownerId;
    /// <summary>
    /// 仓库名称。
    /// </summary>
    public string name;
    /// <summary>
    /// 格子数量。
    /// </summary>
    public int slotCount;
}

/// <summary>
/// 背包格子数据。
/// </summary>
public struct SlotData
{
    /// <summary>
    /// 所属仓库/背包ID。
    /// </summary>
    public int inventoryId;
    /// <summary>
    /// 物品ID。
    /// </summary>
    public int itemId;
    /// <summary>
    /// 物品数量。
    /// </summary>
    public int count;
    /// <summary>
    /// 最大容量，为0表示无限大。
    /// </summary>
    public int capacity;

}

