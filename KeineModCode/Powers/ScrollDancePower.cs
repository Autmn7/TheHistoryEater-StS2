using KeineMod.KeineModCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Powers;

public class ScrollDancePower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        var consumedCards = (await ConsumeCmd.FromHandUpTo(choiceContext, Owner.Player, Amount, this)).ToList();
        if (consumedCards.Count != 0)
            await CardPileCmd.Draw(choiceContext, consumedCards.Count, Owner.Player);
    }
}