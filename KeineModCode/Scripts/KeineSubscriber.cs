using KeineMod.KeineModCode.Core;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Scripts;

public static class KeineSubscriber
{
    public static void Subscribe()
    {
        ModHelper.SubscribeForCombatStateHooks("KeineMod", CollectModels);
    }

    public static IEnumerable<AbstractModel> CollectModels(
        CombatState combatState)
    {
        return combatState.Players.Select(KeineModel.GetStanceModel).Where(stance => stance is not HumanForm);
    }
}