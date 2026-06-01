using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class FirstPyramid : KeineModCard, IOnConsumed
{
    public FirstPyramid() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<KnowledgePower>(2);
        WithPower<WisdomPower>(1, 1);
        WithKeywords(CardKeyword.Innate, CardKeyword.Exhaust);
        WithKeyword(CardKeyword.Ethereal, UpgradeType.Remove);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<WisdomPower>(choiceContext, Owner.Creature, DynamicVars["WisdomPower"].BaseValue, Owner.Creature, this);
    }

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard == this)
            await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
    }
}