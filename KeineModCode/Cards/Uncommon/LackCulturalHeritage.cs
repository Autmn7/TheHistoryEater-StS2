using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class LackCulturalHeritage : KeineModCard
{
    public LackCulturalHeritage() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
    {
        WithVar("EmotionalDamage", 3);
        WithPower<KnowledgePower>(2, 1);
        WithTip(typeof(ForeignKnowledge));
        WithTip(new TooltipSource(card => new HoverTip(new LocString("cards", Id.Entry + ".extraTipTitle"), new LocString("cards", Id.Entry + ".extraTipDescription"))));
    }

    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target?.Player == null)
            return;
        if (!cardPlay.Target.HasPower<KnowledgePower>())
            await CreatureCmd.Damage(choiceContext, cardPlay.Target, DynamicVars["EmotionalDamage"].BaseValue, ValueProp.Unpowered, this, cardPlay);
        await PowerCmd.Apply<KnowledgePower>(choiceContext, cardPlay.Target, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
        var card = CombatState.CreateCard<ForeignKnowledge>(cardPlay.Target.Player);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
    }
    
    public override decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver, decimal amount, Creature? target, CardModel? cardSource)
    {
        if (cardSource == this && power is KnowledgePower)
            return target?.Player?.Character.Id.ToString() == "CHARACTER.MOKOUMOD-MOKOU_MOD" ? 1M : 0M;

        return 0M;
    }
}