using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace KeineMod.KeineModCode.Cards.Rare;

public class YamatoKingdom : KeineModCard
{
    public YamatoKingdom() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithBlock(5);
        WithTip(typeof(KnowledgePower));
        WithKeywords(KeineKeywords.Knowledgeable, CardKeyword.Retain, CardKeyword.Exhaust);
    }

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var xValue = ResolveEnergyXValue();
        if (IsUpgraded)
            xValue++;
        for (var i = 0; i < xValue; ++i)
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, xValue, Owner.Creature, this);
    }
}