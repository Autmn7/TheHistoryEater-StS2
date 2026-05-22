using BaseLib.Utils;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Common;

public class BorrowedTreasures : KeineModCard
{
    public BorrowedTreasures() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithKeywords(KeineModKeywords.Create, CardKeyword.Exhaust);
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<HeavenlySword>(card.IsUpgraded)));
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<CurvedJewel>(card.IsUpgraded)));
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<EightSpanMirror>(card.IsUpgraded)));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
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
        var created = await CardSelectCmd.FromChooseACardScreen(choiceContext, treasures, Owner);
        await CreateCmd.Execute(created, Owner);
    }
}