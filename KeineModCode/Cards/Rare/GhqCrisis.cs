using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using KeineMod.KeineModCode.UIs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Rare;

public class GhqCrisis : KeineModCard
{
    public GhqCrisis() : base(4, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(3, 1);
        WithVar(new RepeatVar(4));
        WithCards(4);
        WithKeywords(KeineModKeywords.Hakutaku);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState).WithHitCount(DynamicVars.Repeat.IntValue).Execute(choiceContext);
        if (InHakutaku())
        {
            await RecallCmd.FromScrollUpTo(choiceContext, Owner, DynamicVars.Cards.IntValue);
            await CardCmd.Exhaust(choiceContext, this);
        }
    }

    public override async Task AfterCardChangedPilesLate(CardModel card, PileType oldPileType, AbstractModel? source)
    {
        await UpdateCost();
    }

    public override async Task AfterCardEnteredCombat(CardModel card)
    {
        await UpdateCost();
    }

    private async Task UpdateCost()
    {
        if (Pile != null && Pile.IsCombatPile && Owner.PlayerCombatState != null && CombatState != null && CombatState.IsLiveCombat())
        {
            var baseCost = EnergyCost._base;
            var bonus = ScrollPile.Scroll.GetPile(Owner).Cards.Count;
            var targetCost = Math.Max(0, baseCost - bonus);
            EnergyCost._localModifiers.RemoveAll(m => (int)m.Type == 1 && (int)m.Expiration == 0);
            EnergyCost.SetThisCombat(targetCost);
            InvokeEnergyCostChanged();
        }
    }
}