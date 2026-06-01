using Godot;
using KeineMod.KeineModCode.Core;
using KeineMod.KeineModCode.Scripts;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;

namespace KeineMod.KeineModCode.UIs;

public class UseFullMoonAction : GameAction
{
    private readonly Player _player;

    public UseFullMoonAction(Player player)
    {
        _player = player;
    }

    public override ulong OwnerId => _player.NetId;

    public override GameActionType ActionType => GameActionType.CombatPlayPhaseOnly;

    public override INetAction ToNetAction()
    {
        return new UseFullMoonNetAction();
    }

    protected override Task ExecuteAction()
    {
        if (_player == null) return Task.CompletedTask;
        var fullMoonUi = KeineConstantsStateRegistry.Get(_player);
        if (!fullMoonUi.CanUse(_player)) return Task.CompletedTask;

        fullMoonUi.ClickedThisTurn = true;
        fullMoonUi.LoseFullMoon(1);

        if (LocalContext.NetId.HasValue)
        {
            // 1. Create the hook choice context
            var realContext = new HookPlayerChoiceContext(_player, LocalContext.NetId.Value, GameActionType.Combat);

            // 2. Prepare the stance task (which triggers OnEnterStance -> RecallCmd internally)
            var stanceTask = KeineModel.SetStance<HakutakuForm>(realContext, _player, null);

            // 🌟 Chain the visual updates to run precisely after the stance action evaluates
            var totalTask = stanceTask.ContinueWith(t =>
            {
                // Defer to Godot's main thread to prevent asynchronous layout crashes
                Callable.From(MainFile.CardGlowPatch.RefreshAllVisuals).CallDeferred();
            });

            // 3. Bind the task to the context using the existing assembly method.
            // CRITICAL: Do NOT await this call or stanceTask here. 
            // Allowing this method to finish instantly frees the ActionExecutor pipeline 
            // so the screen selection action can execute next.
            _ = realContext.AssignTaskAndWaitForPauseOrCompletion(totalTask);
        }
        else
        {
            Log.Error("Cannot enter HakutakuForm: LocalContext.NetId is null.");
        }

        return Task.CompletedTask;
    }
}