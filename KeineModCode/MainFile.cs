using System.Reflection;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using KeineMod.KeineModCode.Cards;
using KeineMod.KeineModCode.Core;
using KeineMod.KeineModCode.Extensions;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using KeineMod.KeineModCode.Stances;
using KeineMod.KeineModCode.UIs;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace KeineMod.KeineModCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "KeineMod"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();
        ScriptManagerBridge.LookupScriptsInAssembly(assembly);
        Log.Info("[KeineMod] Init called");
        Harmony harmony = new(ModId);
        harmony.PatchAll();
        Log.Info("[KeineMod] Harmony PatchAll completed");
        KeineSubscriber.Subscribe();
        Log.Info("[KeineMod] Combat Hooks subscribed");
    }

    [HarmonyPatch(typeof(CombatManager), nameof(CombatManager.SetUpCombat))]
    public static class KeineConstantsResetPatch
    {
        [HarmonyPostfix]
        private static void KeineConstantsReset()
        {
            var tracker = CombatManager.Instance.StateTracker;
            KeineConstantsStateRegistry.Clear(tracker);
        }
    }

    [HarmonyPatch(typeof(NCombatUi), "Activate")]
    public static class UiInjectionPatch
    {
        [HarmonyPostfix]
        public static void InjectScrollPile(NCombatUi __instance, CombatState state)
        {
            if (__instance.GetNodeOrNull<Control>("ScrollPileRoot") != null)
                return;

            var scene = PreloadManager.Cache.GetScene("scroll_pile/scroll_pile.tscn".ScenePath());

            if (scene == null)
                return;

            var control = scene.Instantiate<Control>();

            control.Name = "ScrollPileRoot";
            control.TopLevel = false;
            control.ZIndex = 0;

            __instance.EnergyCounterContainer.AddChild(control);

            control.Position = new Vector2(-80f, -40f);

            var controller =
                control.GetNode<NScrollPileController>("NScrollPileController");

            var me = LocalContext.GetMe(state);

            if (me != null)
                controller.Initialize(me);

            Logger.Info("[KeineMod] Loaded scroll pile UI");
        }

        [HarmonyPostfix]
        public static void InjectFullMoon(NCombatUi __instance, CombatState state)
        {
            if (__instance.GetNodeOrNull<Control>("FullMoonRoot") != null)
                return;

            var scene = PreloadManager.Cache.GetScene("full_moon/full_moon_ui.tscn".ScenePath());

            if (scene == null)
                return;

            var control = scene.Instantiate<Control>();

            control.Name = "FullMoonRoot";
            control.TopLevel = false;
            control.ZIndex = 0;

            __instance.EnergyCounterContainer.AddChild(control);

            control.Position = new Vector2(125f, -40f);

            var controller =
                control.GetNode<NFullMoonController>("NFullMoonController");

            var me = LocalContext.GetMe(state);

            if (me != null)
                controller.Initialize(me);

            Logger.Info("[KeineMod] Loaded full moon UI");
        }
    }

    [HarmonyPatch(typeof(PowerModel), nameof(PowerModel.ShouldRemoveDueToAmount))]
    public static class TimeShiftPreventRemovalPatch
    {
        public static bool Prefix(PowerModel __instance, ref bool __result)
        {
            if (__instance is TimeShiftPower)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(NHandCardHolder), nameof(NHandCardHolder.UpdateCard))]
    public static class CardGlowPatch
    {
        // Tracks active UI cardholders without preventing them from being garbage collected
        private static readonly List<WeakReference<NHandCardHolder>> _trackedHolders = [];

        [HarmonyPostfix]
        private static void RenderGlow(NHandCardHolder __instance)
        {
            // 1. Track this instance whenever the game updates it naturally
            TrackInstance(__instance);

            // 2. Run your existing glow logic
            var card = __instance.CardNode?.Model;
            if (card is not KeineModCard keineCard || card.Owner.PlayerCombatState == null || !keineCard.CanPlay()) return;

            var highlight = __instance.CardNode.CardHighlight;

            var hasHakutaku = keineCard.Keywords.Contains(KeineModKeywords.Hakutaku);
            var hasHuman = keineCard.Keywords.Contains(KeineModKeywords.Human);
            var hasDualForm = keineCard.Owner.Creature.HasPower<DualFormPower>();
            var isHakutakuForm = KeineModel.IsInStance<HakutakuForm>(keineCard.Owner);

            if (!hasHakutaku) return;
            if (!hasDualForm)
            {
                if (isHakutakuForm)
                    highlight.Modulate = new Color(0.35f, 1.0f, 0.5f, 0.98f);
            }
            else
            {
                highlight.Modulate = !hasHuman ? new Color(0.35f, 1.0f, 0.5f, 0.98f) : new Color(1.0f, 1.0f, 1.0f, 0.98f);
            }
        }

        private static void TrackInstance(NHandCardHolder instance)
        {
            // Clean up dead/disposed Godot instances to prevent leaks
            _trackedHolders.RemoveAll(w => !w.TryGetTarget(out var target) || !IsInstanceValid(target));

            // Add if it's a new instance
            if (!_trackedHolders.Any(w => w.TryGetTarget(out var target) && target == instance)) _trackedHolders.Add(new WeakReference<NHandCardHolder>(instance));
        }

        /// <summary>
        /// Forces all currently active hand cards to re-evaluate their glow colors immediately.
        /// </summary>
        public static void RefreshAllVisuals()
        {
            _trackedHolders.RemoveAll(w => !w.TryGetTarget(out var target) || !IsInstanceValid(target));

            foreach (var weak in _trackedHolders)
                if (weak.TryGetTarget(out var holder) && IsInstanceValid(holder.CardNode))
                {
                    // Force the underlying NCard node to re-evaluate formulas and redraw
                    // Note: If 'Normal' isn't the exact name of the default enum value in your assembly,
                    // change it to whatever your CardPreviewMode enum uses (e.g., Default, None, Standard)
                    holder.CardNode.UpdateVisuals(PileType.Hand, CardPreviewMode.Normal);
                    RenderGlow(holder);
                }
        }
    }

    [HarmonyPatch(typeof(CardModel), "GetDescriptionForPile")]
    [HarmonyPatch([typeof(PileType), typeof(CardModel.DescriptionPreviewType), typeof(Creature)])]
    public static class HideSacredScrollKeywordPatch
    {
        private static string GetCustomCardText(CardKeyword keyword)
        {
            // 1. Match the JSON casing
            var key = keyword.ToString().ToUpperInvariant();

            // 2. Fetch the strings
            var title = new LocString("card_keywords", key + ".title").GetFormattedText();
            var period = new LocString("card_keywords", "PERIOD").GetRawText();

            // 3. Return exactly what CardKeywordExtensions.GetCardText() produces
            return $"[gold]{title}[/gold]{period}";
        }

        [HarmonyPostfix]
        private static void HideSacredScrollKeyword(CardModel __instance, ref string __result)
        {
            if (!__instance.Keywords.Contains(KeineModKeywords.Sacredscroll)) return;
            try
            {
                var unplayableSearch = GetCustomCardText(CardKeyword.Unplayable);

                if (string.IsNullOrEmpty(__result)) return;

                // Remove the keyword text and the newline the engine joined with
                __result = __result.Replace(unplayableSearch + "\n", "");
                __result = __result.Replace(unplayableSearch, "");

                __result = __result.Trim('\n', '\r', ' ');
            }
            catch (Exception)
            {
                // Safety first
            }
        }
    }
}