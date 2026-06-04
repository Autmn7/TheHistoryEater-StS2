using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Rare;

public class PassageOfTime : KeineModCard
{
    public PassageOfTime() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithPower<TimeShiftPower>(2, 1);
        WithCards(2, 1);
        WithEnergy(1);
        WithKeywords(KeineKeywords.Human, KeineKeywords.Consume, KeineKeywords.Hakutaku, CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner.Creature, DynamicVars["TimeShiftPower"].BaseValue, Owner.Creature, this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        if (InHuman())
            await ConsumeCmd.FromHandUpTo(choiceContext, Owner, DynamicVars.Cards.IntValue, this);
        if (InHakutaku())
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
}