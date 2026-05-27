using MegaCrit.Sts2.Core.Entities.Powers;

namespace KeineMod.KeineModCode.Powers;

public class TreasureOfWisdomPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
}