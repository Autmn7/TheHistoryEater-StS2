using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class DupRekindlePower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("HealPercent")];

    private decimal GetHealPercent()
    {
        var player = Owner.Player;
        if (player == null)
            return 15M;
        var tempEssence = 0;
        if (Owner.HasPower<DupTempEssencePower>()) tempEssence = Owner.GetPowerAmount<DupTempEssencePower>();
        return 15M + tempEssence * 5M;
    }

    private void UpdateHealPercentVar()
    {
        var value = (int)GetHealPercent();
        var dynamicVar = (StringVar)DynamicVars["HealPercent"];
        dynamicVar.StringValue = value.ToString();
    }

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (power is DupRekindlePower or DupTempEssencePower) UpdateHealPercentVar();

        return base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
    }

    public override bool ShouldDie(Creature creature)
    {
        return creature != Owner;
    }

    public override async Task AfterPreventingDeath(Creature creature)
    {
        if (creature.Player == null || creature != Owner)
            return;

        var healPercent = GetHealPercent();
        var healAmount = Math.Max(1M, creature.MaxHp * (healPercent / 100M));
        await CreatureCmd.Heal(creature, healAmount);

        await PowerCmd.Decrement(this);
    }
}