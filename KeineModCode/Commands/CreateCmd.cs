using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Commands;

public static class CreateCmd
{
    public static async Task Execute(CardModel? createdCard, Player creator, bool shouldUpgrade = false, PileType toPile = PileType.Hand, bool freeThisTurn = false)
    {
        if (CombatManager.Instance.IsOverOrEnding || createdCard == null)
            return;
        if (shouldUpgrade)
            CardCmd.Upgrade(createdCard);
        if (freeThisTurn)
            createdCard.SetToFreeThisTurn();
        if (createdCard.Type != CardType.Status)
            CardCmd.ApplyKeyword(createdCard, CardKeyword.Retain);
        if (toPile == PileType.Hand)
            await CardPileCmd.AddGeneratedCardToCombat(createdCard, toPile, creator);
        else
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(createdCard, toPile, creator));
    }
}