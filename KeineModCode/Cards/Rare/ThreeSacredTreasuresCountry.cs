using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Rare;

public class ThreeSacredTreasuresCountry : KeineModCard
{
    public ThreeSacredTreasuresCountry() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("TreasureOfCountryPower", 1);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithKeywords(KeineModKeywords.Create, KeineModKeywords.Sacredpower);
        WithTip(typeof(ScrollOfValor));
        WithTip(typeof(ScrollOfBenevolence));
        WithTip(typeof(ScrollOfWisdom));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<TreasureOfCountryPower>(choiceContext, Owner.Creature, DynamicVars["TreasureOfCountryPower"].BaseValue, Owner.Creature, this);
        List<CardModel> treasures =
        [
            CombatState.CreateCard<ScrollOfValor>(Owner),
            CombatState.CreateCard<ScrollOfBenevolence>(Owner),
            CombatState.CreateCard<ScrollOfWisdom>(Owner)
        ];
        foreach (var card in treasures) await CreateCmd.Execute(card, Owner);
    }
}