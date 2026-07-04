using Godot;
using Godot.Collections;
using KeineMod.KeineModCode.Extensions;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace KeineMod.KeineModCode.Scripts;

public partial class HakuBackgroundOverlay : TextureRect
{
    public override void _Process(double delta)
    {
        if (!Visible) return;

        var parent = GetParent() as CanvasItem;
        if (parent == null || !IsInstanceValid(parent)) return;

        var invTransform = parent.GetGlobalTransform().AffineInverse();
        var viewportSize = GetViewportRect().Size;

        var localTopLeft = invTransform * Vector2.Zero;
        var localBottomRight = invTransform * viewportSize;

        Position = localTopLeft;
        Size = localBottomRight - localTopLeft;
    }
}

/// <summary>
/// Monitors and mirrors Spine animations from human form to Hakutaku form.
/// </summary>
public partial class SpineAnimationSyncer : Node
{
    private GodotObject? _humanVisuals;
    private GodotObject? _hakuVisuals;

    public void Setup(GodotObject humanVisuals, GodotObject hakuVisuals)
    {
        _humanVisuals = humanVisuals;
        _hakuVisuals = hakuVisuals;
    }

    public override void _Process(double delta)
    {
        var parent = GetParent() as CanvasItem;
        if (parent == null || !parent.Visible) return;

        if (!IsInstanceValid(_humanVisuals) || !IsInstanceValid(_hakuVisuals)) return;

        // 1. Grab current animation name from human form
        var humanAnimState = _humanVisuals.Call("get_animation_state").AsGodotObject();
        var humanTrack = humanAnimState.Call("get_current", 0).AsGodotObject();
        var humanAnim = humanTrack.Call("get_animation").AsGodotObject();
        var targetAnimName = humanAnim.Call("get_name").AsString();

        // 2. Resolve if it should loop (idle_loop vs hurt)
        var isLooping = true;
        var loopVal = humanTrack.Get("loop");

        if (loopVal.VariantType != Variant.Type.Nil)
            isLooping = loopVal.AsBool();
        else
            try
            {
                isLooping = humanTrack.Call("get_loop").AsBool();
            }
            catch
            {
                // Matches "idle_loop" perfectly based on your setup
                isLooping = targetAnimName.Contains("loop", StringComparison.OrdinalIgnoreCase) ||
                            targetAnimName.Contains("idle", StringComparison.OrdinalIgnoreCase);
            }

        // 3. Inspect Hakutaku's current active state
        var hakuAnimState = _hakuVisuals.Call("get_animation_state").AsGodotObject();
        var hakuTrack = hakuAnimState.Call("get_current", 0).AsGodotObject();
        var hakuAnim = hakuTrack.Call("get_animation").AsGodotObject();
        var hakuAnimName = hakuAnim.Call("get_name").AsString();

        // 4. Force synchronization if state is mismatched
        if (hakuAnimName != targetAnimName) hakuAnimState.Call("set_animation", targetAnimName, isLooping, 0);
    }
}

public class KeineHooks
{
    private static HakuBackgroundOverlay? HakuBgOverlay = null;

    private static T? FindChildOfType<T>(Node root) where T : Node
    {
        if (root is T target) return target;

        foreach (var child in root.GetChildren())
        {
            var result = FindChildOfType<T>(child);
            if (result != null) return result;
        }

        return null;
    }

    private static async Task Dispatch<T>(PlayerChoiceContext choiceContext, Player player, Func<T, Task> invoke) where T : class
    {
        var combatState = player.Creature.CombatState;
        if (combatState == null) return;
        foreach (var model in combatState.IterateHookListeners().OfType<T>())
        {
            var abstractModel = model as AbstractModel;
            choiceContext.PushModel(abstractModel);
            await invoke(model);
            choiceContext.PopModel(abstractModel);
        }
    }

