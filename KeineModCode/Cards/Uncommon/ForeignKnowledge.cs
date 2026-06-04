using BaseLib.Utils;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class ForeignKnowledge : KeineModCard
{
    public ForeignKnowledge() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyPlayer)
    {
        WithKeyword(KeineKeywords.Create);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
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

        // Roll 3 distinct random Attack cards based on combat RNG seed
        var choices = CardFactory.GetDistinctForCombat(Owner, attackPool, 3, Owner.RunState.Rng.CombatCardGeneration).ToList();

        // Open the selection screen and wait for the player to choose a card
        var chosenCard = await CardSelectCmd.FromChooseACardScreen(choiceContext, choices, Owner, true);

        // Create the chosen card into the player's hand with the Knowledgeable keyword applied
        if (chosenCard != null) await CreateCmd.Execute(chosenCard, Owner, false, PileType.Hand, true);
    }
}