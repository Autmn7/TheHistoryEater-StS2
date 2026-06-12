using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class SidesOfTheMoon : KeineModCard
{
    public SidesOfTheMoon() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithCalculatedVar("Gap", 2, 2, (card, _) => card is KeineModCard keineModCard && keineModCard.InHakutaku() ? 1 : 0, 2);
        WithVar("Ratio", 2);
        WithKeywords(CardKeyword.Retain, KeineKeywords.Human, KeineKeywords.Hakutaku);
        WithTip(typeof(TimeShiftPower));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<HistoricalGapPower>(choiceContext, cardPlay.Target, (int)((CalculatedVar)DynamicVars["Gap"]).Calculate(cardPlay.Target), Owner.Creature, this);
        if (InHuman())
        {
            var gapAmount = (int)Math.Floor(cardPlay.Target.GetPowerAmount<HistoricalGapPower>() / DynamicVars["Ratio"].BaseValue);
            if (gapAmount > 0)
                await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner.Creature, gapAmount, Owner.Creature, this);
        }
    }
}