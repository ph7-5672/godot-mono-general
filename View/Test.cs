using Godot;
using GodotMonoGeneral.Logic;
using GodotMonoGeneral.Utils;

public partial class Test : CanvasLayer
{

    

    public override void _Ready()
    {
        base._Ready();
        var inventoryId = LogicFacade.CreateInventory(999, "Test");
       // LogicFacade.CreateSlotsToInventory(inventoryId, 4);
        LoadSave();
        var test = this.OpenScene<CanvasLayer>("res://View/InventoryTest.tscn");
        test.SetMeta("inventoryId", inventoryId);
    }


    public void SaveGame()
    {
        LogicFacade.SaveGame(0);
    }

    public void LoadSave()
    {
        LogicFacade.LoadSave(0);
    }

}
