using BaseLib.Utils;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class ThreeSacredTreasuresOrb : KeineModCard
{
    public ThreeSacredTreasuresOrb() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(14, 4);
        WithVar("TreasureOfBenevolencePower", 1);
        WithKeyword(KeineKeywords.Create);
        WithTip(typeof(CurvedJewel));
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<ScrollOfBenevolence>(card.IsUpgraded)));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<TreasureOfBenevolencePower>(choiceContext, Owner.Creature, DynamicVars["TreasureOfBenevolencePower"].BaseValue, Owner.Creature, this);
        CardModel created = CombatState.CreateCard<ScrollOfBenevolence>(Owner);
        await CreateCmd.Execute(choiceContext, created, Owner, IsUpgraded);
    }
}