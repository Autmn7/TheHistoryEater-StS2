using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace KeineMod.KeineModCode.Stances;

public class StanceVfxController(StanceVfxConfig cfg)
{
    private const float AmbienceFadeTime = 0.6f;

    private const float AmbienceVolume = -6f;

    private static Color? _originalModulate;

    private static AudioStreamPlayer? _ambiencePlayer;

    private Node2D? _vfxInstance;

    public async Task OnEnter(Creature owner)
    {
        // await CreateAura(owner);
        ApplyBodyTint(owner);
        // PlayEnterSfx();
        // StartAmbience();
        if (LocalContext.IsMe(owner))
            // PlayScreenFlash();
            PlayScreenShake();
    }

    public async Task OnExit(Creature owner)
    {
        // RemoveAura();
        ResetBodyTint(owner);
        // StopAmbience();
        await Task.CompletedTask;
    }

    // private Task CreateAura(Creature owner)
    // {
    // 	//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_0134: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_0139: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_0147: Unknown result type (might be due to invalid IL or missing references)
    // 	if (cfg.AuraScenePath == null)
    // 	{
    // 		return Task.CompletedTask;
    // 	}
    // 	NCombatRoom instance = NCombatRoom.Instance;
    // 	object obj;
    // 	if (instance == null)
    // 	{
    // 		obj = null;
    // 	}
    // 	else
    // 	{
    // 		NCreature creatureNode = instance.GetCreatureNode(owner);
    // 		obj = ((creatureNode != null) ? creatureNode.Visuals : null);
    // 	}
    // 	NCreatureVisuals val = (NCreatureVisuals)obj;
    // 	if (val == null)
    // 	{
    // 		return Task.CompletedTask;
    // 	}
    // 	Node2D val2 = ((Node)val).GetNodeOrNull<Node2D>(NodePath.op_Implicit("StanceVfxContainer")) ?? CreateContainer((Node)(object)val);
    // 	if (_vfxInstance != null && GodotObject.IsInstanceValid((GodotObject)(object)_vfxInstance))
    // 	{
    // 		((Node)_vfxInstance).QueueFree();
    // 	}
    // 	_vfxInstance = PreloadManager.Cache.GetScene(cfg.AuraScenePath).Instantiate<Node2D>((PackedScene.GenEditState)0);
    // 	_vfxInstance.Position = Vector2.Zero;
    // 	_vfxInstance.Scale = Vector2.One;
    // 	((Node)val2).AddChild((Node)(object)_vfxInstance, false, (Node.InternalMode)0);
    // 	foreach (Node2D item in ((IEnumerable<Node>)((Node)_vfxInstance).GetChildren(false)).Where((Node c) => ((object)c.Name).ToString().Contains("Burst")).Cast<Node2D>())
    // 	{
    // 		Vector2 globalPosition = item.GlobalPosition;
    // 		((Node)item).Reparent((Node)(object)val, true);
    // 		item.GlobalPosition = globalPosition;
    // 		((Node)val).MoveChild((Node)(object)item, 0);
    // 	}
    // 	return Task.CompletedTask;
    // }

    // private static Node2D CreateContainer(Node visuals)
    // {
    // 	//IL_0001: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_0006: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_0017: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_0018: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_0024: Expected O, but got Unknown
    // 	Node2D val = new Node2D
    // 	{
    // 		Name = StringName.op_Implicit("StanceVfxContainer"),
    // 		Position = Vector2.Zero
    // 	};
    // 	visuals.AddChild((Node)(object)val, false, (Node.InternalMode)0);
    // 	return val;
    // }

    // private void RemoveAura()
    // {
    // 	if (_vfxInstance == null || !GodotObject.IsInstanceValid((GodotObject)(object)_vfxInstance))
    // 	{
    // 		return;
    // 	}
    // 	foreach (Node child in ((Node)_vfxInstance).GetChildren(false))
    // 	{
    // 		Node val = child;
    // 		Node val2 = val;
    // 		if (!(val2 is WrathGlowSparkSpawner wrathGlowSparkSpawner))
    // 		{
    // 			if (!(val2 is CalmFrostStreakSpawner calmFrostStreakSpawner))
    // 			{
    // 				if (!(val2 is DivinityEyeSpawner divinityEyeSpawner))
    // 				{
    // 					AuraBlobEmitter auraBlobEmitter = val2 as AuraBlobEmitter;
    // 					if (auraBlobEmitter == null)
    // 					{
    // 						continue;
    // 					}
    // 					foreach (CpuParticles2D item in ((IEnumerable)((Node)auraBlobEmitter).GetChildren(false)).OfType<CpuParticles2D>())
    // 					{
    // 						item.Emitting = false;
    // 					}
    // 					SceneTreeTimer val3 = ((Node)auraBlobEmitter).GetTree().CreateTimer(2.5, true, false, false);
    // 					val3.Timeout += delegate
    // 					{
    // 						if (GodotObject.IsInstanceValid((GodotObject)(object)auraBlobEmitter))
    // 						{
    // 							((Node)auraBlobEmitter).QueueFree();
    // 						}
    // 					};
    // 				}
    // 				else
    // 				{
    // 					divinityEyeSpawner.StopSpawning();
    // 				}
    // 			}
    // 			else
    // 			{
    // 				calmFrostStreakSpawner.StopSpawning();
    // 			}
    // 		}
    // 		else
    // 		{
    // 			wrathGlowSparkSpawner.StopSpawning();
    // 		}
    // 	}
    // 	_vfxInstance = null;
    // }

