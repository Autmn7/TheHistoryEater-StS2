using BaseLib.Abstracts;
using Godot;
using KeineMod.KeineModCode.Cards;
using KeineMod.KeineModCode.Core;
using KeineMod.KeineModCode.Extensions;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using KeineMod.KeineModCode.Stances;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace KeineMod.KeineModCode.Character;

public class KeineModCardPool : CustomCardPoolModel
{
    public override string Title => KeineMod.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();


    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 1f; //Hue; changes the color.
    public override float S => 1f; //Saturation
    public override float V => 1f; //Brightness

    //Color of small card icons
    public override Color DeckEntryCardColor => new("#00afaf");

    public override bool IsColorless => false;

    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load KeineMod/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/
    public override Texture2D CustomFrame(CustomCardModel card)
    {
        var cardFrame = "keine";
        if (card is KeineModCard keineCard && !keineCard.IsCanonical && keineCard.Owner?.Creature != null && card.Owner.PlayerCombatState != null && card.CombatState != null && card.CombatState.IsLiveCombat())
        {
            var hasHakutaku = keineCard.Keywords.Contains(KeineKeywords.Hakutaku);
            var hasHuman = keineCard.Keywords.Contains(KeineKeywords.Human);
            var hasDualForm = keineCard.Owner.Creature.HasPower<DualFormPower>();
            var isHakutakuForm = KeineModel.IsInStance<HakutakuForm>(keineCard.Owner);

            if (hasHakutaku)
            {
                if (!hasDualForm)
                {
                    if (isHakutakuForm)
                        cardFrame = "hakutaku";
                }
                else
                {
                    cardFrame = !hasHuman ? "hakutaku" : "dual";
                }
            }
        }

        var type = card.Type switch
        {
            CardType.Attack => "attack",
            CardType.Power => "power",
            _ => "skill"
        };

        return PreloadManager.Cache.GetAsset<Texture2D>(
            $"frame_{type}_{cardFrame}.png".CardFramePath());
    }
}