using KeineMod.KeineModCode.Extensions;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Relics;

public class CursedKimono : KeineModRelic, IOnConsumed
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2M, ValueProp.Unblockable | ValueProp.Unpowered)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(KeineKeywords.Consume)];

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard.Owner != Owner || Owner.Creature.Side != Owner.Creature.CombatState?.CurrentSide)
            return;
        Flash();
        SfxCmd.Play("burn.wav".SoundEffectPath());
        foreach (var enemy in Owner.Creature.CombatState.HittableEnemies)
            NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NFireBurstVfx.Create(enemy, 0.75f));
        await CreatureCmd.Damage(choiceContext, Owner.Creature.CombatState.HittableEnemies, DynamicVars.Damage, Owner.Creature);
    }
}