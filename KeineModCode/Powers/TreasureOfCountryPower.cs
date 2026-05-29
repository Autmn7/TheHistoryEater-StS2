using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace KeineMod.KeineModCode.Powers;

public class TreasureOfCountryPower : KeineModPower, IOnConsumed
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard.Owner.Creature != Owner || player.Creature != Owner)
            return;
        switch (consumedCard)
        {
            case ScrollOfValor:
                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, Amount, Owner, null);
                break;
            case ScrollOfBenevolence:
                await PowerCmd.Apply<DexterityPower>(choiceContext, Owner, Amount, Owner, null);
                break;
            case ScrollOfWisdom:
                await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner, Amount, Owner, null);
                break;
        }
    }
}