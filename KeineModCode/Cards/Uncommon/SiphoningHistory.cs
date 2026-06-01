using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class SiphoningHistory : KeineModCard
{
    public SiphoningHistory() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVar("Ratio", 4, -1);
        WithKeywords(KeineModKeywords.Hakutaku, KeineModKeywords.Create, CardKeyword.Exhaust);
        WithTip(typeof(SplittingHistory));
        WithTip(typeof(HistoricalGapPower));
        WithTip(typeof(StrengthPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        decimal gapRemoved = cardPlay.Target.GetPowerAmount<HistoricalGapPower>();
        var strengthDownAmount = (int)Math.Floor(gapRemoved / DynamicVars["Ratio"].BaseValue);
        await PowerCmd.Remove(cardPlay.Target.GetPower<HistoricalGapPower>());
        if (strengthDownAmount > 0)
            await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, -strengthDownAmount, Owner.Creature, this);
        if (InHakutaku())
        {
            var splittingHistory = CombatState.CreateCard<SplittingHistory>(Owner);
            splittingHistory.GapAmt = gapRemoved;
            await CreateCmd.Execute(splittingHistory, Owner, false, PileType.Discard);
        }
    }
}