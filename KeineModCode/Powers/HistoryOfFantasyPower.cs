using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class HistoryOfFantasyPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || creator.Creature != Owner)
            return;
        Flash();
        var enemy = Owner.Player?.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
        if (enemy != null) await PowerCmd.Apply<HistoricalGapPower>(new ThrowingPlayerChoiceContext(), enemy, Amount, Owner, null);
    }
}