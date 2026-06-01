using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class Guidance : KeineModCard
{
    public Guidance() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(7, 3);
        WithKeywords(KeineModKeywords.Knowledgeable);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        var card = (await CardSelectCmd.FromHand(choiceContext, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), (Func<CardModel, bool>) (c => !c.Keywords.Contains(KeineModKeywords.Knowledgeable)), this)).FirstOrDefault();
        if (card == null)
            return;
        CardCmd.ApplyKeyword(card, KeineModKeywords.Knowledgeable);
    }
}