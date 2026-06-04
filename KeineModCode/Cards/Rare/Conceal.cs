using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace KeineMod.KeineModCode.Cards.Rare;

public class Conceal : KeineModCard
{
    public Conceal() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithBlock(7, 2);
        WithPower<HistoricalGapPower>(2, 1);
        WithKeywords(KeineKeywords.Human, KeineKeywords.Hakutaku);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<HistoricalGapPower>(choiceContext, cardPlay.Target, DynamicVars["HistoricalGapPower"].BaseValue, Owner.Creature, this);
        if (InHuman())
            await PowerCmd.Apply<BlurPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
        if (InHakutaku())
            await PowerCmd.Apply<ConcealHistoryPower>(choiceContext, cardPlay.Target, 1, Owner.Creature, this);
    }
}