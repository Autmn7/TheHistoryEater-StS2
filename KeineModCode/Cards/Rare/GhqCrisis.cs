using KeineMod.KeineModCode.Scripts;
using KeineMod.KeineModCode.UIs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Rare;

public class GhqCrisis : KeineModCard, IOnConsumed
{
    public GhqCrisis() : base(4, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(4);
        WithCalculatedVar("Repeat", 0, (card, _) =>
        {
            if (card is not KeineModCard keineModCard)
                return 0;
            var totalCount = 0;
            if (keineModCard.InHuman())
                totalCount += ScrollPile.Scroll.GetPile(keineModCard.Owner).Cards.ToList().Count;
            if (keineModCard.InHakutaku())
                totalCount += PileType.Hand.GetPile(keineModCard.Owner).Cards.Where(c => c != keineModCard).ToList().Count;
            return totalCount;
        });
        WithEnergy(1);
        WithKeywords(KeineKeywords.Human, KeineKeywords.Hakutaku);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
        WithTip(KeineKeywords.Consume);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitCount((int)((CalculatedVar)DynamicVars["Repeat"]).Calculate(cardPlay.Target)).Execute(choiceContext);
    }

    public Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard.Owner == Owner)
            EnergyCost.AddThisCombat(-DynamicVars.Energy.IntValue);
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || IsClone)
            return Task.CompletedTask;
        EnergyCost.AddThisCombat(-KeineConstantsStateRegistry.Get(Owner).CardsConsumedThisCombat * DynamicVars.Energy.IntValue);
        return Task.CompletedTask;
    }
}