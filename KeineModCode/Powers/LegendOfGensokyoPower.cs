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

            // Flatten all available card options from the other characters
            var allOtherCards = otherPools.SelectMany(pool => pool.AllCards);

            // Roll 3 distinct random Attack cards based on combat RNG seed
            var choices = CardFactory.GetDistinctForCombat(Owner.Player, allOtherCards, 3, Owner.Player.RunState.Rng.CombatCardGeneration).ToList();

            // Open the selection screen and wait for the player to choose a card
            var chosenCard = await CardSelectCmd.FromChooseACardScreen(choiceContext, choices, Owner.Player, true);

            // Create the chosen card into the player's hand with the Knowledgeable keyword applied
            if (chosenCard != null) await CreateCmd.Execute(chosenCard, Owner.Player, false, PileType.Hand, true);
        }
    }
}