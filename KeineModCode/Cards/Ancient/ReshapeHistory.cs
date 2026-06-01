using BaseLib.Utils;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Ancient;

public class ReshapeHistory : KeineModCard
{
    public ReshapeHistory() : base(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
    {
        WithBlock(12, 4);
        WithCards(2, 1);
        WithKeywords(CardKeyword.Innate, KeineModKeywords.Human, KeineModKeywords.Consume, KeineModKeywords.Hakutaku, KeineModKeywords.Create);
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<HeavenlySword>(card.IsUpgraded)));
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<CurvedJewel>(card.IsUpgraded)));
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<EightSpanMirror>(card.IsUpgraded)));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        if (InHuman())
            await ConsumeCmd.FromHandUpTo(choiceContext, Owner, DynamicVars.Cards.IntValue, this);
        if (InHakutaku())
        {
            List<CardModel> treasures =
            [
                CombatState.CreateCard<HeavenlySword>(Owner),
                CombatState.CreateCard<CurvedJewel>(Owner),
                CombatState.CreateCard<EightSpanMirror>(Owner)
            ];
            if (IsUpgraded)
                foreach (var treasure in treasures)
                    CardCmd.Upgrade(treasure);
            var created = await CardSelectCmd.FromChooseACardScreen(choiceContext, treasures, Owner, true);
            await CreateCmd.Execute(created, Owner);
        }
    }
}