    public static Task OnStanceChange(PlayerChoiceContext choiceContext, Player player, KeineStanceModel oldStance, KeineStanceModel newStance)
    {
        var combatRoom = NCombatRoom.Instance;
        var isHakutaku = newStance is HakutakuForm;
        if (combatRoom == null) return Task.CompletedTask;

        if (player.Character is Character.KeineMod)
        {
            // 1. Grab the correct visual node directly using the engine's room lookup maps
            var creatureNode = combatRoom.GetCreatureNode(player.Creature);
            var targetVisuals = creatureNode?.Visuals;

            if (targetVisuals != null && GodotObject.IsInstanceValid(targetVisuals))
            {
                var hakuForm = targetVisuals.GetNodeOrNull<CanvasItem>("HakutakuFormNode");

                if (hakuForm != null)
                {
                    if (isHakutaku)
                    {
                        hakuForm.Visible = true;
                        var hiddenNodes = new List<CanvasItem>();

                        // Hide the human parts and preserve them directly in this instance's metadata
                        foreach (var child in targetVisuals.GetChildren())
                        {
                            if (child.Name == "HakutakuFormNode") continue;
                            if (child is CanvasItem humanPart && humanPart.Visible)
                            {
                                humanPart.Visible = false;
                                hiddenNodes.Add(humanPart);
                            }
                        }

                        targetVisuals.SetMeta("DynamicHiddenHumanNodes", new Array<CanvasItem>(hiddenNodes));

                        // Instantiate or verify our real-time synchronization tracker
                        if (!hakuForm.HasNode("SpineAnimationSyncer"))
                        {
                            var humanSpine = targetVisuals.GetNodeOrNull("Visuals");
                            var hakuSpine = hakuForm.GetNodeOrNull("Visuals");

                            if (humanSpine != null && hakuSpine != null)
                            {
                                var syncer = new SpineAnimationSyncer { Name = "SpineAnimationSyncer" };
                                syncer.Setup(humanSpine, hakuSpine);
                                hakuForm.AddChild(syncer);
                            }
                        }
                    }
                    else
                    {
                        hakuForm.Visible = false;

                        // Restore human parts safely from this instance's metadata
                        if (targetVisuals.HasMeta("DynamicHiddenHumanNodes"))
                        {
                            var hiddenNodes = targetVisuals.GetMeta("DynamicHiddenHumanNodes").AsGodotArray<CanvasItem>();
                            foreach (var humanPart in hiddenNodes)
                                if (GodotObject.IsInstanceValid(humanPart))
                                    humanPart.Visible = true;
                            targetVisuals.RemoveMeta("DynamicHiddenHumanNodes");
                        }
                    }
                }
            }
        }

        // 2. Combat Background Swapping (Only execute visual layout shifts for the local monitor view, applicable to non-Keine characters as well)
        if (LocalContext.IsMe(player) && combatRoom.GetTree()?.CurrentScene != null)
        {
            var currentScene = combatRoom.GetTree().CurrentScene;
            var combatBg = FindChildOfType<NCombatBackground>(currentScene);

            if (combatBg != null && GodotObject.IsInstanceValid(combatBg))
            {
                if (isHakutaku)
                {
                    if (HakuBgOverlay == null || !GodotObject.IsInstanceValid(HakuBgOverlay))
                    {
                        var texturePath = "full_moon/full_moon_background.png".ScenePath();
                        var bgTexture = GD.Load<Texture2D>(texturePath);

                        if (bgTexture == null) Log.Info($"[KeineMod] ERROR: Failed to load background texture at: {texturePath}");

                        HakuBgOverlay = new HakuBackgroundOverlay
                        {
                            Name = "KeineHakutakuBgOverlay",
                            Texture = bgTexture,
                            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                            StretchMode = TextureRect.StretchModeEnum.Scale,
                            Visible = false
                        };

                        combatBg.AddChild(HakuBgOverlay);
                    }

                    // --- UPDATED SMART LAYER SORTING ---
                    var targetIndex = combatBg.GetChildCount() - 1;
                    for (var i = 0; i < combatBg.GetChildCount(); ++i)
                    {
                        var child = combatBg.GetChild(i);
                        if (child == HakuBgOverlay) continue;

                        var childName = child.Name.ToString();

                        // Added "KaiserCrab" check to safely slot the overlay behind the custom boss layers
                        if (childName == "Foreground" || childName.Contains("SpineSprite") || childName.Contains("KaiserCrab"))
                        {
                            targetIndex = child.GetIndex();
                            break;
                        }
                    }

                    // Slip the full moon right behind the discovered foreground or crab boss node
                    combatBg.MoveChild(HakuBgOverlay, targetIndex);

                    // --- FADE IN LOGIC ---
                    if (!HakuBgOverlay.Visible)
                    {
                        HakuBgOverlay.Modulate = new Color(1, 1, 1, 0f);
                        HakuBgOverlay.Visible = true;
                    }

                    var fadeInTween = combatBg.CreateTween();
                    fadeInTween.TweenProperty(HakuBgOverlay, "modulate:a", 1.0f, 0.6f)
                        .SetTrans(Tween.TransitionType.Cubic)
                        .SetEase(Tween.EaseType.Out);
                }
                else
                {
                    // --- FADE OUT LOGIC ---
                    if (HakuBgOverlay != null && GodotObject.IsInstanceValid(HakuBgOverlay) && HakuBgOverlay.Visible)
                    {
                        var fadeOutTween = combatBg.CreateTween();
                        fadeOutTween.TweenProperty(HakuBgOverlay, "modulate:a", 0.0f, 0.5f)
                            .SetTrans(Tween.TransitionType.Cubic)
                            .SetEase(Tween.EaseType.In);

                        fadeOutTween.TweenCallback(Callable.From(() => HakuBgOverlay.Visible = false));
                    }
                }
            }
        }

        return Dispatch(choiceContext, player, (IOnStanceChange m) => m.OnStanceChange(choiceContext, player, oldStance, newStance));
    }

    public static Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        return Dispatch(choiceContext, player, (IOnConsumed m) => m.OnConsumed(choiceContext, player, consumedCard));
    }

    public static Task OnConsumedLate(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        return Dispatch(choiceContext, player, (IOnConsumedLate m) => m.OnConsumedLate(choiceContext, player, consumedCard));
    }

    public static Task OnCreated(PlayerChoiceContext choiceContext, Player player, CardModel createdCard)
    {
        return Dispatch(choiceContext, player, (IOnCreated m) => m.OnCreated(choiceContext, player, createdCard));
    }
}