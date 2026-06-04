using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class KnowledgeGap : KeineModCard
{
    public KnowledgeGap() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(12, 3);
        WithVar("Ratio", 3);
        WithKeywords(KeineKeywords.Knowledgeable);
        WithTip(typeof(HistoricalGapPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        var gap = (int)Math.Floor(attackCommand.Results.SelectMany(r => r).Sum(r => r.TotalDamage + r.OverkillDamage) / DynamicVars["Ratio"].BaseValue);
        await PowerCmd.Apply<HistoricalGapPower>(choiceContext, cardPlay.Target, gap, Owner.Creature, this);
    }
}