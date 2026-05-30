using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace KeineMod.KeineModCode.Powers;

public class EphemeralityPower : KeineModPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power != this)
            return;
        switch (Amount)
        {
            case 1:
                await PowerCmd.Apply<WeakPower>(choiceContext, Owner, 1, null, null);
                break;
            case 3:
                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, -3, null, null);
                break;
            case 7:
                await PowerCmd.Apply<DisintegrationPower>(choiceContext, Owner, 137, null, null);
                break;
        }
    }
}