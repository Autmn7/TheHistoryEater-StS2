using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Common;

public class MasakadoCrisis : KeineModCard
{
    public MasakadoCrisis() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(12, 4);
        WithCards(2);
        WithPower<TimeShiftPower>(2);
        WithEnergy(2);
        WithKeywords(KeineModKeywords.Human, KeineModKeywords.Consume, KeineModKeywords.Hakutaku, KeineModKeywords.Recall);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        if (InHuman())
        {
            var consumedCards = (await ConsumeCmd.FromHandUpTo(choiceContext, Owner, DynamicVars.Cards.IntValue, this)).ToList();
            for (var i = 0; i < consumedCards.Count; ++i) await PowerCmd.Apply<TimeShiftPower>(choiceContext, Owner.Creature, DynamicVars["TimeShiftPower"].BaseValue, Owner.Creature, this);
        }

        if (InHakutaku())
        {
            var recalledCards = (await RecallCmd.FromScrollUpTo(choiceContext, Owner, DynamicVars.Cards.IntValue)).ToList();
            var energy = DynamicVars.Energy.BaseValue - recalledCards.Count;
            if (energy > 0)
                await PlayerCmd.GainEnergy(energy, Owner);
        }
    }
}