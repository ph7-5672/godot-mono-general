
using GodotMonoGeneral.Logic.ECS;
using GodotMonoGeneral.Utils;

namespace GodotMonoGeneral.Logic;

/// <summary>
/// 逻辑层门面类。
/// </summary>
public class LogicFacade
{
    private LogicFacade() { }

    private static ECSWorld World => SingletonFactory.GetSingleton<ECSWorld>();
    private static InventorySystem Inventory => SingletonFactory.GetSingleton<InventorySystem>();

    /// <summary>
    /// 获取指定格子数据。
    /// </summary>
    /// <param name="slotId"></param>
    /// <returns></returns>
    public static ref SlotData GetSlotData(int slotId)
    {
        return ref Inventory.GetSlotData(slotId);
    }

    /// <summary>
    /// 为实体创建新仓库/背包。
    /// </summary>
    /// <param name="entityId">实体id</param>
    /// <param name="name">仓库/背包名称</param>
    /// <return>新仓库/背包的实体id</return>
    public static int CreateInventory(int entityId = -1, string name = null)
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


    #region 存读档

    private static string GetSavePath(int index)
    {
        return $"res://Saves/{index}.save";
    }

    /// <summary>
    /// 加载指定索引的存档。
    /// </summary>
    /// <param name="index"></param>
    public static void LoadSave(int index)
    {
        if (index < 0)
        {
            return;
        }
        var path = GetSavePath(index);
        var snapshot = IOHelper.ReadJson<ECSSnapshot>(path);
        World.LoadSnapshot(snapshot);
        EventBus.Raise("load_save", index);
    }

    /// <summary>
    /// 保存游戏到指定存档。
    /// </summary>
    /// <param name="index"></param>
    public static void SaveGame(int index)
    {
        if (index < 0)
        {
            return;
        }
        var path = GetSavePath(index);
        var snapshot = World.GetSnapshot();
        IOHelper.WriteJson(snapshot, path);
        EventBus.Raise("save_game", index);
    }
    #endregion

}
