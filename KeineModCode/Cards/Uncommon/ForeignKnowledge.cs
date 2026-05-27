using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class ForeignKnowledge : KeineModCard
{
    public ForeignKnowledge() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(KeineModKeywords.Create);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
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
            (!c.Tags.Contains(CardTag.OstyAttack) || Owner.IsOstyAlive)
        );

        // Roll 3 distinct random Attack cards based on combat RNG seed
        var choices = CardFactory.GetDistinctForCombat(Owner, attackPool, 3, Owner.RunState.Rng.CombatCardGeneration).ToList();

        // Open the selection screen and wait for the player to choose a card
        var chosenCard = await CardSelectCmd.FromChooseACardScreen(choiceContext, choices, Owner);

        // Create the chosen card into the player's hand with the Knowledgeable keyword applied
        if (chosenCard != null) await CreateCmd.Execute(chosenCard, Owner, false, PileType.Hand, true);
    }
}