using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace KeineMod.KeineModCode.Cards.Uncommon;

public class AncientProtection : KeineModCard
{
    public AncientProtection() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithVar("AncientProtectionPower", 3, 1);
        WithTip(KeineModKeywords.Consume);
        WithTip(StaticHoverTip.Block);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<AncientProtectionPower>(choiceContext, Owner.Creature, DynamicVars["AncientProtectionPower"].BaseValue, Owner.Creature, this);
    }
}