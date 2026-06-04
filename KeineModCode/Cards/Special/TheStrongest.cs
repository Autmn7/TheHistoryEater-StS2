using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class TheStrongest : KeineModCard
{
    public TheStrongest() : base(4, CardType.Power, CardRarity.Token, TargetType.Self)
    {
        WithVar("PerfectFreezePower", 99);
        WithCostUpgradeBy(-4);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<PerfectFreezePower>(choiceContext, Owner.Creature, DynamicVars["PerfectFreezePower"].BaseValue, Owner.Creature, this);
    }
}