using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace KeineMod.KeineModCode.Cards.Common;

public class ScrollStorm : KeineModCard
{
    public ScrollStorm() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(5, 2);
        WithCalculatedVar("Repeat", 3, (card, _) => card is KeineModCard keineModCard && keineModCard.InHakutaku() ? 1 : 0);
        WithPower<KnowledgePower>(1);
        WithKeywords(KeineModKeywords.Knowledgeable, KeineModKeywords.Human, KeineModKeywords.Hakutaku);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitCount((int)((CalculatedVar)DynamicVars["Repeat"]).Calculate(cardPlay.Target)).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        if (InHuman())
            await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
    }
}