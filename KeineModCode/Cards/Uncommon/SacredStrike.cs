using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using KeineMod.KeineModCode.UIs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class SacredStrike : KeineModCard
{
    public SacredStrike() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(4, 2);
        WithCalculatedVar("Repeat", 0, (card, _) => PileType.Hand.GetPile(card.Owner).Cards.Concat(ScrollPile.Scroll.GetPile(card.Owner).Cards).Count(c => c.Tags.Contains(KeineTags.Sacred)));
        WithKeyword(KeineKeywords.Create);
        WithTip(KeineKeywords.Sacredscroll);
        WithTip(typeof(ScrollOfValor));
        WithTip(typeof(HeavenlySword));
        WithTags(CardTag.Strike);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel created = CombatState.CreateCard<ScrollOfValor>(Owner);
        await CreateCmd.Execute(created, Owner);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitCount((int)((CalculatedVar)DynamicVars["Repeat"]).Calculate(cardPlay.Target)).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }
}