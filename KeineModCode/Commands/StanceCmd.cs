using KeineMod.KeineModCode.Core;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Commands;

public static class StanceCmd
{
    public static Task EnterHakutaku(PlayerChoiceContext ctx, Player player, CardModel? cardSource)
    {
        return KeineModel.SetStance<HakutakuForm>(ctx, player, cardSource);
    }

    public static Task ExitStance(PlayerChoiceContext ctx, Player player, CardModel? cardSource)
    {
        return KeineModel.SetStance<HumanForm>(ctx, player, cardSource);
    }
}