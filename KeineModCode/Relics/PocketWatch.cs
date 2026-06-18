using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;

namespace KeineMod.KeineModCode.Relics;

public class PocketWatch : KeineModRelic
{
    public override RelicRarity Rarity => RelicRarity.Shop;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<TimeShiftPower>(),
        HoverTipFactory.FromKeyword(KeineKeywords.Fullmoon),
        HoverTipFactory.FromKeyword(KeineKeywords.Hakutaku)
    ];
}