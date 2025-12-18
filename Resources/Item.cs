using Godot;

/// <summary>
/// 物品数据配置。
/// </summary>
[GlobalClass]
public partial class Item : Resource
{
    /// <summary>
    /// 物品名称。
    /// </summary>
    [Export] public string itemName;
    /// <summary>
    /// 物品图标。
    /// </summary>
    [Export] public Texture2D icon;
}
