using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Rare;

public class Ephemerality137 : KeineModCard, IOnConsumed
{
    public Ephemerality137() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithPower<EphemeralityPower>(1);
        WithKeywords(CardKeyword.Ethereal);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<EphemeralityPower>(choiceContext, CombatState.HittableEnemies, DynamicVars["EphemeralityPower"].BaseValue, Owner.Creature, this);
        await CardPileCmd.Add(this, PileType.Draw, IsUpgraded ? CardPilePosition.Top : CardPilePosition.Random);
    }

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard != this) return;
        await PowerCmd.Apply<EphemeralityPower>(choiceContext, CombatState.HittableEnemies, DynamicVars["EphemeralityPower"].BaseValue, Owner.Creature, this);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(CreateClone(), PileType.Draw, IsUpgraded ? CardPilePosition.Top : CardPilePosition.Random));
    }
}