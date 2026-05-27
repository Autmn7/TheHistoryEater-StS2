using KeineMod.KeineModCode.Cards.Special;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class TreasureOfValorPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override Task BeforeAttack(AttackCommand command)
    {
        if (!(command.ModelSource is CardModel modelSource) || modelSource.Owner.Creature != Owner || modelSource is not HeavenlySword)
            return Task.CompletedTask;
        var internalData = GetInternalData<Data>();
        if (internalData.commandToModify != null)
            return Task.CompletedTask;
        internalData.commandToModify = command;
        return Task.CompletedTask;
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        var internalData = GetInternalData<Data>();
        if (command != internalData.commandToModify)
            return;
        internalData.commandToModify = null;
        await PowerCmd.Decrement(this);
    }

    public class Data
    {
        public AttackCommand? commandToModify;
    }
}