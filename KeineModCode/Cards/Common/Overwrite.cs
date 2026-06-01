using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Common;

public class Overwrite : KeineModCard
{
    public Overwrite() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(7, 2);
        WithPower<HistoricalGapPower>(2, 1);
        WithCards(1);
        WithKeyword(KeineModKeywords.Create);
        WithTip(typeof(Flow));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        await PowerCmd.Apply<HistoricalGapPower>(choiceContext, cardPlay.Target, DynamicVars["HistoricalGapPower"].BaseValue, Owner.Creature, this);
        for (var i = 0; i < DynamicVars.Cards.BaseValue; ++i)
        {
            CardModel flow = CombatState.CreateCard<Flow>(Owner);
            await CreateCmd.Execute(flow, Owner);
        }
    }
}