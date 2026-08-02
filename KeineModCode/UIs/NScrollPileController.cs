using Godot;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace KeineMod.KeineModCode.UIs;

public partial class NScrollPileController : Control
{
    private Player? _player;
    private Control _ui;
    private Label? _countLabel;
    private HoverTip _hoverTip;

    // Hover Scaling Variables
    private readonly Vector2 _baseScale = new(1.5f, 1.5f);
    private readonly Vector2 _hoverScale = new(1.7f, 1.7f); // Scales up slightly on hover
    private Vector2 _targetScale;

    public override void _Ready()
    {
        _ui = GetParent<Control>();
        _countLabel = _ui.GetNodeOrNull<Label>((NodePath)"Count") ?? _ui.GetNodeOrNull<Label>((NodePath)"CountContainer/Count");

        // Initialize HoverTip with localization keys
        var tipTitle = new LocString("static_hover_tips", "KEINEMOD-SCROLL_PILE.title");
        var tipDesc = new LocString("static_hover_tips", "KEINEMOD-SCROLL_PILE.description");
        _hoverTip = new HoverTip(tipTitle, tipDesc);

        _targetScale = _baseScale;
        _ui.Scale = _baseScale;
        _ui.MouseFilter = MouseFilterEnum.Stop;
        _ui.GuiInput += OnGuiInput;

        // Connect mouse hover events
        _ui.MouseEntered += OnHovered;
        _ui.MouseExited += OnUnhovered;
    }

    private void OnHovered()
    {
        _targetScale = _hoverScale;

        // Spawn and position the hover tip above the element
        NHoverTipSet.CreateAndShow(_ui, _hoverTip)
            ?.SetGlobalPosition(_ui.GlobalPosition + new Vector2(125f, -150f));
    }

    private void OnUnhovered()
    {
        _targetScale = _baseScale;

        // Remove hover tip when mouse leaves
        NHoverTipSet.Remove(_ui);
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (_player == null || @event is not InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
            return;
        var scrollPile = _player != null ? ScrollPile.Scroll.GetPile(_player) : null;
        if (scrollPile is { IsEmpty: false })
        {
            NCardPileScreen.ShowScreen(scrollPile, []);
            _ui.GetViewport().SetInputAsHandled();
        }
    }

    public override void _Process(double delta)
    {
        if (_player == null)
            return;
        base._Process(delta);
        var scrollPile = _player != null ? ScrollPile.Scroll.GetPile(_player) : null;
        if (scrollPile == null || CombatManager.Instance.IsOverOrEnding || KeineConstantsStateRegistry.Get(_player).CardsConsumedThisCombat <= 0)
        {
            if (_ui.Visible)
            {
                NHoverTipSet.Remove(_ui); // Safeguard: remove tip if element hides while hovering
                _ui.Visible = false;
            }
        }
        else
        {
            _ui.Visible = true;
            _ui.Scale = _ui.Scale.Lerp(_targetScale, (float)delta * 12f);
            if (_countLabel != null)
                _countLabel.Text = scrollPile.Cards.Count.ToString();
            _ui.Modulate = Colors.White;
        }
    }

    public void Initialize(Player player)
    {
        _player = player;
    }
}