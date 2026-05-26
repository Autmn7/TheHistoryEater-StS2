using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;

namespace KeineMod.KeineModCode.Scripts;

public class KeineConstantsStateRegistry
{
    private static readonly Dictionary<CombatStateTracker, Dictionary<Player, KeineConstantsState>> states = new();

    public static KeineConstantsState Get(Player owner)
    {
        var tracker = CombatManager.Instance.StateTracker;

        if (!states.TryGetValue(tracker, out var perPlayer))
        {
            perPlayer = new Dictionary<Player, KeineConstantsState>();
            states[tracker] = perPlayer;
        }

        if (!perPlayer.TryGetValue(owner, out var state))
        {
            state = new KeineConstantsState();
            perPlayer[owner] = state;
        }

        return state;
    }

    public static void Clear(CombatStateTracker tracker)
    {
        states.Remove(tracker);
    }
}