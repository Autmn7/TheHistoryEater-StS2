using KeineMod.KeineModCode.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Powers;

public class LegendOfGensokyoPower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner.Player)
            return;
        for (var i = 0; i < Amount; ++i)
        {
            // Get all character pools and exclude the player's own pool
            var otherPools = Owner.Player.UnlockState.CharacterCardPools.ToList();
            if (otherPools.Count > 1) otherPools.Remove(Owner.Player.Character.CardPool);

            // Flatten and filter the pool down strictly to valid Attacks and Block Skills
            var allOtherCards = otherPools.SelectMany(pool => pool.AllCards).Where(c =>
                // Shared Condition: Card must be affordable based on current Stars
                c.BaseStarCost <= Owner.Player.PlayerCombatState?.Stars &&
                (
                    // Pool Option 1: Valid Attacks (including Osty validation)
                    (c.Type == CardType.Attack && (!c.Tags.Contains(CardTag.OstyAttack) || Owner.Player.IsOstyAlive))
                    ||
                    // Pool Option 2: Valid Block Skills (Value must be strictly greater than 1)
                    (c.Type == CardType.Skill && c.DynamicVars.ContainsKey("Block") && c.DynamicVars["Block"].BaseValue > 1)
                )
            );

            // Roll 3 distinct random cards from the combined pool based on combat RNG seed
            var choices = CardFactory.GetDistinctForCombat(Owner.Player, allOtherCards, 3, Owner.Player.RunState.Rng.CombatCardGeneration).ToList();

            // Open the selection screen and wait for the player to choose a card
            var chosenCard = await CardSelectCmd.FromChooseACardScreen(choiceContext, choices, Owner.Player, true);

            // Create the chosen card into the player's hand with the Knowledgeable keyword applied
            if (chosenCard != null) await CreateCmd.Execute(chosenCard, Owner.Player, false, PileType.Hand, true);
        }
    }
}