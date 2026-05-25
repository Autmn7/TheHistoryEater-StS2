using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Relics;

public class OldScroll : KeineModRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new PowerVar<TimeShiftPower>(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(KeineModKeywords.Create),
        HoverTipFactory.FromPower<TimeShiftPower>(),
        HoverTipFactory.FromCard<ScrollOfValor>()
    ];

    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player != Owner || combatState.RoundNumber != 1)
            return;
        Flash();
        CardModel created = Owner.Creature.CombatState.CreateCard<ScrollOfValor>(Owner);
        await CreateCmd.Execute(created, Owner);
    }

    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner)
            return;
        await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner.Creature, DynamicVars["TimeShiftPower"].BaseValue, Owner.Creature, null);
    }
}