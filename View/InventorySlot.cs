using Godot;
using GodotMonoGeneral.Logic;
using GodotMonoGeneral.View.Api;

namespace GodotMonoGeneral.View;

/// <summary>
/// 背包格子。
/// </summary>
public partial class InventorySlot : Control, IViewController
{
    int slotId = -1;

    public override void _Ready()
    {
        base._Ready();
        CallDeferred("Refresh");
    }

    public void Refresh()
    {
        Visible = false;
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
        Visible = true;
        var data = LogicFacade.GetSlotData(slotId);
        var next = this.Divide(); // 分裂。
        next.Owner = Owner; // 设置数据共享。
    }

}
