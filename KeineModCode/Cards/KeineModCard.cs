using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using KeineMod.KeineModCode.Character;
using KeineMod.KeineModCode.Core;
using KeineMod.KeineModCode.Extensions;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards;

[Pool(typeof(KeineModCardPool))]
public abstract class KeineModCard : ConstructedCardModel
{
    protected KeineModCard(int cost, CardType type, CardRarity rarity, TargetType target)
        : base(cost, type, rarity, target)
    {
        WithTip(new TooltipSource((Func<CardModel, IHoverTip>)((CardModel card) =>
            (IHoverTip)(object)new HoverTip(new LocString("static_hover_tips", "KEINEMOD-ARTIST-TITLE"), new LocString("cards", ((AbstractModel)this).Id.Entry + ".artist"), null))));
    }

    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    protected bool InHuman()
    {
        return Owner.Creature.HasPower<DualFormPower>() || !KeineModel.IsInStance<HakutakuForm>(Owner);
    }

    protected bool InHakutaku()
    {
        return Owner.Creature.HasPower<DualFormPower>() || KeineModel.IsInStance<HakutakuForm>(Owner);
    }
}