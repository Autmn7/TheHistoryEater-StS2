using BaseLib.Hooks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace KeineMod.KeineModCode.Powers;

public class EruditePower : KeineModPower, IMaxHandSizeModifier
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public int ModifyMaxHandSize(Player player, int currentMaxHandSize)
    {
        if (player.Creature != Owner)
            return currentMaxHandSize;
        currentMaxHandSize += Amount;
        return currentMaxHandSize;
    }
}