using KeineMod.KeineModCode.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace KeineMod.KeineModCode.Stances;

public sealed class HakutakuForm : KeineStanceModel
{
    public override bool ShouldReceiveCombatHooks => true;

    protected override StanceVfxConfig VfxConfig => new(null, null, null, null, (ShakeStrength)0);

    public override async Task OnEnterStance(PlayerChoiceContext choiceContext, Player player, CardModel? source)
    {
        // var recallAmount = 2;
        // if (player.Creature.HasPower<WisdomPower>())
        //     recallAmount += player.Creature.GetPowerAmount<WisdomPower>();
        // await RecallCmd.FromScrollUpTo(choiceContext, player, recallAmount);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature))
            return;
        await StanceCmd.ExitStance(choiceContext, Owner, null);
    }
}