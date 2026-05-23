using System.Reflection;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using KeineMod.KeineModCode.Cards;
using KeineMod.KeineModCode.Core;
using KeineMod.KeineModCode.Extensions;
using KeineMod.KeineModCode.Piles;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
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
    }

    [HarmonyPatch(typeof(NCombatUi), "Activate")]
    public static class ScrollPileInjectionPatch
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

            control.Position = new Vector2(140f, -40f);

            var controller =
                control.GetNode<NScrollPileController>("NScrollPileController");

            var me = LocalContext.GetMe(state);

            if (me != null)
                controller.Initialize(me);

            Logger.Info("[KeineMod] Loaded scroll pile UI");
        }
    }

    [HarmonyPatch(typeof(NHandCardHolder), nameof(NHandCardHolder.UpdateCard))]
    public static class CardGlowPatch
    {
        [HarmonyPostfix]
        private static void RenderGlow(NHandCardHolder __instance)
        {
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
    }
}