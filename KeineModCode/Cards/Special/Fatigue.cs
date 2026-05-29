using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class Fatigue : KeineModCard, IOnConsumed
{
    public Fatigue() : base(-1, CardType.Status, CardRarity.Status, TargetType.Self)
    {
        WithKeywords(CardKeyword.Unplayable);
        WithTip(KeineModKeywords.Consume);
    }

    public override int MaxUpgradeLevel => 0;

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard == this)
            await PowerCmd.Apply<FatiguePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}