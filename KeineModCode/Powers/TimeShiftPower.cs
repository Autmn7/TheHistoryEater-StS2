using KeineMod.KeineModCode.Relics;
using KeineMod.KeineModCode.Scripts;
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
        if (power == this && Owner.Player != null)
        {
            var perFullMoon = 12;
            if (Owner.Player.GetRelic<PocketWatch>() != null)
                perFullMoon = 10;
            if (Amount >= perFullMoon)
            {
                Flash();
                var fullMoonToGain = (int)Math.Floor(Amount / (decimal)perFullMoon);
                KeineConstantsStateRegistry.Get(Owner.Player).GainFullMoon(fullMoonToGain);
                await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner, -fullMoonToGain * perFullMoon, Owner, null);
            }
        }
    }

    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner)
            return;
        var timeShift = Math.Max(1, cardPlay.Resources.EnergySpent);
        await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner, timeShift, Owner, null, true);
    }
}