using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class ScrollDance : KeineModCard
{
    public ScrollDance() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("ScrollDancePower", 1);
        WithTip(KeineModKeywords.Consume);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<ScrollDancePower>(choiceContext, Owner.Creature, DynamicVars["ScrollDancePower"].BaseValue, Owner.Creature, this);
    }
}