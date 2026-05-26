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
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithKeywords(KeineModKeywords.Hakutaku, KeineModKeywords.Recall, KeineModKeywords.Create, CardKeyword.Exhaust);
        WithTip(typeof(TheSmartest));
        WithTip(typeof(TheStrongest));
    }

    protected override bool ShouldGlowRedInternal => !InHakutaku();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (InHakutaku())
        {
            CardModel created = CombatState.CreateCard<TheSmartest>(Owner);
            await CreateCmd.Execute(created, Owner);
        }
    }
}