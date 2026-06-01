using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using KeineMod.KeineModCode.Character;
using KeineMod.KeineModCode.Core;
using KeineMod.KeineModCode.Extensions;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace KeineMod.KeineModCode.Cards;

[Pool(typeof(KeineModCardPool))]
public abstract class KeineModCard : ConstructedCardModel, IOnStanceChange
{
    protected KeineModCard(int cost, CardType type, CardRarity rarity, TargetType target)
        : base(cost, type, rarity, target)
    {
        WithTip(new TooltipSource(card => new HoverTip(new LocString("static_hover_tips", "KEINEMOD-ARTIST-TITLE"), new LocString("cards", Id.Entry + ".artist"))));
    }

    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    protected override void AddExtraArgsToDescription(LocString description)
    {
        base.AddExtraArgsToDescription(description);
        FormDescription.AddTokens(this, description);
    }

    public bool InHuman()
    {
        return Owner.Creature.HasPower<DualFormPower>() || !KeineModel.IsInStance<HakutakuForm>(Owner);
    }

    public bool InHakutaku()
    {
        return Owner.Creature.HasPower<DualFormPower>() || KeineModel.IsInStance<HakutakuForm>(Owner);
    }

    public Task OnStanceChange(PlayerChoiceContext choiceContext, Player player, KeineStanceModel oldStance, KeineStanceModel newStance)
    {
        if (player == Owner && VisualCardPool is KeineModCardPool && Keywords.Contains(KeineModKeywords.Hakutaku))
            NCard.FindOnTable(this)?.Reload();
        return Task.CompletedTask;
    }

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power.Owner == Owner.Creature && power is DualFormPower && VisualCardPool is KeineModCardPool && Keywords.Contains(KeineModKeywords.Hakutaku))
            NCard.FindOnTable(this)?.Reload();
        return Task.CompletedTask;
    }

    public override Task AfterCardChangedPiles(CardModel card, PileType oldPile, AbstractModel? clonedBy)
    {
        if (card == this && VisualCardPool is KeineModCardPool && Keywords.Contains(KeineModKeywords.Hakutaku))
            NCard.FindOnTable(card)?.Reload();
        return Task.CompletedTask;
    }
}