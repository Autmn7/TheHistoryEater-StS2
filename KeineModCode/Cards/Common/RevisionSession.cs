using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Common;

public class RevisionSession : KeineModCard, IOnConsumed
{
    public RevisionSession() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(11, 4);
        WithPower<KnowledgePower>(1);
        WithKeywords(KeineModKeywords.Knowledgeable, CardKeyword.Retain);
        WithTip(KeineModKeywords.Consume);
        WithTip(KeineModKeywords.Recall);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel card)
    {
        if (card == this)
            await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
    }
}