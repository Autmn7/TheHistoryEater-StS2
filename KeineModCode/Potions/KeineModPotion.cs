using BaseLib.Abstracts;
using BaseLib.Utils;
using KeineMod.KeineModCode.Character;

namespace KeineMod.KeineModCode.Potions;

[Pool(typeof(KeineModPotionPool))]
public abstract class KeineModPotion : CustomPotionModel;