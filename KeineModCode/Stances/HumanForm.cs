using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace KeineMod.KeineModCode.Stances;

public class HumanForm : KeineStanceModel
{
    public override bool ShouldReceiveCombatHooks => false;

    protected override StanceVfxConfig VfxConfig => new(null, null, null, null, (ShakeStrength)0);
}