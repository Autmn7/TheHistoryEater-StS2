using BaseLib.Utils;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class ThreeSacredTreasuresMirror : KeineModCard
{
    public ThreeSacredTreasuresMirror() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithEnergy(1);
        WithKeyword(KeineModKeywords.Create);
        WithCostUpgradeBy(-1);
        WithTip(typeof(EightSpanMirror));
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<ScrollOfWisdom>(card.IsUpgraded)));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<TreasureOfWisdomPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature, this);
        CardModel created = CombatState.CreateCard<ScrollOfWisdom>(Owner);
        await CreateCmd.Execute(created, Owner, IsUpgraded);
    }
}