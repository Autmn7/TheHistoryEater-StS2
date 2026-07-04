using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class UnofficialHistory : KeineModCard
{
    public UnofficialHistory() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithPower<HistoricalGapPower>(8, 2);
        WithVar("WeakGain", 2);
        WithPower<WeakPower>(1, 1);
        WithKeywords(KeineKeywords.Human, KeineKeywords.Hakutaku);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<HistoricalGapPower>(choiceContext, cardPlay.Target, DynamicVars["HistoricalGapPower"].BaseValue, Owner.Creature, this);
        if (!InHakutaku())
            await PowerCmd.Apply<WeakPower>(choiceContext, Owner.Creature, DynamicVars["WeakGain"].BaseValue, Owner.Creature, this);
        if (InHuman())
            await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this);
    }
}