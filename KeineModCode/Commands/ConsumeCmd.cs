using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Scripts;
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

public static class ConsumeCmd
{
    public static async Task<IEnumerable<CardModel>> FromHand(PlayerChoiceContext choiceContext, Player player, int amount, AbstractModel source, bool shouldUpgrade = false)
    {
        if (CombatManager.Instance.IsOverOrEnding || amount <= 0)
            return [];
        var prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_CONSUME_KEINE"), amount);
        var list = (await CardSelectCmd.FromHand(choiceContext, player, prefs, null, source)).ToList();
        foreach (var consumedCard in list)
        {
            if (shouldUpgrade)
                CardCmd.Upgrade(consumedCard);
            if (consumedCard is not TheSmartest || consumedCard.EnergyCost.GetResolved() <= 0) await CardPileCmd.Add(consumedCard, ScrollPile.Scroll.GetPile(player));
            KeineConstantsStateRegistry.Get(player).IncrementCardsConsumed(1);
            await KeineHooks.OnConsumed(choiceContext, player, consumedCard);
            await KeineHooks.OnConsumedLate(choiceContext, player, consumedCard);
        }

        return list;
    }

    public static async Task<IEnumerable<CardModel>> FromHandUpTo(PlayerChoiceContext choiceContext, Player player, int amount, AbstractModel source, bool shouldUpgrade = false)
    {
        if (CombatManager.Instance.IsOverOrEnding || amount <= 0)
            return [];
        var prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_CONSUME_UPTO_KEINE"), 0, amount);
        var list = (await CardSelectCmd.FromHand(choiceContext, player, prefs, null, source)).ToList();
        foreach (var consumedCard in list)
        {
            if (shouldUpgrade)
                CardCmd.Upgrade(consumedCard);
            if (consumedCard is not TheSmartest || consumedCard.EnergyCost.GetResolved() <= 0) await CardPileCmd.Add(consumedCard, ScrollPile.Scroll.GetPile(player));
            KeineConstantsStateRegistry.Get(player).IncrementCardsConsumed(1);
            await KeineHooks.OnConsumed(choiceContext, player, consumedCard);
            await KeineHooks.OnConsumedLate(choiceContext, player, consumedCard);
        }

        return list;
    }

    public static async Task<CardModel?> FromHandSingle(PlayerChoiceContext choiceContext, Player player, AbstractModel source, bool shouldUpgrade = false)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return null;
        var prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_CONSUME_KEINE"), 1);
        var consumedCard = (await CardSelectCmd.FromHand(choiceContext, player, prefs, null, source)).FirstOrDefault();
        if (consumedCard != null)
        {
            if (shouldUpgrade)
                CardCmd.Upgrade(consumedCard);
            if (consumedCard is not TheSmartest || consumedCard.EnergyCost.GetResolved() <= 0) await CardPileCmd.Add(consumedCard, ScrollPile.Scroll.GetPile(player));
            KeineConstantsStateRegistry.Get(player).IncrementCardsConsumed(1);
            await KeineHooks.OnConsumed(choiceContext, player, consumedCard);
            await KeineHooks.OnConsumedLate(choiceContext, player, consumedCard);
        }

        return consumedCard;
    }

    public static async Task EntireHand(PlayerChoiceContext choiceContext, Player player, AbstractModel source)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        var hand = PileType.Hand.GetPile(player).Cards.ToList();
        foreach (var consumedCard in hand)
        {
            if (consumedCard is not TheSmartest || consumedCard.EnergyCost.GetResolved() <= 0) await CardPileCmd.Add(consumedCard, ScrollPile.Scroll.GetPile(player));
            KeineConstantsStateRegistry.Get(player).IncrementCardsConsumed(1);
            await KeineHooks.OnConsumed(choiceContext, player, consumedCard);
            await KeineHooks.OnConsumedLate(choiceContext, player, consumedCard);
        }
    }

    public static async Task<CardModel?> SpecificCard(PlayerChoiceContext choiceContext, CardModel? consumedCard, Player player, AbstractModel? source, bool shouldUpgrade = false, bool showPreview = false)
    {
        if (CombatManager.Instance.IsOverOrEnding || consumedCard == null)
            return null;
        if (shouldUpgrade)
            CardCmd.Upgrade(consumedCard);
        if (consumedCard is not TheSmartest || consumedCard.EnergyCost.GetResolved() <= 0)
            if (!showPreview)
                await CardPileCmd.Add(consumedCard, ScrollPile.Scroll.GetPile(player));
            else
                CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(consumedCard, ScrollPile.Scroll, player));
        KeineConstantsStateRegistry.Get(player).IncrementCardsConsumed(1);
        await KeineHooks.OnConsumed(choiceContext, player, consumedCard);
        await KeineHooks.OnConsumedLate(choiceContext, player, consumedCard);

        return consumedCard;
    }
}