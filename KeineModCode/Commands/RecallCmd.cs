using KeineMod.KeineModCode.Cards.Common;
using KeineMod.KeineModCode.Cards.Uncommon;
using KeineMod.KeineModCode.UIs;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Commands;

public class RecallCmd
{
    public static async Task<IEnumerable<CardModel>> FromScroll(PlayerChoiceContext choiceContext, Player player, int amount, bool shouldUpgrade = false, bool shouldClone = false)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return [];
        var prefs = new CardSelectorPrefs(new LocString("card_selection", shouldClone ? "TO_RECALL_CLONE_KEINE" : "TO_RECALL_KEINE"), amount);
        var list = (await CardSelectCmd.FromSimpleGrid(choiceContext, ScrollPile.Scroll.GetPile(player).Cards.ToList(), player, prefs)).ToList();
        foreach (var recalledCard in list)
        {
            var outputCard = recalledCard;
            if (shouldClone)
                outputCard = recalledCard.CreateClone();
            if (shouldUpgrade)
                CardCmd.Upgrade(outputCard);
            if (outputCard is not RevisionSession)
                CardCmd.ApplyKeyword(outputCard, CardKeyword.Ethereal);
            if (recalledCard is not FlowerOfEdo)
                CardCmd.ApplyKeyword(outputCard, CardKeyword.Exhaust);
            outputCard.EnergyCost.AddThisTurn(-1);
            if (!shouldClone)
                await CardPileCmd.Add(outputCard, PileType.Hand);
            else
                await CardPileCmd.AddGeneratedCardToCombat(outputCard, PileType.Hand, player);
        }

        return list;
    }

    public static async Task<IEnumerable<CardModel>> FromScrollUpTo(PlayerChoiceContext choiceContext, Player player, int amount, bool shouldUpgrade = false)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return [];
        var prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_RECALL_UPTO_KEINE"), 0, amount);
        var list = (await CardSelectCmd.FromSimpleGrid(choiceContext, ScrollPile.Scroll.GetPile(player).Cards.ToList(), player, prefs)).ToList();
        foreach (var recalledCard in list)
        {
            if (shouldUpgrade)
                CardCmd.Upgrade(recalledCard);
            if (recalledCard is not RevisionSession)
                CardCmd.ApplyKeyword(recalledCard, CardKeyword.Ethereal);
            if (recalledCard is not FlowerOfEdo)
                CardCmd.ApplyKeyword(recalledCard, CardKeyword.Exhaust);
            recalledCard.EnergyCost.AddThisTurn(-1);
            await CardPileCmd.Add(recalledCard, PileType.Hand);
        }

        return list;
    }
}