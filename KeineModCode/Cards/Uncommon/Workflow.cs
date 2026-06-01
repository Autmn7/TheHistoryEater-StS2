using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class Workflow : KeineModCard
{
    public Workflow() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(4, 3);
        WithPower<KnowledgePower>(1);
        WithKeywords(KeineModKeywords.Human, KeineModKeywords.Consume, KeineModKeywords.Create, KeineModKeywords.Hakutaku);
        WithTip(typeof(Flow));
        WithTip(typeof(ScrollOfWisdom));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        if (InHuman())
        {
            var consumedCard = await ConsumeCmd.FromHandSingle(choiceContext, Owner, this);
            if (consumedCard is ScrollOfValor or ScrollOfBenevolence or ScrollOfWisdom)
            {
                CardModel flow = CombatState.CreateCard<Flow>(Owner);
                await CreateCmd.Execute(flow, Owner);
            }
        }

        if (InHakutaku())
        {
            CardModel wisdom = CombatState.CreateCard<ScrollOfWisdom>(Owner);
            await CreateCmd.Execute(wisdom, Owner);
        }
    }
}