using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Common;

public class GoodAppleBadApple : KeineModCard
{
    public GoodAppleBadApple() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithPower<BenevolencePower>(2, 2);
        WithPower<ValorPower>(2, 2);
        WithCards(2);
        WithEnergy(1);
        WithKeywords(KeineModKeywords.Human, KeineModKeywords.Hakutaku);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BenevolencePower>(choiceContext, Owner.Creature, DynamicVars["BenevolencePower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<ValorPower>(choiceContext, Owner.Creature, DynamicVars["ValorPower"].BaseValue, Owner.Creature, this);
        if (InHuman())
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        if (InHakutaku())
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
}