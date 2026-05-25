using KeineMod.KeineModCode.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

[Obsolete("This power is replaced by the Full Moon UI system to control direct stance change.")]
public class FullMoonPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this && Owner.Player != null && Owner.HasPower<FullMoonPower>())
            await StanceCmd.EnterHakutaku(choiceContext, Owner.Player, null);
    }

    public override async Task AfterRemoved(Creature oldOwner)
    {
        if (oldOwner == Owner && oldOwner.Player != null)
            await StanceCmd.ExitStance(new ThrowingPlayerChoiceContext(), oldOwner.Player, null);
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;
        await PowerCmd.Decrement(this);
    }
}