using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class Fatigue : KeineModCard
{
    public Fatigue() : base(-1, CardType.Status, CardRarity.Token, TargetType.Self)
    {
        WithKeywords(CardKeyword.Unplayable);
    }

    public override int MaxUpgradeLevel => 0;

    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        if (card != this)
            return;
        await PowerCmd.Apply<FatiguePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}