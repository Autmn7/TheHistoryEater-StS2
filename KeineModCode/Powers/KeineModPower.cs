using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using KeineMod.KeineModCode.Extensions;

namespace KeineMod.KeineModCode.Powers;

public abstract class KeineModPower : CustomPowerModel
{
    //Loads from KeineMod/images/powers/your_power.png
    public override string CustomPackedIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".PowerImagePath();
        }
    }

    public override string CustomBigIconPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
            return ResourceLoader.Exists(path) ? path : "power.png".BigPowerImagePath();
        }
    }
}