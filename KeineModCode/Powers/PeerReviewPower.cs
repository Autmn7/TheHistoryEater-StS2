using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class PeerReviewPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, Decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (amount == 0M || power.GetTypeForAmount(amount) != PowerType.Debuff || !power.Owner.IsEnemy || applier != Owner || power is ITemporaryPower || power is HistoricalGapPower)
            return;
        await PowerCmd.Apply<HistoricalGapPower>(choiceContext, power.Owner, Amount, Owner, null);
    }
}