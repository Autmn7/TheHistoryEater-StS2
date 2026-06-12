using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class HistoryOfFantasy : KeineModCard
{
    public HistoryOfFantasy() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("HistoryOfFantasyPower", 1);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithTip(typeof(HistoricalGapPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<HistoryOfFantasyPower>(choiceContext, Owner.Creature, DynamicVars["HistoryOfFantasyPower"].BaseValue, Owner.Creature, this);
    }
}