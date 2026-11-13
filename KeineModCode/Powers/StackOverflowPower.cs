using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Powers;

public class StackOverflowPower : KeineModPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new("DamageTaken", 0M)];

    private void RefreshDamageTakenPreview()
    {
        DynamicVars["DamageTaken"].PreviewValue =
            Math.Floor(Owner.GetPowerAmount<KnowledgePower>() / 2M);
    }

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        await base.AfterApplied(applier, cardSource);
        RefreshDamageTakenPreview();
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        await base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);

        if (power is KnowledgePower && power.Owner == Owner)
            RefreshDamageTakenPreview();
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;
        VfxCmd.PlayOnCreatureCenter(Owner, "vfx/vfx_starry_impact");
        await CreatureCmd.Damage(choiceContext, Owner, (int)Math.Floor(Owner.GetPowerAmount<KnowledgePower>() / 2.0), ValueProp.Unpowered, null, null);
    }
}