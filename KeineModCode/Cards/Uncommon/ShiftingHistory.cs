using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class ShiftingHistory : KeineModCard
{
    public ShiftingHistory() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithPower<HistoricalGapPower>(4, 2);
        WithTip(typeof(TimeShiftPower));
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<HistoricalGapPower>(choiceContext, cardPlay.Target, DynamicVars["HistoricalGapPower"].BaseValue, Owner.Creature, this);
        int gapAmount = cardPlay.Target.GetPowerAmount<HistoricalGapPower>();
        if (gapAmount > 0)
            await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner.Creature, gapAmount, Owner.Creature, this);
    }
}