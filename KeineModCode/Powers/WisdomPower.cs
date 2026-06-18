using BaseLib.Hooks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class WisdomPower : KeineModPower, IMaxHandSizeModifier
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public int ModifyMaxHandSize(Player player, int currentMaxHandSize)
    {
        if (player.Creature != Owner)
            return currentMaxHandSize;
        currentMaxHandSize += Amount;
        return currentMaxHandSize;
    }

    public override Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target, Creature? applier, CardModel? cardSource)
    {
        if (power != this || target != Owner)
            return Task.CompletedTask;
        if (Amount + (int)amount > 5) GetInternalData<Data>().exceededAmount += Amount + (int)amount - 5;
        return Task.CompletedTask;
    }

    public override decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver, decimal amount, Creature? target, CardModel? cardSource)
    {
        if (power != this || target != Owner || GetInternalData<Data>().exceededAmount <= 0)
            return 0;
        return -GetInternalData<Data>().exceededAmount;
    }

    public override async Task AfterModifyingPowerAmountGiven(PowerModel power)
    {
        if (power != this || GetInternalData<Data>().exceededAmount <= 0)
            return;
        await PowerCmd.Apply<KnowledgePower>(new ThrowingPlayerChoiceContext(), Owner, GetInternalData<Data>().exceededAmount, Owner, null);
        GetInternalData<Data>().exceededAmount = 0;
    }

    private class Data
    {
        public int exceededAmount;
    }
}