using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Rare;

public class DualForm : KeineModCard
{
    public DualForm() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithTip(KeineKeywords.Human);
        WithTip(KeineKeywords.Hakutaku);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<DualFormPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}