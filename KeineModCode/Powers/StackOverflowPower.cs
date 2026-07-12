using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Powers;

public class StackOverflowPower : KeineModPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StackOverflowVar("DamageTaken", 2)];

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;
        VfxCmd.PlayOnCreatureCenter(Owner, "vfx/vfx_starry_impact");
        await CreatureCmd.Damage(choiceContext, Owner, (int)Math.Floor(Owner.GetPowerAmount<KnowledgePower>() / 2.0), ValueProp.Unpowered, null, null);
    }
}