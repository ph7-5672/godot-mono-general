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

    int slotId = -1;

    public override void _Ready()
    {
        base._Ready();
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
        if (!Owner.HasMeta("inventoryId")) // 以Owner（预制场景的根节点）的元数据作为数据共享。
        {
            return;
        }
        if (slotId == -1) // 尝试获取格子id。
        {
            var inventoryId = Owner.GetMeta("inventoryId").AsInt32(); // 获取背包id用以查询数据。
            var index = GetIndex();
            slotId = LogicFacade.GetSlotId(inventoryId, index);
        }
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
