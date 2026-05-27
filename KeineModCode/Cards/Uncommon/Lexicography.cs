using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class Lexicography : KeineModCard
{
    public Lexicography() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(4);
        WithPower<KnowledgePower>(1);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (var hittableEnemy in CombatState.HittableEnemies)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
        }
    }
}