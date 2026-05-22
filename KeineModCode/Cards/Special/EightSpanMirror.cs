using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class EightSpanMirror : KeineModCard
{
    public EightSpanMirror() : base(2, CardType.Power, CardRarity.Token, TargetType.Self)
    {
        WithVar("MirroredPower", 1);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<MirroredPower>(choiceContext, Owner.Creature, DynamicVars["MirroredPower"].BaseValue, Owner.Creature, this);
    }
}