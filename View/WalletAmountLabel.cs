using Godot;
using GodotMonoGeneral.Logic;
using GodotMonoGeneral.Utils;

public partial class WalletAmountLabel : Label
{
    int WalletId
    {
        get
        {
            return GetMeta("entityId").AsInt32();
        }
        set
        {
            SetMeta("entityId", value);
        }
    }

    readonly ECSWorld World = SingletonFactory.GetSingleton<ECSWorld>();

    public override void _Ready()
    {
        base._Ready();
        if (!HasMeta("owner"))
        {
            return;
        }
        var ownerPath = GetMeta("owner").AsNodePath();
        var owner = GetNode(ownerPath);
        WalletId = World.CreateEntity(); // 创建钱包。
        var ownerId = owner.GetMeta("entityId").AsInt32();
        var initialAmount = Text.ToInt(); // 初始文字设置金额。
        var wallet = new Wallet()
        {
            ownerId = ownerId,
            amount = initialAmount
        };
        World.AddComponent(WalletId, ref wallet);
    }

    public void Refresh()
    {
        if (WalletId == -1)
        {
            return;
        }
        var wallet = World.GetComponent<Wallet>(WalletId);
        Text = wallet.amount.ToString();
    }

}
