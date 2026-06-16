using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Powers;

public class SandpitMarkPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier;

    public override async Task AfterRemoved(Creature oldOwner)
    {
        if (Applier?.Player?.RunState.CurrentRoom is { RoomType: RoomType.Boss } && !Owner.HasPower<MinionPower>())
        {
            var hpLoss = Owner.MaxHp * 0.5;
            var modifier = Applier.CombatState != null ? 2.0 / (Applier.CombatState.Players.Count + 1) : 0.0;
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner, (int)(hpLoss * modifier), ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, (Creature)null);
        }
        else
        {
            await CreatureCmd.Kill(Owner);
        }
    }
}