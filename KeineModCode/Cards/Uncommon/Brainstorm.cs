using BaseLib.Utils;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class Brainstorm : KeineModCard
{
    public Brainstorm() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyPlayer)
    {
        WithKeywords(KeineKeywords.Create, CardKeyword.Exhaust);
        WithTip(StaticHoverTip.Block);
        WithTip(KeineKeywords.Knowledgeable);
        WithTip(new TooltipSource(card => new HoverTip(new LocString("cards", Id.Entry + ".extraTipTitle"), new LocString("cards", Id.Entry + ".extraTipDescription"))));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> cardPool;
        // Get all character pools and exclude the player's own pool
        if (cardPlay.Target?.Player != null && cardPlay.Target != Owner.Creature)
        {
            cardPool = cardPlay.Target.Player.Character.CardPool.AllCards.ToList().Where(c => c.Rarity is CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare);
        }
        else
        {
            var otherPools = Owner.UnlockState.CharacterCardPools.ToList();
            if (otherPools.Count > 1) otherPools.Remove(Owner.Character.CardPool);
            cardPool = otherPools.SelectMany(pool => pool.AllCards);
        }

        // Filter out valid Attack choices
        var attackPool = cardPool.Where(c =>
            c.Type == CardType.Attack &&
            (!c.Tags.Contains(CardTag.OstyAttack) || Owner.IsOstyAlive) &&
            c.BaseStarCost <= Owner.PlayerCombatState?.Stars
        );

        // Filter out valid Block Skill choices using your exact evaluation rule
        var blockPool = cardPool.Where(c =>
            c.Type == CardType.Skill &&
            c.DynamicVars.ContainsKey("Block") &&
            c.DynamicVars["Block"].BaseValue > 1 &&
            c.BaseStarCost <= Owner.PlayerCombatState?.Stars
        );

        // Roll 1 random distinct card from each pool based on combat RNG seed
        var generatedAttack = CardFactory.GetDistinctForCombat(Owner, attackPool, 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
        var generatedBlock = CardFactory.GetDistinctForCombat(Owner, blockPool, 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();

        // Push both cards into the player's hand sequentially
        if (generatedAttack != null) await CreateCmd.Execute(choiceContext, generatedAttack, Owner, IsUpgraded, PileType.Hand, true);
        if (generatedBlock != null) await CreateCmd.Execute(choiceContext, generatedBlock, Owner, IsUpgraded, PileType.Hand, true);
    }
}