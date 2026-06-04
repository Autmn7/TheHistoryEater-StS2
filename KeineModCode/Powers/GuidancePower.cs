using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class GuidancePower : KeineModPower, IOnConsumedLate
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public async Task OnConsumedLate(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (player == Owner.Player && consumedCard.Owner == Owner.Player && !consumedCard.Keywords.Contains(CardKeyword.Unplayable))
            foreach (var creature in CombatState.GetTeammatesOf(Owner).Where(c => c.IsAlive && c.IsPlayer && c.HasPower<GuidedPower>() && c.GetPower<GuidedPower>()?.Applier == Owner))
            {
                var clone = consumedCard.CreateClone();
                if (creature.Player != null)
                    clone._owner = creature.Player;
                if (creature.Player?.Character.Id.ToString() == "CHARACTER.MOKOUMOD-MOKOU_MOD")
                    CardCmd.Upgrade(clone);
                await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Hand, Owner.Player);
            }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;
        await PowerCmd.Remove(this);
    }
}