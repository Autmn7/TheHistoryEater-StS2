using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class MoonShift : KeineModCard
{
    public MoonShift() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCalculatedVar("CalculatedEnergy", 0, (card, _) => KeineConstantsStateRegistry.Get(card.Owner).FullMoonCharge, 1);
        WithKeywords(CardKeyword.Retain, KeineModKeywords.Fullmoon, CardKeyword.Exhaust);
        WithEnergyTip();
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(((CalculatedVar)DynamicVars["CalculatedEnergy"]).Calculate(cardPlay.Target), Owner);
    }
}