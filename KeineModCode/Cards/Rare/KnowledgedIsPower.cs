using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Rare;

public class KnowledgeIsPower : KeineModCard
{
    public KnowledgeIsPower() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("KnowledgeIsPowerPower", 1);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<KnowledgeIsPowerPower>(choiceContext, Owner.Creature, DynamicVars["KnowledgeIsPowerPower"].BaseValue, Owner.Creature, this);
    }
}