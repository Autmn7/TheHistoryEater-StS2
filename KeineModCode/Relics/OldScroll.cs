using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace KeineMod.KeineModCode.Relics;

public class OldScroll : KeineModRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<TimeShiftPower>(3)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<TimeShiftPower>(),
        HoverTipFactory.FromKeyword(KeineKeywords.Create),
        HoverTipFactory.FromCard<ScrollOfValor>()
    ];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return;
        Flash();
        await PowerCmd.Apply<TimeShiftPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["TimeShiftPower"].BaseValue, Owner.Creature, null);
    }

    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != Owner || combatState.RoundNumber != 1)
            return;
        CardModel created = Owner.Creature.CombatState.CreateCard<ScrollOfValor>(Owner);
        await CreateCmd.Execute(created, Owner);
    }

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<EnchantedScroll>();
    }
}