
using GodotMonoGeneral.Logic.ECS;
using GodotMonoGeneral.Utils;

namespace GodotMonoGeneral.Logic;

/// <summary>
/// 逻辑层门面类。
/// </summary>
public class LogicFacade
{
    private LogicFacade() { }

    public static readonly ECSWorld World = SingletonFactory.GetSingleton<ECSWorld>();

    #region 仓库操作

    /// <summary>
    /// 为实体创建新仓库。
    /// </summary>
    /// <param name="entityId">实体id</param>
    /// <param name="name">仓库名称</param>
    /// <return>新仓库的实体id</return>
    public static int CreateInventory(int entityId = -1, string name = null)
    {
        var inventoryId = World.CreateEntity(); // 新实体。
        var inventoryData = new InventoryData // 组件。
        {
            ownerId = entityId,
            name = name,
            slotCount = 0
        };
        World.AddComponent(inventoryId, ref inventoryData); // 绑定组件。
        return inventoryId;
    }

    /// <summary>
    /// 为仓库添加格子。
    /// </summary>
    /// <param name="inventoryId">仓库id</param>
    /// <param name="count">格子数量</param>
    /// <param name="capacity">格子的最大容量</param>
    /// <return>格子的id数组</return>
    public static int CreateSlot(int inventoryId, int capacity = 0)
    {
        if (!World.HasComponent<InventoryData>(inventoryId)) // 验证仓库id。
        {
            return -1;
        }
        ref var inventory = ref World.GetComponent<InventoryData>(inventoryId);
        var slotId = World.CreateEntity(); // 新格子实体
        var slotData = new SlotData() // 新格子数据
        {
            inventoryId = inventoryId,
            itemId = -1, // 物品id默认-1，表示不存在。
            count = 0,
            capacity = capacity,
            index = inventory.slotCount + 1 // 索引自动增加。
        };
        World.AddComponent(slotId, ref slotData); // 添加实体。
        return slotId;
    }

    #endregion

    #region 存读档
    /// <summary>
    /// 获取存档路径。
    /// </summary>
    /// <param name="index">存档索引</param>
    /// <returns>路径字符串</returns>
    private static string GetSavePath(int index)
    {
        return $"res://Saves/{index}.save";
    }

    /// <summary>
    /// 加载指定索引的存档。
    /// </summary>
    /// <param name="index">存档索引</param>
    public static void LoadSave(int index)
    {
        if (index < 0)
        {
            return;
        }
        var path = GetSavePath(index);
        var snapshot = IOHelper.ReadJson<ECSSnapshot>(path);
        World.LoadSnapshot(snapshot);
        EventBus.Raise("load_save", index); // 发送事件。
    }

    /// <summary>
    /// 保存游戏到指定存档。
    /// </summary>
    /// <param name="index">存档索引</param>
    public static void SaveGame(int index)
    {
        if (index < 0)
        {
            return;
        }
        var path = GetSavePath(index);
        var snapshot = World.GetSnapshot();
        IOHelper.WriteJson(snapshot, path);
        EventBus.Raise("save_game", index); // 发送事件。
    }
    #endregion

}
