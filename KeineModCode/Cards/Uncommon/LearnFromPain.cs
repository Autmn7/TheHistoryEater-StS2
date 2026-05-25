using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class LearnFromPain : KeineModCard
{
    public LearnFromPain() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVar("Knowledgeable", 1);
        WithDamage(5, 3);
        WithPower<KnowledgePower>(1);
        WithKeywords(KeineModKeywords.Knowledgeable, KeineModKeywords.Human, KeineModKeywords.Consume, KeineModKeywords.Hakutaku, KeineModKeywords.Recall);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitCount(InHakutaku() ? 2 : 1).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        if (InHuman())
        {
            var consumedCard = await ConsumeCmd.FromHandSingle(choiceContext, Owner, this);
            if (consumedCard is { Type: CardType.Status or CardType.Curse })
                await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
        }
    }
}