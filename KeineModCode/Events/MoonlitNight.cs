using BaseLib.Abstracts;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Runs;

namespace KeineMod.KeineModCode.Events;

public class MoonlitNight : CustomEventModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("BuyDrinkCost", 50)];

    public override bool IsAllowed(IRunState runState)
    {
        var canBuyDrink = false;
        foreach (var player in runState.Players)
        {
            if (player.Gold >= DynamicVars["BuyDrinkCost"].BaseValue)
                canBuyDrink = true;
        }

        return canBuyDrink;
    }

    public override LocString InitialDescription
    {
        get
        {
            if (Owner.Character is Character.KeineMod)
                return L10NLookup("KEINEMOD-MOONLIT_NIGHT_KEINE.pages.INITIAL.description");
            if (Owner.Character.Id.ToString() == "CHARACTER.MOKOUMOD-MOKOU_MOD")
                return L10NLookup("KEINEMOD-MOONLIT_NIGHT_MOKOU.pages.INITIAL.description");
            return L10NLookup("KEINEMOD-MOONLIT_NIGHT.pages.INITIAL.description");
        }
    }

    public override string CustomInitialPortraitPath => "moonlit_night.png".EventImagePath();

    private EventOption CreateManualOption(Func<Task> onChosen, string optionKey)
    {
        var title = new LocString(LocTable, optionKey + ".title");
        var description = new LocString(LocTable, optionKey + ".description");
        return new EventOption(this, onChosen, title, description, optionKey, Enumerable.Empty<IHoverTip>());
    }

    private EventOption CreateManualOption(Func<Task> onChosen, string optionKey, IEnumerable<IHoverTip> hoverTips)
    {
        var title = new LocString(LocTable, optionKey + ".title");
        var description = new LocString(LocTable, optionKey + ".description");
        return new EventOption(this, onChosen, title, description, optionKey, hoverTips);
    }

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        var gold = Owner.Gold;
        if (Owner.Character is Character.KeineMod)
            return new List<EventOption>
            {
                CreateManualOption(ShareStories, "KEINEMOD-MOONLIT_NIGHT_KEINE.pages.INITIAL.options.SHARE_STORIES"),
                gold < DynamicVars["BuyDrinkCost"].IntValue ? CreateLockedOption() : CreateManualOption(BuyDrink, "KEINEMOD-MOONLIT_NIGHT_KEINE.pages.INITIAL.options.BUY_DRINK", HoverTipFactory.FromCardWithCardHoverTips<DualBlessing>())
            };
        if (Owner.Character.Id.ToString() == "CHARACTER.MOKOUMOD-MOKOU_MOD")
            return new List<EventOption>
            {
                CreateManualOption(ShareStories, "KEINEMOD-MOONLIT_NIGHT_MOKOU.pages.INITIAL.options.SHARE_STORIES"),
                gold < DynamicVars["BuyDrinkCost"].IntValue ? CreateLockedOption() : CreateManualOption(BuyDrink, "KEINEMOD-MOONLIT_NIGHT_MOKOU.pages.INITIAL.options.BUY_DRINK", HoverTipFactory.FromCardWithCardHoverTips<DualBlessing>())
            };
        return new List<EventOption>
        {
            CreateManualOption(ShareStories, "KEINEMOD-MOONLIT_NIGHT.pages.INITIAL.options.SHARE_STORIES"),
            gold < DynamicVars["BuyDrinkCost"].IntValue ? CreateLockedOption() : CreateManualOption(BuyDrink, "KEINEMOD-MOONLIT_NIGHT.pages.INITIAL.options.BUY_DRINK", HoverTipFactory.FromCardWithCardHoverTips<DualBlessing>())
        };
    }

    private async Task ShareStories()
    {
        var prefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 1);
        await CardPileCmd.RemoveFromDeck((await CardSelectCmd.FromDeckForRemoval(Owner, prefs)).ToList());
        if (Owner.Character is Character.KeineMod)
            SetEventFinished(L10NLookup("KEINEMOD-MOONLIT_NIGHT_KEINE.pages.SHARE_STORIES.description"));
        else if (Owner.Character.Id.ToString() == "CHARACTER.MOKOUMOD-MOKOU_MOD")
            SetEventFinished(L10NLookup("KEINEMOD-MOONLIT_NIGHT_MOKOU.pages.SHARE_STORIES.description"));
        else if (Owner.Character is Ironclad)
            SetEventFinished(L10NLookup("KEINEMOD-MOONLIT_NIGHT.pages.SHARE_STORIES_IRONCLAD.description"));
        else if (Owner.Character is Silent)
            SetEventFinished(L10NLookup("KEINEMOD-MOONLIT_NIGHT.pages.SHARE_STORIES_SILENT.description"));
        else if (Owner.Character is Regent)
            SetEventFinished(L10NLookup("KEINEMOD-MOONLIT_NIGHT.pages.SHARE_STORIES_REGENT.description"));
        else if (Owner.Character is Necrobinder)
            SetEventFinished(L10NLookup("KEINEMOD-MOONLIT_NIGHT.pages.SHARE_STORIES_NECROBINDER.description"));
        else if (Owner.Character is Defect)
            SetEventFinished(L10NLookup("KEINEMOD-MOONLIT_NIGHT.pages.SHARE_STORIES_DEFECT.description"));
        else
            SetEventFinished(L10NLookup("KEINEMOD-MOONLIT_NIGHT.pages.SHARE_STORIES.description"));
    }

    private async Task BuyDrink()
    {
        await PlayerCmd.LoseGold(DynamicVars["BuyDrinkCost"].IntValue, Owner, GoldLossType.Spent);
        var card = Owner.RunState.CreateCard<DualBlessing>(Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck));
        if (Owner.Character is Character.KeineMod)
            SetEventFinished(L10NLookup("KEINEMOD-MOONLIT_NIGHT_KEINE.pages.BUY_DRINK.description"));
        else if (Owner.Character.Id.ToString() == "CHARACTER.MOKOUMOD-MOKOU_MOD")
            SetEventFinished(L10NLookup("KEINEMOD-MOONLIT_NIGHT_MOKOU.pages.BUY_DRINK.description"));
        else
            SetEventFinished(L10NLookup("KEINEMOD-MOONLIT_NIGHT.pages.BUY_DRINK.description"));
    }

    private EventOption CreateLockedOption()
    {
        return new EventOption(this, null, "KEINEMOD-MOONLIT_NIGHT.pages.INITIAL.options.LOCKED");
    }
}