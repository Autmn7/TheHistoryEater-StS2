using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace KeineMod.KeineModCode.Cards.Rare;

public class TheScaleGrows : KeineModCard
{
    public TheScaleGrows() : base(0, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<KnowledgePower>(6, 3);
        WithPower<VulnerablePower>(999);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, Owner.Creature, DynamicVars["VulnerablePower"].BaseValue, Owner.Creature, this);
    }
}