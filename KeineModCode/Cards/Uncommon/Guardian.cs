using BaseLib.Utils;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class Guardian : KeineModCard
{
    public Guardian() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
    {
        WithBlock(6, 3);
        WithKeywords(KeineKeywords.Knowledgeable, KeineKeywords.Hakutaku);
        WithTip(new TooltipSource(card => new HoverTip(new LocString("cards", Id.Entry + ".extraTipTitle"), new LocString("cards", Id.Entry + ".extraTipDescription"))));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (var creature in CombatState.GetTeammatesOf(Owner.Creature).Where(c => c.IsAlive && c.IsPlayer))
        {
            var isMokou = creature.Player?.Character.Id.ToString() == "CHARACTER.MOKOUMOD-MOKOU_MOD" && creature != Owner.Creature;
            await CreatureCmd.GainBlock(creature, isMokou ? DynamicVars.Block.BaseValue * 2 : DynamicVars.Block.BaseValue, ValueProp.Move, cardPlay);
            if (InHakutaku())
                await PowerCmd.Apply<BlurPower>(choiceContext, creature, isMokou ? 2 : 1, Owner.Creature, this);
        }
    }
}