using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Rare;

public class KnowledgeBase : KeineModCard
{
    public KnowledgeBase() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("KnowledgeBasePower", 1);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithTip(typeof(KnowledgePower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<KnowledgeBasePower>(choiceContext, Owner.Creature, DynamicVars["KnowledgeBasePower"].BaseValue, Owner.Creature, this);
    }
}