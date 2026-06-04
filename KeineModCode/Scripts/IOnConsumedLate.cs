using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Scripts;

public interface IOnConsumedLate
{
    Task OnConsumedLate(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard);
}