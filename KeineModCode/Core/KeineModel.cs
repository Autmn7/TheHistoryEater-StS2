using BaseLib.Abstracts;
using BaseLib.Utils;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace KeineMod.KeineModCode.Core;

public class KeineModel : CustomSingletonModel
{
    private static readonly SpireField<Player, KeineStanceModel> ActiveStance = new((Func<KeineStanceModel>)KeineModelDb.KeineStance<HumanForm>);

    public override bool ShouldReceiveCombatHooks => true;

    public KeineModel()
        : base(true, false)
    {
    }

    public static KeineStanceModel GetStanceModel(Player player)
    {
        return ActiveStance[player] ?? KeineModelDb.KeineStance<HumanForm>();
    }

    public static bool IsInStance<T>(Player player) where T : KeineStanceModel
    {
        return ActiveStance[player] is T;
    }

    public static async Task SetStance<T>(PlayerChoiceContext ctx, Player player, CardModel? source) where T : KeineStanceModel
    {
        await SetStance(ctx, player, KeineModelDb.KeineStance<T>(), source);
    }

    private static async Task SetStance(PlayerChoiceContext ctx, Player player, KeineStanceModel newCanonical, CardModel? source)
    {
        var current = ActiveStance[player];
        if (((object)current)?.GetType() == ((object)newCanonical).GetType()) return;
        if (current != null) await current.OnExitStance(ctx, player, source);
        var mutable = newCanonical.ToMutable(player);
        ActiveStance[player] = mutable;
        await mutable.OnEnterStance(ctx, player, source);
        var instance = NCombatRoom.Instance;
        var creatureNode = instance != null ? instance.GetCreatureNode(player.Creature) : null;
        // WatcherNCreatureVisuals visuals = ((creatureNode != null) ? creatureNode.Visuals : null) as WatcherNCreatureVisuals;
        // WatcherNCreatureVisuals watcherNCreatureVisuals = visuals;
        // if (watcherNCreatureVisuals != null)
        // {
        // 	if (1 == 0)
        // 	{
        // 	}
        // 	string eyeStance = ((mutable is WrathStance) ? "wrath" : ((mutable is CalmStance) ? "calm" : ((!(mutable is DivinityStance)) ? "RESET" : "divinity")));
        // 	if (1 == 0)
        // 	{
        // 	}
        // 	watcherNCreatureVisuals.SetEyeStance(eyeStance);
        // }
        // await WatcherHook.OnStanceChange(ctx, player, current, ActiveStance[player]);
    }

    public override Task BeforeCombatStart()
    {
        var val = CombatManager.Instance.DebugOnlyGetState();
        if (val == null) return Task.CompletedTask;
        foreach (var player in val.Players) ActiveStance[player] = KeineModelDb.KeineStance<HumanForm>();
        return Task.CompletedTask;
    }
}