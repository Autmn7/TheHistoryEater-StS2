using KeineMod.KeineModCode.UIs;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Rare;

public class GodsRealm : KeineModCard
{
    public GodsRealm() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithBlock(3, 1);
        WithKeywords(CardKeyword.Retain, CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (var card in ScrollPile.Scroll.GetPile(Owner).Cards.ToList())
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            CardCmd.Upgrade(card);
        }
    }
}