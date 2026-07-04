using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Potions;

public class SharpeningOil : KeineModPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ValorPower>(7), new CardsVar(1)];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ValorPower>(), HoverTipFactory.FromKeyword(KeineKeywords.Create), HoverTipFactory.FromCard<HeavenlySword>(true)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        if (target?.Player == null)
            return;

        await PowerCmd.Apply<ValorPower>(choiceContext, target, DynamicVars["ValorPower"].BaseValue, Owner.Creature, null);
        for (var i = 0; i < DynamicVars.Cards.IntValue; ++i)
        {
            CardModel sword = target.CombatState.CreateCard<HeavenlySword>(target.Player);
            await CreateCmd.Execute(choiceContext, sword, target.Player, true);
        }
    }
}