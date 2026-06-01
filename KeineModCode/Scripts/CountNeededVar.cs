using KeineMod.KeineModCode.Cards.Rare;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Scripts;

public class CountNeededVar : DynamicVar
{
    public const string DefaultName = "CountNeeded";

    public CountNeededVar(decimal value) : base(DefaultName, value)
    {
    }

    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        if (card is Deconstruction deconstruction)
            PreviewValue = deconstruction.CountNeeded;
    }
}