using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Common;

public class LongRead : KeineModCard
{
    public LongRead() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithCards(3, 1);
        WithKeywords(KeineKeywords.Human, KeineKeywords.Consume);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        if (InHuman())
            await ConsumeCmd.FromHand(choiceContext, Owner, 1, this);
    }
}