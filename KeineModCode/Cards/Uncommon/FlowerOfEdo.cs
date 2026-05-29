using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class FlowerOfEdo : KeineModCard, IOnConsumed
{
    public decimal _extraDamage;

    public FlowerOfEdo() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(10, 2);
        WithVar("Increase", 6, 2);
        WithTip(KeineModKeywords.Consume);
        WithTip(KeineModKeywords.Recall);
        WithTip(CardKeyword.Exhaust);
    }

    public decimal ExtraDamage
    {
        get => _extraDamage;
        set
        {
            AssertMutable();
            _extraDamage = value;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (var enemy in CombatState.HittableEnemies)
        {
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(enemy));
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireBurstVfx.Create(enemy, 0.75f));
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState).Execute(choiceContext);
    }

    public Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard != this)
            return Task.CompletedTask;
        var baseValue = DynamicVars["Increase"].BaseValue;
        var damage = DynamicVars.Damage;
        damage.BaseValue += baseValue;
        ExtraDamage += baseValue;
        return Task.CompletedTask;
    }
}