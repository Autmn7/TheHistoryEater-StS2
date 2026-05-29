using KeineMod.KeineModCode.Scripts;
using KeineMod.KeineModCode.UIs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Common;

public class WellRead : KeineModCard
{
    public WellRead() : base(3, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        WithBlock(10, 4);
        WithEnergy(1);
        WithKeyword(KeineModKeywords.Knowledgeable);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
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
            var bonus = ScrollPile.Scroll.GetPile(Owner).Cards.Count * DynamicVars.Energy.IntValue;
            var targetCost = Math.Max(0, baseCost - bonus);
            EnergyCost._localModifiers.RemoveAll(m => (int)m.Type == 1 && (int)m.Expiration == 0);
            EnergyCost.SetThisCombat(targetCost);
            InvokeEnergyCostChanged();
        }
    }
}