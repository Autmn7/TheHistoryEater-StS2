using Godot;
using KeineMod.KeineModCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace KeineMod.KeineModCode.Powers.Reincarnation;

public class ReincarnationPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;
    
    public override string CustomPackedIconPath
    {
        get
        {
            var path = "reincarnation_power.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = "reincarnation_power.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}