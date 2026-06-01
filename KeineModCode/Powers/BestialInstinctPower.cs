using KeineMod.KeineModCode.Core;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Powers;

public class BestialInstinctPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageAdditive(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (cardSource != null && Owner.Player != null && Owner == dealer && props.IsPoweredAttack() && KeineModel.IsInStance<HakutakuForm>(Owner.Player))
            return Amount;
        return 0M;
    }
}