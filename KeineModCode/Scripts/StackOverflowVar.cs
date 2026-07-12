using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Scripts;

public class StackOverflowVar : DynamicVar
{
    private readonly int _divisor;

    public StackOverflowVar(string name, int divisor) : base(name, 0M)
    {
        _divisor = divisor <= 0 ? 1 : divisor;
    }

    /// <summary>
    /// Overriding this guarantees that whenever the UI asks for this variable's value 
    /// to format your description string, it calculates it fresh based on your current Power stacks.
    /// </summary>
    protected override decimal GetBaseValueForIConvertible()
    {
        if (_owner is PowerModel power) return (decimal)Math.Floor(power.Owner.GetPowerAmount<KnowledgePower>() / (double)_divisor);
        return base.GetBaseValueForIConvertible();
    }

    /// <summary>
    /// Backing this up ensures things like ToString() and standard selectors match perfectly.
    /// </summary>
    public override string ToString()
    {
        return ((int)GetBaseValueForIConvertible()).ToString();
    }
}