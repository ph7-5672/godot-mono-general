using Godot;
using GodotMonoGeneral.Logic;
using System;

public partial class Test : CanvasLayer
{

    public void CreateInventory()
    {
        LogicFacade.CreateInventory(0, "测试背包", 100);

    }

}
