using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Powers;

public class StackOverflowPower : KeineModPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power.Owner == Owner && Owner.GetPowerAmount<KnowledgePower>() >= Amount)
        {
            SfxCmd.Play("event:/sfx/characters/defect/defect_lightning_evoke");
            VfxCmd.PlayOnCreature(Owner, "vfx/vfx_attack_lightning");
            await PowerCmd.Remove(this);
            await CreatureCmd.Kill(Owner);
        }
    }
}