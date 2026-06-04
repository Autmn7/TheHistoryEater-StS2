using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace KeineMod.KeineModCode.Cards.Rare;

public class FeastOfHistory : KeineModCard
{
    public FeastOfHistory() : base(3, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithPower<HistoricalGapPower>(3);
        WithCalculatedVar("Times", 2, 2, (card, _) => card is KeineModCard keineModCard && keineModCard.InHakutaku() ? 1 : 0);
        WithKeywords(KeineKeywords.Human, KeineKeywords.Consume, KeineKeywords.Hakutaku, CardKeyword.Exhaust);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (var i = 0; i < (int)((CalculatedVar)DynamicVars["Times"]).Calculate(cardPlay.Target); ++i)
            await PowerCmd.Apply<HistoricalGapPower>(choiceContext, CombatState.HittableEnemies, DynamicVars["HistoricalGapPower"].BaseValue, Owner.Creature, this);
        if (InHuman())
            await ConsumeCmd.EntireHand(choiceContext, Owner, this);
    }
}