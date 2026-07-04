using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace KeineMod.KeineModCode.Potions;

public class CondensedKnowledge : KeineModPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.AnyPlayer;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<KnowledgePower>(3), new CardsVar(1)];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<KnowledgePower>(), HoverTipFactory.FromKeyword(KeineKeywords.Create), HoverTipFactory.FromKeyword(KeineKeywords.Knowledgeable)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        if (target?.Player == null)
            return;

        await PowerCmd.Apply<KnowledgePower>(choiceContext, target, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, null);

        var otherPools = target.Player.UnlockState.CharacterCardPools.ToList();
        if (otherPools.Count > 1) otherPools.Remove(target.Player.Character.CardPool);
        var cardPool = otherPools.SelectMany(pool => pool.AllCards).ToList();

        var attackPool = cardPool.Where(c =>
            c.Type == CardType.Attack &&
            (!c.Tags.Contains(CardTag.OstyAttack) || target.Player.IsOstyAlive) &&
            c.BaseStarCost <= target.Player.PlayerCombatState?.Stars
        );

        var blockPool = cardPool.Where(c =>
            c.Type == CardType.Skill &&
            c.DynamicVars.ContainsKey("Block") &&
            c.DynamicVars["Block"].BaseValue > 1 &&
            c.BaseStarCost <= target.Player.PlayerCombatState?.Stars
        );

        var generatedAttacks = CardFactory.GetDistinctForCombat(target.Player, attackPool, DynamicVars.Cards.IntValue, target.Player.RunState.Rng.CombatCardGeneration).ToList();
        var generatedBlocks = CardFactory.GetDistinctForCombat(target.Player, blockPool, DynamicVars.Cards.IntValue, target.Player.RunState.Rng.CombatCardGeneration).ToList();

        foreach (var attack in generatedAttacks)
            await CreateCmd.Execute(choiceContext, attack, target.Player, false, PileType.Hand, true);
        foreach (var skill in generatedBlocks)
            await CreateCmd.Execute(choiceContext, skill, target.Player, false, PileType.Hand, true);
    }
}