using KeineMod.KeineModCode.Cards.Uncommon;
using KeineMod.KeineModCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class FlashbackPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (Amount > 0 && card.Owner == Owner.Player && card is not YesterdayOnceMore)
        {
            var outputCard = card.CreateClone();
            await ConsumeCmd.SpecificCard(choiceContext, outputCard, Owner.Player, null, false, true);
            await PowerCmd.Decrement(this);
        }
    }
}