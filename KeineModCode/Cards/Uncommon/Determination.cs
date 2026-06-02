using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class Determination : KeineModCard
{
    public Determination() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(12, 3);
        WithPower<ValorPower>(3, 2);
        WithCards(2, 1);
        WithKeywords(KeineModKeywords.Human, KeineModKeywords.Consume, KeineModKeywords.Hakutaku);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<ValorPower>(choiceContext, Owner.Creature, DynamicVars["ValorPower"].BaseValue, Owner.Creature, this);
        if (InHuman())
            await ConsumeCmd.FromHandUpTo(choiceContext, Owner, DynamicVars.Cards.IntValue, this);
        if (InHakutaku())
        {
            var list = (await CardSelectCmd.FromHand(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, DynamicVars.Cards.IntValue), null, this)).ToList();
            foreach (var card in list)
                CardCmd.Upgrade(card);
        }
    }
}