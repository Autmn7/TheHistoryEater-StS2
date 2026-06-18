using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class UndocumentedEra : KeineModCard
{
    public UndocumentedEra() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithPower<HistoricalGapPower>(3, 1);
        WithBlock(3, 1);
        WithKeywords(KeineKeywords.Human, KeineKeywords.Hakutaku);
    }

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var xValue = ResolveEnergyXValue();
        if (InHakutaku())
            xValue++;
        for (var i = 0; i < xValue; ++i)
            await PowerCmd.Apply<HistoricalGapPower>(choiceContext, cardPlay.Target, DynamicVars["HistoricalGapPower"].BaseValue, Owner.Creature, this);
        if (InHuman())
            for (var i = 0; i < xValue; ++i)
                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }
}