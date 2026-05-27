using BaseLib.Utils;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace KeineMod.KeineModCode.Cards.Common;

public class Intertwined : KeineModCard
{
    public Intertwined() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithEnergy(1, 1);
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<ScrollOfBenevolence>(card.IsUpgraded)));
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<ScrollOfValor>(card.IsUpgraded)));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel benevolence = CombatState.CreateCard<ScrollOfBenevolence>(Owner);
        CardModel valor = CombatState.CreateCard<ScrollOfValor>(Owner);
        await CreateCmd.Execute(benevolence, Owner, IsUpgraded);
        await CreateCmd.Execute(valor, Owner, IsUpgraded);
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature, this);
    }
}