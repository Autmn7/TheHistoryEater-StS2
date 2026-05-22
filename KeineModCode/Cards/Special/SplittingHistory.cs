using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class SplittingHistory : KeineModCard
{
    private decimal _gapAmt;

    public SplittingHistory() : base(1, CardType.Skill, CardRarity.Token, TargetType.AnyEnemy)
    {
        WithPower<HistoricalGapPower>((int)GapAmt);
        WithKeywords(CardKeyword.Exhaust);
        WithCostUpgradeBy(-1);
    }

    public decimal GapAmt
    {
        get => _gapAmt;
        set
        {
            _gapAmt = value;
            if (DynamicVars.ContainsKey("HistoricalGapPower")) DynamicVars["HistoricalGapPower"].BaseValue = value;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<HistoricalGapPower>(choiceContext, cardPlay.Target, GapAmt, Owner.Creature, this);
    }
}