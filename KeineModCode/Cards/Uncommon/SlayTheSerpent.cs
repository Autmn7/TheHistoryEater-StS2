using BaseLib.Utils;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class SlayTheSerpent : KeineModCard
{
    public SlayTheSerpent() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(10, 4);
        WithCards(1);
        WithKeywords(KeineModKeywords.Human, KeineModKeywords.Consume, KeineModKeywords.Create, KeineModKeywords.Hakutaku);
        WithTip(typeof(Snakebite));
        WithTip(typeof(SerpentForm));
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<HeavenlySword>(card.IsUpgraded)));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        if (InHuman())
        {
            var consumedCard = await ConsumeCmd.FromHandSingle(choiceContext, Owner, this);
            if (consumedCard is Snakebite or SerpentForm)
            {
                CardModel sword = CombatState.CreateCard<HeavenlySword>(Owner);
                await CreateCmd.Execute(sword, Owner, IsUpgraded);
            }
        }

        if (InHakutaku())
        {
            CardModel serpent = CombatState.CreateCard<Snakebite>(Owner);
            if (IsUpgraded)
                serpent = CombatState.CreateCard<SerpentForm>(Owner);
            await CreateCmd.Execute(serpent, Owner);
        }
    }
}