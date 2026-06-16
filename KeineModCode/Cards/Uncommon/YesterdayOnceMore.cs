using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class YesterdayOnceMore : KeineModCard
{
    public YesterdayOnceMore() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(1);
        WithKeywords(KeineKeywords.Recall, CardKeyword.Exhaust);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await RecallCmd.FromScroll(choiceContext, Owner, DynamicVars.Cards.IntValue);
        await PowerCmd.Apply<YesterdayOnceMorePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
    }
}