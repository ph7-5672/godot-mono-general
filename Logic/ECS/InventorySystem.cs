using GodotMonoGeneral.Utils;

namespace GodotMonoGeneral.Logic.ECS;

public class InventorySystem
{
    readonly EntityIdGenerator entityIdGenerator = SingletonFactory.GetSingleton<EntityIdGenerator>();
    readonly SparseSet<InventoryData> inventories = new();
    readonly SparseSet<SlotData> slots = new();

    public void CreateInventory(int ownerId, string name, int capacity = 0)
    {
        var inventoryId = entityIdGenerator.Next(); // 新实体id。
        var InventoryData = new InventoryData // 组件。
        {
            ownerId = ownerId,
            name = name,
            slotCapacity = capacity,
            slotCount = 0
        };
        inventories.AddOrUpdate(inventoryId, InventoryData); // 绑定组件。
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

    public void AddItemToInventory(int ownerId, string inventoryName, int itemId, int count = 1)
    {
        if (!TryGetInventory(ownerId, inventoryName, out var inventory))
        {
            return;
        }
        
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
    /// <summary>
    /// 格子容量，为0表示无限大。
    /// </summary>
    public int slotCapacity;

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

