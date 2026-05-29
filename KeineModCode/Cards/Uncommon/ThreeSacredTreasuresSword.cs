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

public class ThreeSacredTreasuresSword : KeineModCard
{
    public ThreeSacredTreasuresSword() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(8, 2);
        WithVar("Repeat", 2);
        WithVar("TreasureOfValorPower", 1);
        WithKeyword(KeineModKeywords.Create);
        WithTip(typeof(HeavenlySword));
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<ScrollOfValor>(card.IsUpgraded)));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitCount(DynamicVars["Repeat"].IntValue).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        await PowerCmd.Apply<TreasureOfValorPower>(choiceContext, Owner.Creature, DynamicVars["TreasureOfValorPower"].BaseValue, Owner.Creature, this);
        CardModel created = CombatState.CreateCard<ScrollOfValor>(Owner);
        await CreateCmd.Execute(created, Owner, IsUpgraded);
    }
}