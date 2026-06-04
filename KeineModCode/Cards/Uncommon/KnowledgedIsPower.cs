using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class KnowledgeIsPower : KeineModCard
{
    public KnowledgeIsPower() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("KnowledgeIsPowerPower", 1);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<KnowledgeIsPowerPower>(choiceContext, Owner.Creature, DynamicVars["KnowledgeIsPowerPower"].BaseValue, Owner.Creature, this);
    }
}