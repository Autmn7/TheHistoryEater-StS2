using BaseLib.Utils;
using KeineMod.KeineModCode.UIs;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class Reconstruction : KeineModCard
{
    public Reconstruction() : base(1, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithTip(CardKeyword.Ethereal);
        WithTip(CardKeyword.Exhaust);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = (await CardSelectCmd.FromSimpleGrid(choiceContext, ScrollPile.Scroll.GetPile(Owner).Cards.ToList(), Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1))).FirstOrDefault();
        if (card != null)
        {
            var outputCard = card.CreateClone();
            CardCmd.ApplyKeyword(outputCard, CardKeyword.Ethereal);
            CardCmd.ApplyKeyword(outputCard, CardKeyword.Exhaust);
            await CardPileCmd.AddGeneratedCardToCombat(outputCard, PileType.Hand, Owner);
        }
    }
}