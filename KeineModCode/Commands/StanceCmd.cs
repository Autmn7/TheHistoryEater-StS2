using KeineMod.KeineModCode.Core;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Commands;

public static class StanceCmd
{
    public static Task EnterHakutaku(PlayerChoiceContext choiceContext, Player player, CardModel? cardSource)
    {
        return KeineModel.SetStance<HakutakuForm>(choiceContext, player, cardSource);
    }

    public static Task ExitStance(PlayerChoiceContext choiceContext, Player player, CardModel? cardSource)
    {
        return KeineModel.SetStance<HumanForm>(choiceContext, player, cardSource);
    }
}