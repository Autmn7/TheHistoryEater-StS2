using KeineMod.KeineModCode.Cards.Special;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class UnlimitedBladeWorksPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power.Owner == Owner && applier == Owner && power is ValorPower && amount > 0)
            foreach (var player in CombatState.Players.Where(p => p.Creature.IsAlive && p != Owner.Player))
                await PowerCmd.Apply<ValorPower>(choiceContext, player.Creature, amount, Owner, null);
    }

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (clonedBy is UnlimitedBladeWorksPower)
            return;
        if (card.Owner.Creature == Owner && card is HeavenlySword && card.Pile?.Type == PileType.Hand)
            foreach (var player in CombatState.Players.Where(p => p.Creature.IsAlive && p != Owner.Player))
            {
                var clone = card.CreateClone();
                clone._owner = player;
                if (player.Character.Id.ToString() == "CHARACTER.MOKOUMOD-MOKOU_MOD")
                    clone.EnergyCost.SetUntilPlayed(0);
                await CardPileCmd.Add(clone, PileType.Hand, clonedBy: this);
            }
    }
}