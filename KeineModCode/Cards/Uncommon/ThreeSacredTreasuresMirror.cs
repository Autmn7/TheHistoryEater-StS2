using BaseLib.Utils;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class ThreeSacredTreasuresMirror : KeineModCard
{
    public ThreeSacredTreasuresMirror() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<KnowledgePower>(1);
        WithPower<WisdomPower>(1);
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<ScrollOfWisdom>(card.IsUpgraded)));
        WithTip(typeof(EightSpanMirror));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<WisdomPower>(choiceContext, Owner.Creature, DynamicVars["WisdomPower"].BaseValue, Owner.Creature, this);
        CardModel created = CombatState.CreateCard<ScrollOfWisdom>(Owner);
        await CreateCmd.Execute(created, Owner, IsUpgraded);
    }
}