using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Scripts;

public class KeineHooks
{
    private static async Task Dispatch<T>(PlayerChoiceContext choiceContext, Player player, Func<T, Task> invoke) where T : class
    {
        var combatState = player.Creature.CombatState;
        if (combatState == null) return;
        foreach (var model in combatState.IterateHookListeners().OfType<T>())
        {
            var abstractModel = model as AbstractModel;
            choiceContext.PushModel(abstractModel);
            await invoke(model);
            choiceContext.PopModel(abstractModel);
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

    public static Task OnConsumedLate(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        return Dispatch(choiceContext, player, (IOnConsumedLate m) => m.OnConsumedLate(choiceContext, player, consumedCard));
    }
}