using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Common;

public class BorrowedTreasures : KeineModCard
{
    public BorrowedTreasures() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(9, 1);
        WithKeywords(KeineKeywords.Create);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
        WithTip(KeineKeywords.Sacredscroll);
        WithTip(typeof(ScrollOfValor));
        WithTip(typeof(ScrollOfBenevolence));
        WithTip(typeof(ScrollOfWisdom));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        if (!CombatManager.Instance.IsOverOrEnding)
        {
            List<CardModel> treasures =
            [
                CombatState.CreateCard<ScrollOfValor>(Owner),
                CombatState.CreateCard<ScrollOfBenevolence>(Owner),
                CombatState.CreateCard<ScrollOfWisdom>(Owner)
            ];
            var created = await CardSelectCmd.FromChooseACardScreen(choiceContext, treasures, Owner, true);
            await CreateCmd.Execute(created, Owner);
        }
    }
}