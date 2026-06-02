using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace KeineMod.KeineModCode.Cards.Rare;

public class BestialInstinct : KeineModCard
{
    public BestialInstinct() : base(2, CardType.Power, CardRarity.Rare, TargetType.AnyAlly)
    {
        WithPower<TimeShiftPower>(12);
        WithCalculatedVar("BestialInstinctPower", 3, (_, target) => target?.Player?.Character.Id.ToString() == "CHARACTER.MOKOUMOD-MOKOU_MOD" ? 2 : 0, 2);
        WithTip(typeof(BestialInstinctPower));
        WithTip(new TooltipSource(card => new HoverTip(new LocString("cards", Id.Entry + ".extraTipTitle"), new LocString("cards", Id.Entry + ".extraTipDescription"))));
    }

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BestialInstinctPower>(choiceContext, Owner.Creature, ((CalculatedVar)DynamicVars["BestialInstinctPower"]).Calculate(cardPlay.Target), Owner.Creature, this);
        await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner.Creature, DynamicVars["TimeShiftPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<BestialInstinctPower>(choiceContext, cardPlay.Target, ((CalculatedVar)DynamicVars["BestialInstinctPower"]).Calculate(cardPlay.Target), Owner.Creature, this);
        await PowerCmd.Apply<TimeShiftPower>(choiceContext, cardPlay.Target, DynamicVars["TimeShiftPower"].BaseValue, Owner.Creature, this);
    }
}