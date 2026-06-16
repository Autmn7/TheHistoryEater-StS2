using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Rare;

public class GoWithTheFlow : KeineModCard
{
    public GoWithTheFlow() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithCards(2);
        WithTip(CardKeyword.Exhaust);
        WithTip(typeof(Flow));
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<GoWithTheFlowPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        for (var i = 0; i < DynamicVars.Cards.BaseValue; ++i)
        {
            CardModel flow = CombatState.CreateCard<Flow>(Owner);
            await CreateCmd.Execute(choiceContext, flow, Owner);
        }
    }
}