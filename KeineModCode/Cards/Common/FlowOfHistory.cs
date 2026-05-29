using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Common;

public class FlowOfHistory : KeineModCard, IOnConsumed
{
    public FlowOfHistory() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithPower<HistoricalGapPower>(3, 1);
        WithCards(2);
        WithVar("Consumed", 1, 1);
        WithKeyword(KeineModKeywords.Create);
        WithTip(KeineModKeywords.Consume);
        WithTip(typeof(Flow));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<HistoricalGapPower>(choiceContext, cardPlay.Target, DynamicVars["HistoricalGapPower"].BaseValue, Owner.Creature, this);
        for (var i = 0; i < DynamicVars.Cards.BaseValue; ++i)
        {
            CardModel flow = CombatState.CreateCard<Flow>(Owner);
            await CreateCmd.Execute(flow, Owner);
        }
    }

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard == this)
            await CardPileCmd.Draw(choiceContext, DynamicVars["Consumed"].BaseValue, Owner);
    }
}