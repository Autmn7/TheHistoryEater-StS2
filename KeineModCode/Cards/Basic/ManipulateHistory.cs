using BaseLib.Abstracts;
using BaseLib.Utils;
using KeineMod.KeineModCode.Cards.Ancient;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Basic;

public class ManipulateHistory : KeineModCard, ITranscendenceCard
{
    public ManipulateHistory() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        WithBlock(7, 2);
        WithCards(1);
        WithKeywords(KeineKeywords.Human, KeineKeywords.Consume, KeineKeywords.Hakutaku, KeineKeywords.Recall, KeineKeywords.Create);
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<ScrollOfValor>(card.IsUpgraded)));
        WithTip(typeof(HeavenlySword));
    }

    public CardModel GetTranscendenceTransformedCard()
    {
        return ModelDb.Card<ReshapeHistory>();
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        if (InHuman())
        {
            if (IsUpgraded)
                await ConsumeCmd.FromHandUpTo(choiceContext, Owner, DynamicVars.Cards.IntValue, this);
            else
                await ConsumeCmd.FromHand(choiceContext, Owner, DynamicVars.Cards.IntValue, this);
        }

        if (InHakutaku())
        {
            CardModel created = CombatState.CreateCard<ScrollOfValor>(Owner);
            await CreateCmd.Execute(choiceContext, created, Owner, IsUpgraded);
        }
    }
}