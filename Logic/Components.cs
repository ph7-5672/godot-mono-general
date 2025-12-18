namespace GodotMonoGeneral.Logic;

/// <summary>
/// 仓库数据。
/// </summary>
public struct InventoryData
{
    /// <summary>
    /// 所属实体id。
    /// </summary>
    public int ownerId;
    /// <summary>
    /// 仓库名称。
    /// </summary>
    public string name;
}

/// <summary>
/// 仓库格子数据。
/// </summary>
public struct SlotData
{
    /// <summary>
    /// 所属仓库ID。
    /// </summary>
    public int inventoryId;
    /// <summary>
    /// 索引。
    /// </summary>
    public int index;
    /// <summary>
    /// 物品ID。
    /// </summary>
    public int itemId;
    /// <summary>
    /// 物品数量。
    /// </summary>
    public int count;
    /// <summary>
    /// 最大容量，为0表示无限大。
    /// </summary>
    public int capacity;

}

/// <summary>
/// 钱包组件。
/// </summary>
public struct Wallet
{   
    /// <summary>
    /// 所属的实体ID。
    /// </summary>
    public int ownerId;
    /// <summary>
    /// 余额。
    /// </summary>
    public int amount;
}