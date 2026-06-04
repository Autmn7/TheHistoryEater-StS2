using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Cards.Rare;

public class WipedFromHistory : KeineModCard
{
    public WipedFromHistory() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithCalculatedDamage(10, (card, target) => card is KeineModCard keineModCard && keineModCard.InHakutaku() && target is not null && target.HasPower<HistoricalGapPower>() ? target.GetPowerAmount<HistoricalGapPower>() * 2 : 0, ValueProp.Move, 2);
        WithTip(StaticHoverTip.Block);
        WithTip(typeof(HistoricalGapPower));
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithKeywords(KeineKeywords.Human, KeineKeywords.Hakutaku);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        if (InHuman())
            if (cardPlay.Target.HasPower<HistoricalGapPower>())
                await CreatureCmd.GainBlock(Owner.Creature, cardPlay.Target.GetPowerAmount<HistoricalGapPower>(), ValueProp.Move, cardPlay);
    }
}