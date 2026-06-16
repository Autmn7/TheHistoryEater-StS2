using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class LearnFromPain : KeineModCard
{
    public LearnFromPain() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(4, 3);
        WithCalculatedVar("Repeat", 1, (card, _) => card is KeineModCard keineModCard && keineModCard.InHakutaku() ? 1 : 0);
        WithPower<KnowledgePower>(1);
        WithKeywords(KeineKeywords.Knowledgeable, KeineKeywords.Human, KeineKeywords.Consume, KeineKeywords.Hakutaku);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitCount((int)((CalculatedVar)DynamicVars["Repeat"]).Calculate(cardPlay.Target)).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        if (InHuman())
        {
            var consumedCard = await ConsumeCmd.FromHandSingle(choiceContext, Owner, this);
            if (consumedCard is { Type: CardType.Status or CardType.Curse })
                await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
        }
    }
}