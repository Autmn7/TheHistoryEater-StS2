using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Cards.Rare;

public class Amaterasu : KeineModCard
{
    public Amaterasu() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithCalculatedDamage(9, (card, _) => card is KeineModCard keineModCard && keineModCard.InHakutaku() ? card.Owner.Creature.GetPowerAmount<ValorPower>() : 0, ValueProp.Move, 1);
        WithPower<ValorPower>(3, 2);
        WithKeywords(KeineModKeywords.Human, KeineModKeywords.Hakutaku);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (var enemy in CombatState.HittableEnemies)
        {
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(enemy));
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireBurstVfx.Create(enemy, 0.75f));
        }

        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).TargetingAllOpponents(CombatState).Execute(choiceContext);
        if (InHuman())
            await PowerCmd.Apply<ValorPower>(choiceContext, Owner.Creature, DynamicVars["ValorPower"].BaseValue, Owner.Creature, this);
    }
}