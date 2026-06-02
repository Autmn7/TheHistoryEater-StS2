using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class InternalizePower : KeineModPower, IOnConsumed
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard.Owner.Creature != Owner || player.Creature != Owner)
            return;
        await PowerCmd.Apply<ValorPower>(choiceContext, Owner, Amount, Owner, null);
        await PowerCmd.Apply<BenevolencePower>(choiceContext, Owner, Amount, Owner, null);
    }
}