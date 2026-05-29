using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Common;

public class SwirlOfHistory : KeineModCard, IOnConsumed
{
    public SwirlOfHistory() : base(2, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
    {
        WithDamage(12, 3);
        WithPower<HistoricalGapPower>(3, 1);
        WithVar("Consumed", 2, 1);
        WithTip(KeineModKeywords.Consume);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState).WithHitFx("vfx/vfx_giant_horizontal_slash").Execute(choiceContext);
        await PowerCmd.Apply<HistoricalGapPower>(choiceContext, CombatState.HittableEnemies, DynamicVars["HistoricalGapPower"].BaseValue, Owner.Creature, this);
    }

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard == this)
            await PowerCmd.Apply<HistoricalGapPower>(choiceContext, CombatState.HittableEnemies, DynamicVars["Consumed"].BaseValue, Owner.Creature, this);
    }
}