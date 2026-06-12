using BaseLib.Utils;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
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
        WithCards(1);
        WithKeywords(KeineKeywords.Recall);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await RecallCmd.FromScroll(choiceContext, Owner, DynamicVars.Cards.IntValue, false, true);
    }
}