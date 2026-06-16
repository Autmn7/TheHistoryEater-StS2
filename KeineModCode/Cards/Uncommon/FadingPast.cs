using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class FadingPast : KeineModCard
{
    public FadingPast() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(1);
        WithKeywords(KeineKeywords.Recall, KeineKeywords.Create);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
        WithTip(typeof(Fatigue));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await RecallCmd.FromScroll(choiceContext, Owner, DynamicVars.Cards.IntValue);
        CardModel fatigue = CombatState.CreateCard<Fatigue>(Owner);
        await CreateCmd.Execute(choiceContext, fatigue, Owner);
    }
}