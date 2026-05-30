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
        WithCards(3, 1);
        WithPower<TimeShiftPower>(3, 1);
        WithKeywords(KeineModKeywords.Human, KeineModKeywords.Consume);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner.Creature, DynamicVars["TimeShiftPower"].BaseValue, Owner.Creature, this);
        if (InHuman())
            await ConsumeCmd.SpecificCard(choiceContext, this, Owner, this);
    }
}