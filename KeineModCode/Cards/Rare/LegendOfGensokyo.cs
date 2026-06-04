using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Rare;

public class LegendOfGensokyo : KeineModCard
{
    public LegendOfGensokyo() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("LegendOfGensokyoPower", 1);
        WithTip(KeineKeywords.Knowledgeable);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<LegendOfGensokyoPower>(choiceContext, Owner.Creature, DynamicVars["LegendOfGensokyoPower"].BaseValue, Owner.Creature, this);
    }
}