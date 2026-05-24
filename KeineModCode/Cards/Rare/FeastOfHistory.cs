using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Rare;

public class FeastOfHistory : KeineModCard
{
    public FeastOfHistory() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithPower<HistoricalGapPower>(3, 2);
        WithKeywords(KeineModKeywords.Human, KeineModKeywords.Consume, KeineModKeywords.Hakutaku, CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<HistoricalGapPower>(choiceContext, CombatState.HittableEnemies, DynamicVars["HistoricalGapPower"].BaseValue, Owner.Creature, this);
        if (InHakutaku())
            foreach (var enemy in CombatState.HittableEnemies)
                if (enemy.HasPower<HistoricalGapPower>())
                    await PowerCmd.Apply<HistoricalGapPower>(choiceContext, enemy, enemy.GetPowerAmount<HistoricalGapPower>(), Owner.Creature, this);
        if (InHuman())
            await ConsumeCmd.EntireHand(choiceContext, Owner, this);
    }
}