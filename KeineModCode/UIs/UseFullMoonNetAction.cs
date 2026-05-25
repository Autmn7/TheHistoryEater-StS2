using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

namespace KeineMod.KeineModCode.UIs;

public class UseFullMoonNetAction : INetAction
{
    // 必须保留显式的无参构造函数，供网络底层反序列化时实例化对象
    public UseFullMoonNetAction()
    {
    }

    // 因为此动作没有任何本地字段需要跨网络传输，
    // 底层会通过 [GenerateSubtypes] 自动识别并传输类类型，
    // 这里的实例数据序列化直接留空即可。
    public void Serialize(PacketWriter writer)
    {
        // No-op：无自定义字段需要写入
    }

    public void Deserialize(PacketReader reader)
    {
        // No-op：无自定义字段需要读取
    }

    // 联机核心：远端玩家收到该空包后，游戏底层会自动传入发送者（player）
    // 并通过此方法在远端生成对应的 Action 塞进他们的本地队列中
    public GameAction ToGameAction(Player player)
    {
        return new UseFullMoonAction(player);
    }
}