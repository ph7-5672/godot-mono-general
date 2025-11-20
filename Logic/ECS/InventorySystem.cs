using GodotMonoGeneral.Utils;

namespace GodotMonoGeneral.Logic.ECS;

public class InventorySystem
{
    private ECSWorld World => SingletonFactory.GetSingleton<ECSWorld>();

    public int GetSlotId(int inventoryId, int index)
    {
        if (!World.HasComponent<InventoryData>(inventoryId)) // 验证仓库/背包id。
        {
            return -1;
        }
        var entities = World.GetEntities<SlotData>(); // 获取所有格子实体。
        foreach (var slotId in entities)
        {
            var s = World.GetComponent<SlotData>(slotId);
            if (s.inventoryId == inventoryId && s.index == index)
            {
                return slotId;
            }
        }
        return -1;
    }

    public SlotData GetSlotData(int slotId)
    {
        return World.GetComponent<SlotData>(slotId);
    }

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
        if (!World.HasComponent<InventoryData>(inventoryId)) // 验证仓库/背包id。
        {
            return [];
        }
        var result = new int[count];
        ref var inventory = ref World.GetComponent<InventoryData>(inventoryId); // 获取仓库/背包数据。
        for (int i = 0; i < count; ++i) // 遍历数据
        {
            var slotId = World.CreateEntity(); // 新格子实体
            var slotData = new SlotData() // 新格子数据
            {
                inventoryId = inventoryId,
                itemId = -1, // 物品id默认-1，表示不存在。
                count = 0,
                capacity = capacity,
                index = inventory.slotCount + i // 索引自动增加。
            };
            World.AddComponent(slotId, ref slotData); // 添加实体
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
    /// 索引。
    /// </summary>
    public int index;
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

