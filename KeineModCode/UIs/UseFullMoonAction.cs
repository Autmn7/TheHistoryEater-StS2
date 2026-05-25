using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.UIs;

public class UseFullMoonAction : GameAction
{
    private readonly Player _player;

    public UseFullMoonAction(Player player)
    {
        _player = player;
    }

    // 返回当前执行动作的玩家 ID
    public override ulong OwnerId => _player.NetId; 

    // 动作类型归类
    public override GameActionType ActionType => GameActionType.CombatPlayPhaseOnly; 

    // 精确实现：直接返回我们上面写好的精简网络包
    public override INetAction ToNetAction()
    {
        return new UseFullMoonNetAction();
    }

    // 核心业务逻辑执行体
    protected override async Task ExecuteAction()
    {
        if (_player == null) return;

        var fullMoonUi = FullMoonChargeStateRegistry.Get(_player);
        if (fullMoonUi.CanUse())
        {
            // 此时所有联机玩家的电脑上都会同步扣除层数
            fullMoonUi.ClickedThisTurn = true;
            fullMoonUi.LoseFullMoon(1);

            // 获取当前合法的战斗 ChoiceContext 并异步应用能力
            //var currentContext = CombatManager.Instance.;
            await PowerCmd.Apply<FullMoonPower>(new ThrowingPlayerChoiceContext(), _player.Creature, 1, _player.Creature, null);
        }
    }
}