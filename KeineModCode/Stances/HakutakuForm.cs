using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace KeineMod.KeineModCode.Stances;

public sealed class HakutakuForm : KeineStanceModel
{
    public override bool ShouldReceiveCombatHooks => true;


    protected override StanceVfxConfig VfxConfig => new(null, null, null, null, (ShakeStrength)0);

    public override async Task OnEnterStance(PlayerChoiceContext ctx, Player player, CardModel? source)
    {
        await base.OnEnterStance(ctx, player, source);
        var recallAmount = 2;
        if (player.Creature.HasPower<WisdomPower>())
            recallAmount += player.Creature.GetPowerAmount<WisdomPower>();
        await RecallCmd.FromScrollUpTo(ctx, player, recallAmount);
    }
}