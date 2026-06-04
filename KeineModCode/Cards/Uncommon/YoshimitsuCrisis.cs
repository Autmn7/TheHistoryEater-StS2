using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class YoshimitsuCrisis : KeineModCard
{
    public YoshimitsuCrisis() : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(16, 5);
        WithBlock(3);
        WithCards(3);
        WithKeywords(KeineKeywords.Human, KeineKeywords.Consume, KeineKeywords.Hakutaku, KeineKeywords.Recall);
        WithEnergyTip();
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        if (InHuman())
        {
            var consumedCards = (await ConsumeCmd.FromHandUpTo(choiceContext, Owner, DynamicVars.Cards.IntValue, this)).ToList();
            if (consumedCards.Count > 0)
                await PlayerCmd.GainEnergy(consumedCards.Count, Owner);
        }

        if (InHakutaku())
        {
            var recalledCards = (await RecallCmd.FromScrollUpTo(choiceContext, Owner, DynamicVars.Cards.IntValue)).ToList();
            for (var i = 0; i < recalledCards.Count; ++i) await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }
    }
}