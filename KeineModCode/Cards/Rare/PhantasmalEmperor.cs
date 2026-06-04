using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace KeineMod.KeineModCode.Cards.Rare;

public class PhantasmalEmperor : KeineModCard
{
    public PhantasmalEmperor() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(6);
        WithVar("Repeat", 4, 1);
        WithPower<KnowledgePower>(3);
        WithKeyword(KeineKeywords.Knowledgeable);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (var enemy in CombatState.HittableEnemies.ToList())
        {
            var vfx = NHyperbeamVfx.Create(Owner.Creature, enemy);
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(vfx);
        }

        await Cmd.Wait(0.5f);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState).WithHitCount(DynamicVars["Repeat"].IntValue).Execute(choiceContext);
        if (Owner.Creature.GetPower<KnowledgePower>() != null)
            await PowerCmd.ModifyAmount(choiceContext, Owner.Creature.GetPower<KnowledgePower>(), -DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
    }
}