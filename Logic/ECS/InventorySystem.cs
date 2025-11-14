using GodotMonoGeneral.Utils;

namespace GodotMonoGeneral.Logic.ECS;

public class InventorySystem
{
    readonly EntityIdGenerator entityIdGenerator = SingletonFactory.GetSingleton<EntityIdGenerator>();
    readonly SparseSet<InventoryData> inventories = new();
    readonly SparseSet<SlotData> slots = new();


    public int CreateInventory(int ownerId, string name)
    {
        var inventoryId = entityIdGenerator.Next(); // 新实体id。
        var InventoryData = new InventoryData // 组件。
        {
            ownerId = ownerId,
            name = name,
            slotCount = 0
        };
        inventories.AddOrUpdate(inventoryId, InventoryData); // 绑定组件。
        return inventoryId;
    }


    public int[] CreateSlotsToInventory(int inventoryId, int count, int capacity)
    {
        if (!inventories.Has(inventoryId))
        {
            return [];
        }
        var result = new int[count];
        var inventory = inventories.Get(inventoryId);
        for (int i = 0; i < count; ++i)
        {
            var slotId = entityIdGenerator.Next();
            var slotData = new SlotData()
            {
                inventoryId = inventoryId,
                itemId = -1,
                count = 0,
                capacity = capacity
            };
            slots.AddOrUpdate(slotId, slotData);
            result[i] = slotId;
        }
        inventory.slotCount += count;
        return result;
    }


    public void AddItemToInventory(int ownerId, string inventoryName, int itemId, int count = 1)
    {
        if (!TryGetInventory(ownerId, inventoryName, out var inventory))
        {
            return;
        }
        
    }


    public bool TryGetInventory(int ownerId, string inventoryName, out InventoryData inventory)
    {
        for (int i = 0; i < inventories.Count; i++)
        {
            inventory = inventories[i];
            if (inventory.ownerId == ownerId && inventoryName == inventory.name)
            {
                return true;
            }
        }
        inventory = default;
        return false;
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

