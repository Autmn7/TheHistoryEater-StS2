using BaseLib.Abstracts;
using BaseLib.Utils;
using Godot;
using KeineMod.KeineModCode.Scripts;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
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

    public static async Task SetStance<T>(PlayerChoiceContext choiceContext, Player player, CardModel? source) where T : KeineStanceModel
    {
        await SetStance(choiceContext, player, KeineModelDb.KeineStance<T>(), source);
    }

    private static async Task SetStance(PlayerChoiceContext choiceContext, Player player, KeineStanceModel newCanonical, CardModel? source)
    {
        var current = ActiveStance[player];
        if (((object)current)?.GetType() == ((object)newCanonical).GetType()) return;
        if (current != null) await current.OnExitStance(choiceContext, player, source);
        var mutable = newCanonical.ToMutable(player);
        ActiveStance[player] = mutable;
        await mutable.OnEnterStance(choiceContext, player, source);
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
        await KeineHooks.OnStanceChange(choiceContext, player, current, ActiveStance[player]);
    }

    public override Task BeforeCombatStart()
    {
        var val = CombatManager.Instance.DebugOnlyGetState();
        if (val == null) return Task.CompletedTask;
        foreach (var player in val.Players)
            if (player.Character is Character.KeineMod)
                ActiveStance[player] = KeineModelDb.KeineStance<HumanForm>();
        return Task.CompletedTask;
    }

    public override Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != CombatSide.Player)
            return Task.CompletedTask;
        foreach (var player in combatState.Players) KeineConstantsStateRegistry.Get(player).ClickedThisTurn = false;
        return Task.CompletedTask;
    }

    public override Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (!wasRemovalPrevented && creature.IsPlayer)
        {
            var player = creature.Player;
            if (player?.Character is Character.KeineMod)
            {
                var instance = NCombatRoom.Instance;
                var creatureNode = instance?.GetCreatureNode(creature);
                var targetVisuals = creatureNode?.Visuals;

                if (targetVisuals != null && GodotObject.IsInstanceValid(targetVisuals))
                {
                    var hakuForm = targetVisuals.GetNodeOrNull<CanvasItem>("HakutakuFormNode");
                    var isHakutaku = hakuForm != null && hakuForm.Visible;

                    if (isHakutaku)
                    {
                        var hakuSpine = hakuForm.GetNodeOrNull<CanvasItem>("Visuals");
                        var hakuCorpse = hakuForm.GetNodeOrNull<CanvasItem>("Death");

                        if (hakuSpine != null) hakuSpine.Visible = false;
                        if (hakuCorpse != null) hakuCorpse.Visible = true;
                    }
                    else
                    {
                        var humanSpine = targetVisuals.GetNodeOrNull<CanvasItem>("Visuals");
                        var humanCorpse = targetVisuals.GetNodeOrNull<CanvasItem>("Death");

                        if (humanSpine != null) humanSpine.Visible = false;
                        if (humanCorpse != null) humanCorpse.Visible = true;
                    }
                }
            }
        }

        return base.AfterDeath(choiceContext, creature, wasRemovalPrevented, deathAnimLength);
    }
}