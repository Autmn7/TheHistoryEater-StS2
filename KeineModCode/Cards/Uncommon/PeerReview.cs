using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class PeerReview : KeineModCard
{
    public PeerReview() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.AnyAlly)
    {
        WithCalculatedVar("PeerReviewPower", 2, (_, target) => target?.Player?.Character.Id.ToString() == "CHARACTER.MOKOUMOD-MOKOU_MOD" ? 1 : 0);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithTip(typeof(HistoricalGapPower));
        WithTip(new TooltipSource(card => new HoverTip(new LocString("cards", Id.Entry + ".extraTipTitle"), new LocString("cards", Id.Entry + ".extraTipDescription"))));
    }

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target?.Player == null)
            return;
        await PowerCmd.Apply<PeerReviewPower>(choiceContext, cardPlay.Target, ((CalculatedVar)DynamicVars["PeerReviewPower"]).Calculate(cardPlay.Target), Owner.Creature, this);
    }
}