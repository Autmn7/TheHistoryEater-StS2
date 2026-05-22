using BaseLib.Utils;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Common;

public class ThreeSacredTreasuresOrb : KeineModCard
{
    public ThreeSacredTreasuresOrb() : base(1, CardType.Attack, CardRarity.Common, TargetType.Self)
    {
        WithBlock(7, 2);
        WithPower<BenevolencePower>(2, 1);
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<ScrollOfBenevolence>(card.IsUpgraded)));
        WithTip(typeof(CurvedJewel));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<BenevolencePower>(choiceContext, Owner.Creature, DynamicVars["BenevolencePower"].BaseValue, Owner.Creature, this);
        CardModel created = CombatState.CreateCard<ScrollOfBenevolence>(Owner);
        await CreateCmd.Execute(created, Owner, IsUpgraded);
    }
}