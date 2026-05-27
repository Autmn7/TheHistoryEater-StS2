using BaseLib.Utils;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Common;

public class Intertwined : KeineModCard
{
    public Intertwined() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithBlock(8, 3);
        WithDamage(8, 3);
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<ScrollOfBenevolence>(card.IsUpgraded)));
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<ScrollOfValor>(card.IsUpgraded)));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        CardModel benevolence = CombatState.CreateCard<ScrollOfBenevolence>(Owner);
        CardModel valor = CombatState.CreateCard<ScrollOfValor>(Owner);
        await CreateCmd.Execute(benevolence, Owner, IsUpgraded);
        await CreateCmd.Execute(valor, Owner, IsUpgraded);
    }
}