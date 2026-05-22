using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Scripts;

public class KeineHooks
{
    private static async Task Dispatch<T>(PlayerChoiceContext ctx, Player player, Func<T, Task> invoke) where T : class
    {
        var combatState = player.Creature.CombatState;
        if (combatState == null) return;
        foreach (var model in combatState.IterateHookListeners().OfType<T>())
        {
            var abstractModel = model as AbstractModel;
            ctx.PushModel(abstractModel);
            await invoke(model);
            ctx.PopModel(abstractModel);
        }
    }

    public static Task OnStanceChange(PlayerChoiceContext choiceContext, Player player, KeineStanceModel oldStance, KeineStanceModel newStance)
    {
        return Dispatch(choiceContext, player, (IOnStanceChange m) => m.OnStanceChange(choiceContext, player, oldStance, newStance));
    }

    public static Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        return Dispatch(choiceContext, player, (IOnConsumed m) => m.OnConsumed(choiceContext, player, consumedCard));
    }
}