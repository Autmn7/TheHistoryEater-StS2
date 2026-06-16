using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.UIs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class RainOfShowa : KeineModCard
{
    public RainOfShowa() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy)
    {
        WithDamage(4, 1);
        WithCalculatedVar("Repeat", 1, (card, _) => PileType.Exhaust.GetPile(card.Owner).Cards.Concat(ScrollPile.Scroll.GetPile(card.Owner).Cards).Count(c => c is Flow));
        WithCards(1);
        WithTip(typeof(Flow));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingRandomOpponents(CombatState).WithHitCount((int)((CalculatedVar)DynamicVars["Repeat"]).Calculate(cardPlay.Target)).Execute(choiceContext);
        for (var i = 0; i < DynamicVars.Cards.BaseValue; ++i)
        {
            CardModel flow = CombatState.CreateCard<Flow>(Owner);
            await CreateCmd.Execute(choiceContext, flow, Owner);
        }
    }
}