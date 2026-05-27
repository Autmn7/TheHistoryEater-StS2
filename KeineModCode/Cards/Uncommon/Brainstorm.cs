using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class Brainstorm : KeineModCard
{
    public Brainstorm() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeywords(KeineModKeywords.Create, CardKeyword.Exhaust);
        WithTip(StaticHoverTip.Block);
        WithTip(KeineModKeywords.Knowledgeable);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Get all character pools and exclude the player's own pool
        var otherPools = Owner.UnlockState.CharacterCardPools.ToList();
        if (otherPools.Count > 1) otherPools.Remove(Owner.Character.CardPool);

        // Flatten all available card options from the other characters
        var allOtherCards = otherPools.SelectMany(pool => pool.AllCards);

        // Filter out valid Attack choices
        var attackPool = allOtherCards.Where(c =>
            c.Type == CardType.Attack &&
            (!c.Tags.Contains(CardTag.OstyAttack) || Owner.IsOstyAlive) &&
            c.BaseStarCost <= Owner.PlayerCombatState?.Stars
        );

        // Filter out valid Block Skill choices using your exact evaluation rule
        var blockPool = allOtherCards.Where(c =>
            c.Type == CardType.Skill &&
            c.DynamicVars.ContainsKey("Block") &&
            c.DynamicVars["Block"].BaseValue > 1 &&
            c.BaseStarCost <= Owner.PlayerCombatState?.Stars
        );

        // Roll 1 random distinct card from each pool based on combat RNG seed
        var generatedAttack = CardFactory.GetDistinctForCombat(Owner, attackPool, 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
        var generatedBlock = CardFactory.GetDistinctForCombat(Owner, blockPool, 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();

        // Push both cards into the player's hand sequentially
        if (generatedAttack != null) await CreateCmd.Execute(generatedAttack, Owner, IsUpgraded, PileType.Hand, true);
        if (generatedBlock != null) await CreateCmd.Execute(generatedBlock, Owner, IsUpgraded, PileType.Hand, true);
    }
}