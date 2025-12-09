using Godot;
using GodotMonoGeneral.Logic;

public partial class Test : CanvasLayer
{
    [Export]
    Control inventoryTest;

    public override void _Ready()
    {
        base._Ready();
        ProcessPriority = int.MaxValue; // 最后处理，用以事件的每帧清除。
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        LogicFacade.ReleaseEvents();
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
