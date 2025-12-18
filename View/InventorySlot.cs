using Godot;
using GodotMonoGeneral.Logic;
using GodotMonoGeneral.Utils;

namespace GodotMonoGeneral.View;

/// <summary>
/// 背包格子。
/// </summary>
public partial class InventorySlot : Control
{
    /// <summary>
    /// 数量文字节点。
    /// </summary>
    [Export] Label countLabel;
    /// <summary>
    /// 名称文字节点。
    /// </summary>
    [Export] Label nameLabel;
    /// <summary>
    /// 容量配置。为0表示无上限。
    /// </summary>
    [Export] int capacity;

    int InventoryId
    {
        get
        {
            return GetParent().GetMeta("entityId").AsInt32();
        }
        set
        {
            GetParent().SetMeta("entityId", value);
        }
    }

    int SlotId
    {
        get
        {
            return GetMeta("entityId").AsInt32();
        }
        set
        {
            SetMeta("entityId", value);
        }
    }

    readonly ECSWorld World = SingletonFactory.GetSingleton<ECSWorld>();

    public override void _Ready()
    {
        base._Ready();
        var parent = GetParent();
        if (!parent.HasMeta("owner"))
        {
            return;
        }
        if (!parent.HasMeta("entityId")) // 以父节点的元数据作为数据共享。
        {
            var ownerPath = parent.GetMeta("owner").AsNodePath();
            var owner = parent.GetNode(ownerPath);
            // 父节点没有meta数据时，懒加载新的仓库数据。
            InventoryId = World.CreateEntity();
            var inventoryData = new InventoryData()
            {
                ownerId = owner.GetMeta("entityId").AsInt32(),
                name = parent.GetMeta("inventoryName").AsString(),
            };
            World.AddComponent(InventoryId, ref inventoryData);
        }
        // 创建单个格子实体。
        SlotId = World.CreateEntity(); 
        var slotData = new SlotData()
        {
            inventoryId = InventoryId,
            itemId = -1,
            count = 0,
            capacity = capacity,
            index = GetIndex()
        }; // 格子数据。
        World.AddComponent(SlotId, ref slotData); // 添加组件。
        CallDeferred("Refresh");
    }

    public void Refresh()
    {
        if (InventoryId == -1 || SlotId == -1)
        {
            return;
        }
        var data = World.GetComponent<SlotData>(SlotId); // 查询数据。
        if (data.count > 1) // 物品数量超过1时才显示数量。
        {
            countLabel.Text = data.count.ToString();
        }
        // if (Game.ItemDict.TryGetValue(data.itemId, out var item))
        // {
        //     nameLabel.Text = item.itemName; // 显示物品名称。
        // }
    }

}
