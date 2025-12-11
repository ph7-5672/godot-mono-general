using Godot;
using GodotMonoGeneral.Logic;
using GodotMonoGeneral.Logic.ECS;

namespace GodotMonoGeneral.View;

/// <summary>
/// 背包格子。
/// </summary>
public partial class InventorySlot : Control
{
    [Export]
    Label countLabel;
    [Export]
    Label idLabel;
    [Export]
    int capacity;

    int inventoryId = -1;
    int slotId = -1;

    
    public override void _Ready()
    {
        base._Ready();
        // 注册格子数据。
        var parent = GetParent();
        if (!parent.HasMeta("inventoryId")) // 以父节点的元数据作为数据共享。
        {
            var inventory = LogicFacade.CreateInventory();
            parent.SetMeta("inventoryId", inventory);
        }
        inventoryId = parent.GetMeta("inventoryId").AsInt32();
        slotId = LogicFacade.CreateSlotsToInventory(inventoryId, 1, capacity)[0]; // 创建单个格子实体。
        ref SlotData data = ref LogicFacade.GetSlotData(slotId); // 编辑格子数据。
        data.index = GetIndex();  // 同步索引。
        CallDeferred("Refresh");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (LogicFacade.TryGetEvent(out LoadSaveEvent _))
        {
            Refresh();
        }
    }

    public void Refresh()
    {
        if (inventoryId == -1)
        {
            return;
        }
        // if (slotId == -1) // 尝试获取格子id。
        // {
        //     var inventoryId = Owner.GetMeta("inventoryId").AsInt32(); // 获取背包id用以查询数据。
        //     var index = GetIndex();
        //     slotId = LogicFacade.GetSlotId(inventoryId, index);
        // }
        if (slotId == -1)
        {
            return;
        }
        var data = LogicFacade.GetSlotData(slotId);
        if (data.count > 0 && data.itemId != -1)
        {
            countLabel.Text = data.count.ToString();
            idLabel.Text = data.itemId.ToString();
        }
    }

}
