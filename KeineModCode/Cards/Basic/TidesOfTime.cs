using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Basic;

public class TidesOfTime : KeineModCard
{
    public TidesOfTime() : base(0, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithDamage(4, 2);
        WithPower<TimeShiftPower>(1, 1);
        WithKeyword(KeineKeywords.Fullmoon);
    }

    protected override bool ShouldGlowGoldInternal => !(KeineConstantsStateRegistry.Get(Owner).FullMoonCharge > 0);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext);
        if (!(KeineConstantsStateRegistry.Get(Owner).FullMoonCharge > 0))
            await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner.Creature, DynamicVars["TimeShiftPower"].BaseValue, Owner.Creature, this);
    }
}