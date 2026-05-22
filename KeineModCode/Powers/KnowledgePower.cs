using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Powers;

public class KnowledgePower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        return player != Owner.Player ? count : count + (decimal)Math.Floor(Amount / 2.0);
    }

    public override decimal ModifyDamageAdditive(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (cardSource != null && cardSource.Keywords.Contains(KeineModKeywords.Knowledgeable))
            return Owner != dealer || !props.IsPoweredAttack() ? 0M : Amount * (cardSource.DynamicVars.TryGetValue("Knowledgeable", out var v) ? v.BaseValue : 0);
        return 0M;
    }

    public override decimal ModifyBlockAdditive(
        Creature target,
        decimal block,
        ValueProp props,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (cardSource != null && cardSource.Keywords.Contains(KeineModKeywords.Knowledgeable))
            return !props.IsPoweredCardOrMonsterMoveBlock() ? 0M : Amount * (cardSource.DynamicVars.TryGetValue("Knowledgeable", out var v) ? v.BaseValue : 0);
        return 0M;
    }
}