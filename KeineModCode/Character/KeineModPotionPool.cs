using BaseLib.Abstracts;
using Godot;
using KeineMod.KeineModCode.Extensions;

namespace KeineMod.KeineModCode.Character;

public class KeineModPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => KeineMod.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}