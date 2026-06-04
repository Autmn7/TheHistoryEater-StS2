using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Common;

public class NewSemester : KeineModCard
{
    public NewSemester() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(6, 3);
        WithVar("Threshold", 3);
        WithPower<KnowledgePower>(1);
        WithCards(1);
        WithKeywords(KeineKeywords.Knowledgeable);
    }

    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPowerAmount<KnowledgePower>() < DynamicVars["Threshold"].BaseValue;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        if (Owner.Creature.GetPowerAmount<KnowledgePower>() < DynamicVars["Threshold"].BaseValue)
            await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
        else
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
}