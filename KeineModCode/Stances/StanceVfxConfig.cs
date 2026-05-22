using System.Runtime.CompilerServices;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace KeineMod.KeineModCode.Stances;

public record StanceVfxConfig(string? AuraScenePath = null, Color? BodyTint = null, string? EnterSfxPath = null, Color? ScreenFlashColor = null, ShakeStrength ScreenShakeStrength = (ShakeStrength)0, string? AmbienceLoopPath = null)
{
    public IEnumerable<string> AssetPaths
    {
        get
        {
            if (AuraScenePath != null) yield return AuraScenePath;
            if (AmbienceLoopPath != null) yield return AmbienceLoopPath;
            if (EnterSfxPath != null) yield return EnterSfxPath;
        }
    }

    [CompilerGenerated]
    public void Deconstruct(out string? AuraScenePath, out Color? BodyTint, out string? EnterSfxPath, out Color? ScreenFlashColor, out ShakeStrength ScreenShakeStrength, out string? AmbienceLoopPath)
    {
        //IL_002c: Unknown result type (might be due to invalid IL or missing references)
        //IL_0032: Expected I4, but got Unknown
        AuraScenePath = this.AuraScenePath;
        BodyTint = this.BodyTint;
        EnterSfxPath = this.EnterSfxPath;
        ScreenFlashColor = this.ScreenFlashColor;
        ScreenShakeStrength = (ShakeStrength)(int)this.ScreenShakeStrength;
        AmbienceLoopPath = this.AmbienceLoopPath;
    }
}