using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class EightSpanMirror : KeineModCard
{
    public EightSpanMirror() : base(2, CardType.Power, CardRarity.Token, TargetType.Self)
    {
        WithVar("MirroredPower", 1);
        WithTags(KeineTags.Sacred);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<MirroredPower>(choiceContext, Owner.Creature, DynamicVars["MirroredPower"].BaseValue, Owner.Creature, this);
    }

    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power.Owner == Owner.Creature && power is TreasureOfWisdomPower)
            EnergyCost.AddThisCombat((int)-amount);
        base.AfterPowerAmountChanged(choiceContext, power, amount, applier, cardSource);
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || IsClone || !Owner.Creature.HasPower<TreasureOfWisdomPower>())
            return Task.CompletedTask;
        EnergyCost.AddThisCombat(-Owner.Creature.GetPowerAmount<TreasureOfWisdomPower>());
        return Task.CompletedTask;
    }
}