    private void ApplyBodyTint(Creature owner)
    {
        //IL_004b: Unknown result type (might be due to invalid IL or missing references)
        //IL_0050: Unknown result type (might be due to invalid IL or missing references)
        //IL_0083: Unknown result type (might be due to invalid IL or missing references)
        //IL_005f: Unknown result type (might be due to invalid IL or missing references)
        //IL_0064: Unknown result type (might be due to invalid IL or missing references)
        //IL_0066: Unknown result type (might be due to invalid IL or missing references)
        if (!cfg.BodyTint.HasValue) return;
        var instance = NCombatRoom.Instance;
        object obj;
        if (instance == null)
        {
            obj = null;
        }
        else
        {
            var creatureNode = instance.GetCreatureNode(owner);
            obj = creatureNode != null ? creatureNode.Body : null;
        }

        var val = (Node2D)obj;
        if (val != null)
        {
            var valueOrDefault = _originalModulate.GetValueOrDefault();
            if (!_originalModulate.HasValue)
            {
                valueOrDefault = ((CanvasItem)val).Modulate;
                _originalModulate = valueOrDefault;
            }

            ((CanvasItem)val).Modulate = cfg.BodyTint.Value;
        }
    }

    private void ResetBodyTint(Creature owner)
    {
        //IL_0043: Unknown result type (might be due to invalid IL or missing references)
        if (_originalModulate.HasValue)
        {
            var instance = NCombatRoom.Instance;
            object obj;
            if (instance == null)
            {
                obj = null;
            }
            else
            {
                var creatureNode = instance.GetCreatureNode(owner);
                obj = creatureNode != null ? creatureNode.Body : null;
            }

            var val = (Node2D)obj;
            if (val != null)
            {
                ((CanvasItem)val).Modulate = _originalModulate.Value;
                _originalModulate = null;
            }
        }
    }

    // private void PlayEnterSfx()
    // {
    // 	if (cfg.EnterSfxPath != null)
    // 	{
    // 		StanceVfx.PlayStanceSfx(cfg.EnterSfxPath);
    // 	}
    // }
    //
    // private void PlayScreenFlash()
    // {
    // 	//IL_0026: Unknown result type (might be due to invalid IL or missing references)
    // 	if (cfg.ScreenFlashColor.HasValue)
    // 	{
    // 		ScreenFlashEffect.Play(cfg.ScreenFlashColor.Value);
    // 	}
    // }

    private void PlayScreenShake()
    {
        //IL_0007: Unknown result type (might be due to invalid IL or missing references)
        //IL_000d: Invalid comparison between Unknown and I4
        //IL_0024: Unknown result type (might be due to invalid IL or missing references)
        if ((int)cfg.ScreenShakeStrength > 0)
        {
            var instance = NGame.Instance;
            if (instance != null) instance.ScreenShake(cfg.ScreenShakeStrength, (ShakeDuration)1, -1f);
        }
    }

    // private void StartAmbience()
    // {
    // 	//IL_004e: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_0053: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_006f: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_0080: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_0091: Expected O, but got Unknown
    // 	//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
    // 	if (cfg.AmbienceLoopPath != null)
    // 	{
    // 		if (_ambiencePlayer != null && GodotObject.IsInstanceValid((GodotObject)(object)_ambiencePlayer))
    // 		{
    // 			((Node)_ambiencePlayer).QueueFree();
    // 		}
    // 		NCombatRoom instance = NCombatRoom.Instance;
    // 		if (instance != null)
    // 		{
    // 			_ambiencePlayer = new AudioStreamPlayer
    // 			{
    // 				Stream = PreloadManager.Cache.GetAsset<AudioStream>(cfg.AmbienceLoopPath),
    // 				Bus = StringName.op_Implicit("SFX"),
    // 				VolumeDb = -80f
    // 			};
    // 			((Node)instance).AddChild((Node)(object)_ambiencePlayer, false, (Node.InternalMode)0);
    // 			_ambiencePlayer.Play(0f);
    // 			((Node)_ambiencePlayer).CreateTween().TweenProperty((GodotObject)(object)_ambiencePlayer, NodePath.op_Implicit("volume_db"), Variant.op_Implicit(-6f), 0.6000000238418579);
    // 		}
    // 	}
    // }
    //
    // private static void StopAmbience()
    // {
    // 	//IL_0057: Unknown result type (might be due to invalid IL or missing references)
    // 	//IL_0078: Unknown result type (might be due to invalid IL or missing references)
    // 	if (_ambiencePlayer == null || !GodotObject.IsInstanceValid((GodotObject)(object)_ambiencePlayer))
    // 	{
    // 		return;
    // 	}
    // 	AudioStreamPlayer player = _ambiencePlayer;
    // 	_ambiencePlayer = null;
    // 	Tween val = ((Node)player).CreateTween();
    // 	val.TweenProperty((GodotObject)(object)player, NodePath.op_Implicit("volume_db"), Variant.op_Implicit(-80f), 0.6000000238418579);
    // 	val.TweenCallback(Callable.From((Action)delegate
    // 	{
    // 		if (GodotObject.IsInstanceValid((GodotObject)(object)player))
    // 		{
    // 			((Node)player).QueueFree();
    // 		}
    // 	}));
    // }
}