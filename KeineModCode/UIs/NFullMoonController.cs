using Godot;
using KeineMod.KeineModCode.Core;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;

namespace KeineMod.KeineModCode.UIs;

public partial class NFullMoonController : Control
{
    private Player? _player;
    private Control _ui;
    private Label? _countLabel;
    private TextureRect? _fullMoonIcon;
    private TextureRect? _imperishableNightIcon;

    // Hover Scaling Variables
    private readonly Vector2 _baseScale = new(1.5f, 1.5f);
    private readonly Vector2 _hoverScale = new(1.7f, 1.7f); // Scales up slightly on hover
    private Vector2 _targetScale;

    public override void _Ready()
    {
        _ui = GetParent<Control>();
        _countLabel = _ui.GetNodeOrNull<Label>((NodePath)"Count") ?? _ui.GetNodeOrNull<Label>((NodePath)"CountContainer/Count");
        _fullMoonIcon = _ui.GetNodeOrNull<TextureRect>((NodePath)"FullMoonIcon");
        _imperishableNightIcon = _ui.GetNodeOrNull<TextureRect>((NodePath)"ImperishableNightIcon");

        _targetScale = _baseScale;
        _ui.Scale = _baseScale;
        _ui.MouseFilter = MouseFilterEnum.Stop;
        _ui.GuiInput += OnGuiInput;
        _ui.MouseEntered += () => _targetScale = _hoverScale;
        _ui.MouseExited += () => _targetScale = _baseScale;
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (_player == null || @event is not InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true } || !LocalContext.IsMe(_player) || !CombatManager.Instance.IsPartOfPlayerTurn(_player))
            return;

        var fullMoonUi = KeineConstantsStateRegistry.Get(_player);
        if (fullMoonUi.CanUse(_player)) RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(new UseFullMoonAction(_player));
    }

    public override void _Process(double delta)
    {
        if (_player == null)
            return;
        base._Process(delta);

        if (!_player.Creature.HasPower<TimeShiftPower>() && !_player.Creature.HasPower<ImperishableNightPower>())
        {
            _ui.Visible = false;
        }
        else
        {
            _ui.Visible = true;

            // Smoothly interpolate UI scale for the hover animation
            _ui.Scale = _ui.Scale.Lerp(_targetScale, (float)delta * 12f);

            var registry = KeineConstantsStateRegistry.Get(_player);
            var charge = registry.FullMoonCharge;

            if (_countLabel != null)
                _countLabel.Text = charge.ToString();

            // --- VISUAL LOGIC CONTEXT SWITCHING ---
            if (_player.Creature.HasPower<ImperishableNightPower>())
            {
                // Show Imperishable Night, Hide Full Moon
                if (_imperishableNightIcon != null)
                {
                    _imperishableNightIcon.Visible = true;
                    _imperishableNightIcon.Rotation += (float)delta * 0.2f;
                    _imperishableNightIcon.Modulate = Colors.White;
                }

                if (_fullMoonIcon != null) _fullMoonIcon.Visible = false;
            }
            else
            {
                // Hide Imperishable Night, Show and Animate Full Moon
                if (_imperishableNightIcon != null) _imperishableNightIcon.Visible = false;

                if (_fullMoonIcon != null)
                {
                    _fullMoonIcon.Visible = true;
                    var isHakutaku = KeineModel.IsInStance<HakutakuForm>(_player);

                    if (isHakutaku)
                    {
                        _fullMoonIcon.Rotation += (float)delta * 0.4f;
                        _fullMoonIcon.Modulate = Colors.White;
                    }
                    else if (charge > 0)
                    {
                        _fullMoonIcon.Rotation += (float)delta * 0.2f;
                        _fullMoonIcon.Modulate = new Color(0.93f, 0.95f, 0.62f);
                    }
                    else
                    {
                        _fullMoonIcon.Modulate = new Color(0.4f, 0.4f, 0.4f);
                    }
                }
            }

            _ui.Modulate = Colors.White;
        }
    }

    public void Initialize(Player player)
    {
        _player = player;
    }
}