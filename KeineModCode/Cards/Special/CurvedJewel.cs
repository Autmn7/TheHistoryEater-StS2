using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class CurvedJewel : KeineModCard
{
    public CurvedJewel() : base(2, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithCalculatedBlock(10, (card, _) => card.Owner.Creature.GetPowerAmount<BenevolencePower>());
        WithCalculatedVar("RetainBlock", 0, (card, _) => card.Owner.Creature.GetPowerAmount<TreasureOfBenevolencePower>());
        WithKeywords(CardKeyword.Exhaust);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), DynamicVars.CalculatedBlock.Props, cardPlay);
        if (((CalculatedVar)DynamicVars["RetainBlock"]).Calculate(cardPlay.Target) > 0)
            await PowerCmd.Apply<BlurPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}