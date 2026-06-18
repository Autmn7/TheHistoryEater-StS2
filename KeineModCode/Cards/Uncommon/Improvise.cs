using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class Improvise : KeineModCard
{
    public Improvise() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(6, 1);
        WithKeywords(KeineKeywords.Knowledgeable, KeineKeywords.Create);
        WithTip(StaticHoverTip.Block);
        WithTip(KeineKeywords.Knowledgeable);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext);

        var otherPools = Owner.UnlockState.CharacterCardPools.ToList();
        if (otherPools.Count > 1) otherPools.Remove(Owner.Character.CardPool);
        var blockPool = otherPools.SelectMany(pool => pool.AllCards).Where(c =>
            c.Type == CardType.Skill &&
            c.DynamicVars.ContainsKey("Block") &&
            c.DynamicVars["Block"].BaseValue > 1 &&
            c.BaseStarCost <= Owner.PlayerCombatState?.Stars
        );
        var generatedBlock = CardFactory.GetDistinctForCombat(Owner, blockPool, 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
        if (generatedBlock != null) await CreateCmd.Execute(choiceContext, generatedBlock, Owner, IsUpgraded, PileType.Hand, true);
    }
}