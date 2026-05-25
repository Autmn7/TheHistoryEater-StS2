using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Common;

public class Leisure : KeineModCard
{
    public Leisure() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(6, 3);
        WithKeywords(KeineModKeywords.Create, KeineModKeywords.Human, KeineModKeywords.Consume, KeineModKeywords.Hakutaku, KeineModKeywords.Recall);
        WithTip(typeof(Flow));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await CreateCmd.Execute(CombatState.CreateCard<Flow>(Owner), Owner);
        if (InHuman())
            await ConsumeCmd.FromHand(choiceContext, Owner, 1, this);
        if (InHakutaku())
            await CreateCmd.Execute(CombatState.CreateCard<Flow>(Owner), Owner);
    }
}