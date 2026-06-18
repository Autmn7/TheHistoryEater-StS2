using KeineMod.KeineModCode.UIs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class Detachment : KeineModCard
{
    public Detachment() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithVar("Ratio", 4, -1);
        WithCalculatedVar("CalculatedCount", 0, (card, _) => (int)Math.Floor(ScrollPile.Scroll.GetPile(card.Owner).Cards.ToList().Count / card.DynamicVars["Ratio"].BaseValue));
        WithKeyword(CardKeyword.Exhaust);
        WithEnergyTip();
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var energy = (int)((CalculatedVar)DynamicVars["CalculatedCount"]).Calculate(cardPlay.Target);
        var list = ScrollPile.Scroll.GetPile(Owner).Cards.ToList();
        foreach (var card in list)
            await CardCmd.Exhaust(choiceContext, card);
        if (energy > 0)
        {
            await PlayerCmd.GainEnergy(energy, Owner);
            await CardPileCmd.Draw(choiceContext, energy, Owner);
        }
    }
}