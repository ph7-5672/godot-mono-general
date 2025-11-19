using Godot;
using GodotMonoGeneral.Logic;
using GodotMonoGeneral.View.Api;

namespace GodotMonoGeneral.View;

/// <summary>
/// 背包格子。
/// </summary>
public partial class InventorySlot : Control, IViewController, IDivisionNode
{

    int slotId;
    InventorySlot last;

    public override void _Ready()
    {
        base._Ready();
        Refresh();
    }


    public void Refresh()
    {
        if (Owner == null && last != null)
        {
            Owner = last.Owner;
        }
        if (!Owner.HasMeta("inventoryId")) // 以Owner（预制场景的根节点）的元数据作为数据共享。
        {
            return;
        }
        var inventoryId = Owner.GetMeta("inventoryId").AsInt32(); // 获取背包id用以查询数据。
        var index = GetIndex();
        if (!LogicFacade.TryGetSlotData(inventoryId, index, out var id, out var data))
        {
            return;
        }
        slotId = id;
        GD.Print($"index:{data.index},itemId:{data.itemId}");
        // 分裂。
        var next = this.Divide();
        next.last = this;
    }

}
