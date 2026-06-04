using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class Flow : KeineModCard, IOnConsumed
{
    private bool ToDrawByEthereal { get; set; }

    public Flow() : base(-1, CardType.Status, CardRarity.Status, TargetType.Self)
    {
        WithKeywords(CardKeyword.Unplayable, CardKeyword.Ethereal);
        WithTip(KeineKeywords.Consume);
    }

    public override int MaxUpgradeLevel => 0;

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard != this) return;
        await CardCmd.Exhaust(choiceContext, this);
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this)
        {
            ToDrawByEthereal = causedByEthereal;
            if (!causedByEthereal)
                await CardPileCmd.Draw(choiceContext, 1, Owner);
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature))
            return;
        if (ToDrawByEthereal)
        {
            ToDrawByEthereal = false;
            await CardPileCmd.Draw(choiceContext, 1, Owner);
        }
    }

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power.Owner == Owner.Creature && power is GoWithTheFlowPower)
        {
            CardCmd.RemoveKeyword(this, CardKeyword.Unplayable);
            CardCmd.ApplyKeyword(this, CardKeyword.Exhaust);
            EnergyCost.SetThisCombat(0);
        }

        base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || IsClone || !Owner.Creature.HasPower<GoWithTheFlowPower>())
            return Task.CompletedTask;
        CardCmd.RemoveKeyword(this, CardKeyword.Unplayable);
        CardCmd.ApplyKeyword(this, CardKeyword.Exhaust);
        EnergyCost.SetThisCombat(0);
        return Task.CompletedTask;
    }
}