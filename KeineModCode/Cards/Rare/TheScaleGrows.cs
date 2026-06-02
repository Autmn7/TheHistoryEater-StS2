using BaseLib.Extensions;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Rare;

public class TheScaleGrows : KeineModCard
{
    public TheScaleGrows() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("TheScaleGrowsPower", 2);
        WithVar("StackOverflowPower", 20);
        WithTip(typeof(KnowledgePower));
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<TheScaleGrowsPower>(choiceContext, Owner.Creature, DynamicVars["TheScaleGrowsPower"].BaseValue, Owner.Creature, this);
        if (!Owner.HasPower<StackOverflowPower>())
            await PowerCmd.Apply<StackOverflowPower>(choiceContext, Owner.Creature, DynamicVars["StackOverflowPower"].BaseValue, Owner.Creature, this);
    }
}