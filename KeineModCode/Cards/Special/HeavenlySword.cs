using BaseLib.Utils;
using Godot;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class HeavenlySword : KeineModCard
{
    public HeavenlySword() : base(2, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
        WithCalculatedDamage(5, (card, _) => card.Owner.Creature.GetPowerAmount<ValorPower>());
        WithCalculatedVar("Repeat", 2, (card, _) => card.Owner.Creature.HasPower<TreasureOfValorPower>() ? 1 : 0);
        WithKeywords(CardKeyword.Exhaust);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this).Targeting(cardPlay.Target).WithHitCount((int)((CalculatedVar)DynamicVars["Repeat"]).Calculate(cardPlay.Target)).WithHitVfxNode((Func<Creature, Node2D>)(t => (Node2D)NBigSlashVfx.Create(t))).WithHitVfxNode((Func<Creature, Node2D>)(t => (Node2D)NBigSlashImpactVfx.Create(t))).WithAttackerFx(sfx: "event:/sfx/characters/regent/regent_sovereign_blade").Execute(choiceContext);
    }
}