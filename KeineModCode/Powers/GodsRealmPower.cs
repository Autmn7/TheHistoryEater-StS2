using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Powers;

public class GodsRealmPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || creator.Creature != Owner)
            return;
        Flash();
        if (card.IsUpgraded)
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
        CardCmd.Upgrade(card);
    }
}