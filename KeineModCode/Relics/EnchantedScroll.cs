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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace KeineMod.KeineModCode.Relics;

public class EnchantedScroll : KeineModRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<TimeShiftPower>(12), new("SacredPower", 1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<TimeShiftPower>(),
        HoverTipFactory.FromKeyword(KeineKeywords.Create),
        HoverTipFactory.FromKeyword(KeineKeywords.Sacredpower),
        HoverTipFactory.FromCard<ScrollOfValor>(true),
        HoverTipFactory.FromCard<ScrollOfBenevolence>(true),
        HoverTipFactory.FromCard<ScrollOfWisdom>(true)
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
        if (player != Owner || Owner.PlayerCombatState.TurnNumber != 1)
            return;
        List<CardModel> treasures =
        [
            Owner.Creature.CombatState.CreateCard<ScrollOfValor>(Owner),
            Owner.Creature.CombatState.CreateCard<ScrollOfBenevolence>(Owner),
            Owner.Creature.CombatState.CreateCard<ScrollOfWisdom>(Owner)
        ];
        foreach (var treasure in treasures)
            CardCmd.Upgrade(treasure);
        var created = await CardSelectCmd.FromChooseACardScreen(choiceContext, treasures, Owner);
        await CreateCmd.Execute(created, Owner);
        switch (created)
        {
            case ScrollOfValor:
                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars["SacredPower"].BaseValue, Owner.Creature, null);
                break;
            case ScrollOfBenevolence:
                await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, DynamicVars["SacredPower"].BaseValue, Owner.Creature, null);
                break;
            case ScrollOfWisdom:
                await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, DynamicVars["SacredPower"].BaseValue, Owner.Creature, null);
                break;
        }
    }
}