using KeineMod.KeineModCode.Core;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Relics;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Entities.Players;

namespace KeineMod.KeineModCode.Scripts;

public class KeineConstantsState
{
    public int FullMoonCharge;
    public bool ClickedThisTurn;
    public int CardsConsumedThisCombat;

    public void GainFullMoon(int amount)
    {
        FullMoonCharge += amount;
    }

    public bool CanUse(Player player)
    {
        return FullMoonCharge > 0 && (!ClickedThisTurn || player.GetRelic<PocketWatch>() != null) && !KeineModel.IsInStance<HakutakuForm>(player) && !player.Creature.HasPower<SunrisePower>();
    }

    public void LoseFullMoon(int amount)
    {
        FullMoonCharge = Math.Max(0, FullMoonCharge - amount);
    }

    public void IncrementCardsConsumed(int amount)
    {
        CardsConsumedThisCombat += amount;
    }
}