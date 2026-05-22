using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace KeineMod.KeineModCode.Cards.Rare;

public class GhqCrisis : KeineModCard
{
    public GhqCrisis() : base(4, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(3, 1);
        WithVar(new RepeatVar(4));
        WithCards(4);
        WithKeywords(KeineModKeywords.Hakutaku);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState).WithHitCount(DynamicVars.Repeat.IntValue).Execute(choiceContext);
        if (InHakutaku())
        {
            await RecallCmd.FromScrollUpTo(choiceContext, Owner, DynamicVars.Cards.IntValue);
            await CardCmd.Exhaust(choiceContext, this);
        }
    }
}