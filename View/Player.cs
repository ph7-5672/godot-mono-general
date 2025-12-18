using Godot;
using GodotMonoGeneral.Logic;
using GodotMonoGeneral.Utils;

public partial class Player : Node
{
    readonly ECSWorld World = SingletonFactory.GetSingleton<ECSWorld>();
    public override void _Ready()
    {
        base._Ready();
        var entityId = World.CreateEntity();
        SetMeta("entityId", entityId); // 用元数据共享数据。
    }


}
