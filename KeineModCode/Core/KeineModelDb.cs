using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Core;

public class KeineModelDb
{
    public static T KeineStance<T>() where T : KeineStanceModel
    {
        return ModelDb.GetById<T>(ModelDb.GetId<T>());
    }
}