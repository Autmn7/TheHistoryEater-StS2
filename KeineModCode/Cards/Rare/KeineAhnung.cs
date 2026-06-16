using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Rare;

public class KeineAhnung : KeineModCard
{
    public KeineAhnung() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeywords(KeineKeywords.Create, CardKeyword.Exhaust);
        WithTip(typeof(TheSmartest));
        WithTip(typeof(TheStrongest));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel created = CombatState.CreateCard<TheSmartest>(Owner);
        if (IsUpgraded)
            await CreateCmd.Execute(choiceContext, created, Owner);
        else
            await CreateCmd.Execute(choiceContext, created, Owner, false, PileType.Draw, false, CardPilePosition.Random);
    }
}