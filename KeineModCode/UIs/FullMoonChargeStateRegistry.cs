using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;

namespace KeineMod.KeineModCode.UIs;

public class FullMoonChargeStateRegistry
{
    private static readonly Dictionary<CombatStateTracker, Dictionary<Player, FullMoonChargeState>> states = new();

    public static FullMoonChargeState Get(Player owner)
    {
        var tracker = CombatManager.Instance.StateTracker;

        if (!states.TryGetValue(tracker, out var perPlayer))
        {
            perPlayer = new Dictionary<Player, FullMoonChargeState>();
            states[tracker] = perPlayer;
        }

        if (!perPlayer.TryGetValue(owner, out var state))
        {
            state = new FullMoonChargeState();
            perPlayer[owner] = state;
        }

        return state;
    }

    public static void Clear(CombatStateTracker tracker)
    {
        states.Remove(tracker);
    }
}