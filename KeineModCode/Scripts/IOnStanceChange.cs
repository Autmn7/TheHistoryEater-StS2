using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Scripts;

public interface IOnStanceChange
{
    Task OnStanceChange(PlayerChoiceContext choiceContext, Player player, KeineStanceModel oldStance, KeineStanceModel newStance);
}