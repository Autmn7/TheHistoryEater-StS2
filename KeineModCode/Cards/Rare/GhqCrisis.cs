using KeineMod.KeineModCode.Scripts;
using KeineMod.KeineModCode.UIs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Rare;

public class GhqCrisis : KeineModCard, IOnConsumed
{
    public GhqCrisis() : base(4, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(6, 2);
        WithEnergy(1);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithKeyword(CardKeyword.Exhaust);
        WithTip(KeineModKeywords.Consume);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var list = ScrollPile.Scroll.GetPile(Owner).Cards.ToList();
        var cardCount = list.Count;
        foreach (var card in list)
            await CardCmd.Exhaust(choiceContext, card);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitCount(cardCount).Execute(choiceContext);
    }

    public Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
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