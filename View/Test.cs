using Godot;
using GodotMonoGeneral.Logic;

public partial class Test : CanvasLayer
{

    public override void _Ready()
    {
        base._Ready();
        var inventoryId = LogicFacade.CreateInventory(999, "Test");
        LogicFacade.CreateSlotsToInventory(inventoryId, 4);

        var snapshot = LogicFacade.GetGameSnapshot();

        var packedScene = GD.Load<PackedScene>("res://View/InventoryTest.tscn");
        var test = packedScene.Instantiate();
        test.SetMeta("inventoryId", inventoryId);
        AddChild(test);
       
    }



}
