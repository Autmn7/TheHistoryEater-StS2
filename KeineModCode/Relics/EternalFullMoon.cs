using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace KeineMod.KeineModCode.Relics;

public class EternalFullMoon : KeineModRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new("ActiveTurn", 6M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ImperishableNightPower>()];

    public override bool ShowCounter => DisplayAmount > -1;

    public override int DisplayAmount
    {
        get
        {
            if (!CombatManager.Instance.IsInProgress)
                return -1;
            var intValue = DynamicVars["ActiveTurn"].IntValue;
            var roundNumber = Owner.Creature.CombatState.RoundNumber;
            return roundNumber >= intValue ? intValue : roundNumber;
        }
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature))
            return;
        InvokeDisplayAmountChanged();
        var intValue = DynamicVars["ActiveTurn"].IntValue;
        var roundNumber = Owner.Creature.CombatState.RoundNumber;
        if (roundNumber != intValue)
            return;
        Flash();
        await PowerCmd.Apply<ImperishableNightPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1, Owner.Creature, null);
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return Task.CompletedTask;
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }
}