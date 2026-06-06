using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Rare;

public class Erudite : KeineModCard
{
    public Erudite() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVar("EruditePower", 3, 1);
        WithPower<KnowledgePower>(3, 1);
        WithCards(3, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<EruditePower>(choiceContext, Owner.Creature, DynamicVars["EruditePower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }
}