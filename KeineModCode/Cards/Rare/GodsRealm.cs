using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace KeineMod.KeineModCode.Cards.Rare;

public class GodsRealm : KeineModCard
{
    public GodsRealm() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("GodsRealmPower", 2);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithTip(KeineKeywords.Consume);
        WithTip(KeineKeywords.Create);
        WithTip(StaticHoverTip.Block);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<GodsRealmPower>(choiceContext, Owner.Creature, DynamicVars["GodsRealmPower"].BaseValue, Owner.Creature, this);
    }
}