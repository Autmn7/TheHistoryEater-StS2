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
                await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), Owner, 1, Owner, null);
                break;
            case 3:
                await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner, -3, Owner, null);
                break;
            case 7:
                await PowerCmd.Apply<DisintegrationPower>(new ThrowingPlayerChoiceContext(), Owner, 137, Owner, null);
                break;
        }
    }
}