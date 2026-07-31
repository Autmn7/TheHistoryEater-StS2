using Godot;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class SlayTheSerpent : KeineModCard
{
    public SlayTheSerpent() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(11, 3);
        WithCards(1);
        WithKeywords(KeineKeywords.Human, KeineKeywords.Consume, KeineKeywords.Create, KeineKeywords.Hakutaku);
        WithTip(typeof(Snakebite));
        WithTip(typeof(SerpentForm));
        WithTip(typeof(HeavenlySword));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithHitVfxNode((Func<Creature, Node2D>)(t => (Node2D)NBigSlashVfx.Create(t))).Execute(choiceContext);
        if (InHuman())
        {
            var consumedCard = await ConsumeCmd.FromHandSingle(choiceContext, Owner, this);
            if (consumedCard is Snakebite or SerpentForm)
            {
                CardModel sword = CombatState.CreateCard<HeavenlySword>(Owner);
                sword.EnergyCost.SetUntilPlayed(0);
                await CreateCmd.Execute(choiceContext, sword, Owner, IsUpgraded);
            }
        }

        if (InHakutaku())
        {
            CardModel serpent = CombatState.CreateCard<Snakebite>(Owner);
            if (IsUpgraded)
                serpent = CombatState.CreateCard<SerpentForm>(Owner);
            await CreateCmd.Execute(choiceContext, serpent, Owner);
        }
    }
}