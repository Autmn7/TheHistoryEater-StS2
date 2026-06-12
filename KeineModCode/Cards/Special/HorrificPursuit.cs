using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class HorrificPursuit : KeineModCard
{
    public HorrificPursuit() : base(1, CardType.Status, CardRarity.Status, TargetType.None)
    {
        WithKeyword(CardKeyword.Exhaust);
        WithTip(typeof(SandpitMarkPower));
    }

    public override int MaxUpgradeLevel => 0;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (var enemy in CombatState.HittableEnemies)
        {
            if (enemy.HasPower<SandpitMarkPower>() && enemy.GetPower<SandpitMarkPower>().Applier == Owner.Creature)
            {
                await PowerCmd.Decrement(enemy.GetPower<SandpitMarkPower>());
            }
        }
        await CardPileCmd.Draw(choiceContext, 1, Owner);
    }
}