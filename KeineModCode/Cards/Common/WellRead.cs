using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Common;

public class WellRead : KeineModCard, IOnConsumed
{
    public WellRead() : base(4, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(10, 4);
        WithEnergy(1);
        WithKeyword(KeineKeywords.Knowledgeable);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }

    public Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard.Owner == Owner)
            EnergyCost.AddThisCombat(-DynamicVars.Energy.IntValue);
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || IsClone)
            return Task.CompletedTask;
        EnergyCost.AddThisCombat(-KeineConstantsStateRegistry.Get(Owner).CardsConsumedThisCombat * DynamicVars.Energy.IntValue);
        return Task.CompletedTask;
    }
}