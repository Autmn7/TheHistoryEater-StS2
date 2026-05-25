using KeineMod.KeineModCode.UIs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class TimeShiftPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this && Owner.Player != null && Amount >= 12)
        {
            Flash();
            var fullMoonToGain = (int)Math.Floor(Amount / 12.0);
            FullMoonChargeStateRegistry.Get(Owner.Player).GainFullMoon(fullMoonToGain);
            await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner, -fullMoonToGain * 12, Owner, null);
        }
    }

    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner)
            return;
        await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner, 1, Owner, null, true);
    }
}