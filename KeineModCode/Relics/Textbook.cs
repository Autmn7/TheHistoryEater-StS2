using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

namespace KeineMod.KeineModCode.Relics;

public class Textbook : KeineModRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<KnowledgePower>(1), new PowerVar<WisdomPower>(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<KnowledgePower>(),
        HoverTipFactory.FromPower<WisdomPower>()
    ];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
            return;
        Flash();
        await PowerCmd.Apply<KnowledgePower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, null);
        await PowerCmd.Apply<WisdomPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["WisdomPower"].BaseValue, Owner.Creature, null);
    }
}