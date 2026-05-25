using Godot;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;

namespace KeineMod.KeineModCode.UIs;

public partial class NFullMoonController : Control
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

    // ⚡ 改造重点 1：去掉了 async 关键字
    // 因为前端 UI 现在只负责“发送点击请求”，不再直接等待后端的异步命令执行完毕。
    private void OnGuiInput(InputEvent @event)
    {
        if (_player == null || !(@event is InputEventMouseButton eventMouseButton) || eventMouseButton.ButtonIndex != MouseButton.Left || !eventMouseButton.Pressed)
            return;

        // 🔒 联机防线 1：必须是“本地玩家”在点击“自己的 UI”
        // 防止联机时，你点击自己的按钮却触发了队友的动作，或者队友的点击在你的电脑上被错误触发。
        if (!LocalContext.IsMe(_player))
            return;

        // 🔒 联机防线 2：必须在当前玩家的回合内才可以点击
        if (!CombatManager.Instance.IsPartOfPlayerTurn(_player))
            return;

        var fullMoonUi = FullMoonChargeStateRegistry.Get(_player);
        
        // 🔒 联机防线 3：在本地进行前端预检（是否有层数、本回合是否用过）
        if (fullMoonUi.CanUse())
        {
            // ❌ 注意：【绝对不要】在这里写 fullMoonUi.LoseFullMoon(1); 
            // 如果在 UI 里直接扣除层数，会导致你本地扣了，但队友没扣，从而引发不同步（Desync）。

            // 🚀 【核心改变】：将动作打包，丢进联机同步队列
            // 游戏底层会自动调用该 Action 的 ToNetAction() 将其广播给所有玩家，
            // 所有人（包括你自己）的后端队列都会收到这个 Action，并严格同步地执行扣层数和加能力。
            RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue(new UseFullMoonAction(_player));
        }
    }

    public override void _Process(double delta)
    {
        if (_player == null)
            return;
        base._Process(delta);
        if (!_player.Creature.HasPower<TimeShiftPower>())
        {
            _ui.Visible = false;
        }
        else
        {
            _ui.Visible = true;
            // 每帧自动从 Registry 读取最新的层数并刷新显示
            // 因为 Action 执行时所有人的 Registry 都会同步更新，所以这里的 UI 刷新在所有人电脑上都是准确的。
            if (_countLabel != null)
                _countLabel.Text = FullMoonChargeStateRegistry.Get(_player).FullMoonCharge.ToString();
            _ui.Modulate = Colors.White;
        }
    }

    public void Initialize(Player player)
    {
        _player = player;
    }
}