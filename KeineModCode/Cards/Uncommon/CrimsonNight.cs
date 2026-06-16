using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class CrimsonNight : KeineModCard
{
    public CrimsonNight() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(5, 1);
        WithVar("Repeat", 2);
        WithCalculatedVar("CalculatedVulnerable", 0, (card, _) => card.Owner.Creature.GetPowerAmount<KnowledgePower>(), 1);
        WithKeywords(KeineKeywords.Knowledgeable);
        WithTip(typeof(KnowledgePower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitCount(DynamicVars["Repeat"].IntValue).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        var vulAmt = ((CalculatedVar)DynamicVars["CalculatedVulnerable"]).Calculate(cardPlay.Target);
        if (vulAmt > 0)
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, vulAmt, Owner.Creature, this);
    }
}