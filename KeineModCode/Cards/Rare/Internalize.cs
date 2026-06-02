using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Rare;

public class Internalize : KeineModCard
{
    public Internalize() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("InternalizePower", 1);
        WithTip(KeineModKeywords.Consume);
        WithTip(typeof(ValorPower));
        WithTip(typeof(BenevolencePower));
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<InternalizePower>(choiceContext, Owner.Creature, DynamicVars["InternalizePower"].BaseValue, Owner.Creature, this);
    }
}