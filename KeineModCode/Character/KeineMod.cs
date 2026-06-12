using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils.NodeFactories;
using Godot;
using KeineMod.KeineModCode.Cards.Basic;
using KeineMod.KeineModCode.Cards.Rare;
using KeineMod.KeineModCode.Extensions;
using KeineMod.KeineModCode.Relics;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace KeineMod.KeineModCode.Character;

public class KeineMod : PlaceholderCharacterModel
{
    public static NCreatureVisuals? ActiveVisuals { get; private set; }

    public KeineMod() : base()
    {
        // Since human_form.tscn is tracked by CustomVisualPath, BaseLib auto-registers it.
        // We just manually tell BaseLib to bake the Hakutaku form at startup here:
        "visual/hakutaku_form.tscn".ScenePath().RegisterSceneForConversion<NCreatureVisuals>();
    }

    public const string CharacterId = "KeineMod";

    public static readonly Color Color = new("#00afaf");

    public override Color NameColor => Color;
    public override Color EnergyLabelOutlineColor => new("#8b0000");
    public override Color MapDrawingColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 75;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<Reincarnation>()
        // ModelDb.Card<StrikeKeine>(),
        // ModelDb.Card<StrikeKeine>(),
        // ModelDb.Card<StrikeKeine>(),
        // ModelDb.Card<StrikeKeine>(),
        // ModelDb.Card<DefendKeine>(),
        // ModelDb.Card<DefendKeine>(),
        // ModelDb.Card<DefendKeine>(),
        // ModelDb.Card<DefendKeine>(),
        // ModelDb.Card<TidesOfTime>(),
        // ModelDb.Card<ManipulateHistory>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<OldScroll>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<KeineModCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<KeineModRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<KeineModPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomVisualPath => "visual/human_form.tscn".ScenePath();

    public override NCreatureVisuals CreateCustomVisuals()
    {
        // 1. Instantiate your default human form visual scene as the base root
        var humanScene = GD.Load<PackedScene>(CustomVisualPath);
        var visuals = humanScene.Instantiate<NCreatureVisuals>();

        // 2. Load and instantiate the Hakutaku scene
        var hakuScene = GD.Load<PackedScene>("visual/hakutaku_form.tscn".ScenePath());
        var hakuNode = hakuScene.Instantiate();
        hakuNode.Name = "HakutakuFormNode";

        // Hide the Hakutaku visuals by default when entering combat
        if (hakuNode is CanvasItem canvasHaku) canvasHaku.Visible = false;

        // 3. Nest the Hakutaku form safely inside the root visuals
        visuals.AddChild(hakuNode);

        ActiveVisuals = visuals;
        return visuals;
    }

    public override string CustomCharacterSelectBg => "select/character_select_bg_keine.tscn".ScenePath();
    // public override string CustomEnergyCounterPath => "energy/energy_counter_mokou.tscn".ScenePath();
    // public override string CustomRestSiteAnimPath => "rest/rest_site_mokou.tscn".ScenePath();
    // public override string CustomMerchantAnimPath => "merchant/merchant_mokou.tscn".ScenePath();

    public override string CustomIconTexturePath => "character_icon_keine.png".CharacterUiPath();
    public override string CustomIconOutlineTexturePath => "character_icon_outline_keine.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_keine.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_keine_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_keine.png".CharacterUiPath();

    // public override string CustomArmPointingTexturePath => "mokou_point.png".CharacterUiPath();
    // public override string CustomArmRockTexturePath => "mokou_rock.png".CharacterUiPath();
    // public override string CustomArmPaperTexturePath => "mokou_paper.png".CharacterUiPath();
    // public override string CustomArmScissorsTexturePath => "mokou_scissors.png".CharacterUiPath();

    public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/ironclad_transition_mat.tres";

    //public override string CustomAttackSfx => null;
    //public override string CustomCastSfx => null;
    //public override string CustomDeathSfx => null;
    //public override string CharacterSelectSfx => null;
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

    // public override RelicIconData CustomYummyCookie => new("yummy_cookie_mokou.png".BigRelicImagePath(), "yummy_cookie_mokou.png".RelicImagePath(), "relic_outline.png".RelicImagePath());
}