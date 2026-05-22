using KeineMod.KeineModCode.Piles;
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
    public static async Task<IEnumerable<CardModel>> FromScroll(PlayerChoiceContext choiceContext, Player player, int amount, bool shouldUpgrade = false)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return [];
        var prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_RECALL_KEINE"), amount);
        var list = (await CardSelectCmd.FromSimpleGrid(choiceContext, ScrollPile.Scroll.GetPile(player).Cards.ToList(), player, prefs)).ToList();
        foreach (var recalledCard in list)
        {
            if (shouldUpgrade)
                CardCmd.Upgrade(recalledCard);
            CardCmd.ApplyKeyword(recalledCard, CardKeyword.Ethereal);
            CardCmd.ApplyKeyword(recalledCard, CardKeyword.Exhaust);
            recalledCard.EnergyCost.AddThisTurn(-1);
            await CardPileCmd.Add(recalledCard, PileType.Hand);
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
            CardCmd.ApplyKeyword(recalledCard, CardKeyword.Ethereal);
            CardCmd.ApplyKeyword(recalledCard, CardKeyword.Exhaust);
            recalledCard.EnergyCost.AddThisTurn(-1);
            await CardPileCmd.Add(recalledCard, PileType.Hand);
        }

        return list;
    }
}