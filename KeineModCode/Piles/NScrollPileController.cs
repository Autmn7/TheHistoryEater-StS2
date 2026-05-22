using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Runs;

namespace KeineMod.KeineModCode.Piles;

public partial class NScrollPileController : Control
{
    public Player? _player;
    private Control _ui;
    private Label? _countLabel;

    public override void _Ready()
    {
        _ui = GetParent<Control>();
        _countLabel = _ui.GetNodeOrNull<Label>((NodePath)"Count") ?? _ui.GetNodeOrNull<Label>((NodePath)"CountContainer/Count");
        _ui.Scale = new Vector2(1.5f, 1.5f);
        _ui.MouseFilter = MouseFilterEnum.Stop;
        _ui.GuiInput += new GuiInputEventHandler(OnGuiInput);
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (!(@event is InputEventMouseButton eventMouseButton) || eventMouseButton.ButtonIndex != MouseButton.Left || !eventMouseButton.Pressed)
            return;
        var state = RunManager.Instance.State;
        var scrollPile = _player != null ? ScrollPile.Scroll.GetPile(_player) : (CardPile)null;
        if (scrollPile != null && !scrollPile.IsEmpty)
        {
            NCardPileScreen.ShowScreen(scrollPile, new string[0]);
            _ui.GetViewport().SetInputAsHandled();
        }
    }

    public override void _Process(double delta)
    {
        if (_player == null)
            return;
        base._Process(delta);
        var state = RunManager.Instance.State;
        var scrollPile = _player != null ? ScrollPile.Scroll.GetPile(_player) : (CardPile)null;
        if (scrollPile == null || scrollPile.IsEmpty)
        {
            _ui.Visible = false;
        }
        else
        {
            _ui.Visible = true;
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