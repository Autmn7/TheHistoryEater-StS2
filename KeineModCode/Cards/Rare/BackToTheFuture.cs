using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;

namespace KeineMod.KeineModCode.Cards.Rare;

public class BackToTheFuture : KeineModCard
{
    public BackToTheFuture() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithCards(4);
        WithPower<TimeShiftPower>(4);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithKeywords(KeineModKeywords.Human, KeineModKeywords.Consume, CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner.Creature, DynamicVars["TimeShiftPower"].BaseValue, Owner.Creature, this);
        if (InHuman())
        {
            var drawPile = PileTypeExtensions.GetPile(PileType.Draw, Owner);
            var cardsToView = drawPile.Cards.Take(DynamicVars.Cards.IntValue).ToList();
            if (cardsToView.Count != 0)
            {
                var prefs = new CardSelectorPrefs(new LocString("card_selection", "TO_CONSUME_ANY_KEINE"), 0, DynamicVars.Cards.IntValue);
                var cardsToConsume = (await CardSelectCmd.FromSimpleGrid(choiceContext, cardsToView, Owner, prefs)).ToList();
                foreach (var card in cardsToConsume)
                    await ConsumeCmd.SpecificCard(choiceContext, card, Owner, this); 
            }
            await CardPileCmd.Draw(choiceContext, 1, Owner);
        }
    }
}