namespace KeineMod.KeineModCode.UIs;

public class FullMoonChargeState
{
    public int FullMoonCharge;
    public bool ClickedThisTurn;

    public void GainFullMoon(int amount)
    {
        FullMoonCharge += amount;
    }

    public bool CanUse()
    {
        return FullMoonCharge > 0 && !ClickedThisTurn;
    }

    public void LoseFullMoon(int amount)
    {
        FullMoonCharge = Math.Max(0, FullMoonCharge - amount);
    }
}