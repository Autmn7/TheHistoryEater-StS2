using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class ScrollDancePower : KeineModPower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner.Player)
            return;
        for (var i = 0; i < Amount; ++i)
        {
            List<CardModel> treasures =
            [
                CombatState.CreateCard<ScrollOfValor>(Owner.Player),
                CombatState.CreateCard<ScrollOfBenevolence>(Owner.Player),
                CombatState.CreateCard<ScrollOfWisdom>(Owner.Player)
            ];
            var created = await CardSelectCmd.FromChooseACardScreen(choiceContext, treasures, Owner.Player, true);
            await CreateCmd.Execute(created, Owner.Player);
        }
    }
}