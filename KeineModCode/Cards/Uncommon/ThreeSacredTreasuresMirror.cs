using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class ThreeSacredTreasuresMirror : KeineModCard
{
    public ThreeSacredTreasuresMirror() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithEnergy(1);
        WithCards(1, 1);
        WithKeyword(KeineKeywords.Create);
        WithTip(typeof(EightSpanMirror));
        WithTip(typeof(ScrollOfWisdom));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<TreasureOfWisdomPower>(choiceContext, Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature, this);
        for (var i = 0; i < DynamicVars.Cards.IntValue; ++i)
        {
            CardModel created = CombatState.CreateCard<ScrollOfWisdom>(Owner);
            await CreateCmd.Execute(choiceContext, created, Owner);
        }
    }
}