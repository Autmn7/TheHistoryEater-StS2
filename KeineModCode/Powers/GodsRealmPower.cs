using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Powers;

public class GodsRealmPower : KeineModPower, IOnConsumed, IOnCreated
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard.Owner != Owner.Player || player != Owner.Player)
            return;
        if (consumedCard.IsUpgraded)
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
        CardCmd.Upgrade(consumedCard);
    }

    public async Task OnCreated(PlayerChoiceContext choiceContext, Player player, CardModel createdCard)
    {
        if (createdCard.Owner != Owner.Player || player != Owner.Player)
            return;
        if (createdCard.IsUpgraded)
            await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
        CardCmd.Upgrade(createdCard);
    }
}