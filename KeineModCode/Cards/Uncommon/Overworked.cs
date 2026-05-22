using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class Overworked : KeineModCard
{
    public Overworked() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<KnowledgePower>(1);
        WithEnergy(1, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature.HasPower<KnowledgePower>() && Owner.Creature.GetPowerAmount<KnowledgePower>() >= DynamicVars["KnowledgePower"].BaseValue)
        {
            await PowerCmd.ModifyAmount(choiceContext, Owner.Creature.GetPower<KnowledgePower>(), -DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        }
    }
}