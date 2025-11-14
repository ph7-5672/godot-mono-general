using Godot;
using GodotMonoGeneral.Logic;

public partial class Test : CanvasLayer
{

    public void CreateInventory()
    {
        var inventoryId = LogicFacade.CreateInventory(0, "测试背包");
        GD.Print($"创建背包成功，id：{inventoryId}");
    }

    public void CreateSlot()
    {
        var slotIds = LogicFacade.CreateSlotsToInventory(0);
        if (slotIds.Length > 0)
        {
            GD.Print($"创建格子成功，id：{slotIds[0]}");
        }
        else
        {
            GD.Print($"创建格子失败");
        }
    }


}
