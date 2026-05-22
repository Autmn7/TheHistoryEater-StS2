using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class HistoryOfUntrodden : KeineModCard
{
    public HistoryOfUntrodden() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("HistoryOfUntroddenPower", 2, 1);
        WithTip(typeof(HistoricalGapPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<HistoryOfUntroddenPower>(choiceContext, Owner.Creature, DynamicVars["HistoryOfUntroddenPower"].BaseValue, Owner.Creature, this);
    }
}