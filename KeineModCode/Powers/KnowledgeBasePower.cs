using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class KnowledgeBasePower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || creator.Creature != Owner)
            return;
        if (card.Type != CardType.Status && card.Rarity != CardRarity.Status)
        {
            Flash();
            await PowerCmd.Apply<KnowledgePower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
        }
    }
}