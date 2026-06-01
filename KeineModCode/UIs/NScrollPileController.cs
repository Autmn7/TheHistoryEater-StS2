using Godot;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Screens;

namespace KeineMod.KeineModCode.UIs;

public partial class NScrollPileController : Control
{
    private Player? _player;
    private Control _ui;
    private Label? _countLabel;

    // Hover Scaling Variables
    private readonly Vector2 _baseScale = new(1.5f, 1.5f);
    private readonly Vector2 _hoverScale = new(1.7f, 1.7f); // Scales up slightly on hover
    private Vector2 _targetScale;

    public override void _Ready()
    {
        _ui = GetParent<Control>();
        _countLabel = _ui.GetNodeOrNull<Label>((NodePath)"Count") ?? _ui.GetNodeOrNull<Label>((NodePath)"CountContainer/Count");
        _targetScale = _baseScale;
        _ui.Scale = _baseScale;
        _ui.MouseFilter = MouseFilterEnum.Stop;
        _ui.GuiInput += OnGuiInput;
        _ui.MouseEntered += () => _targetScale = _hoverScale;
        _ui.MouseExited += () => _targetScale = _baseScale;
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
            _ui.Visible = false;
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