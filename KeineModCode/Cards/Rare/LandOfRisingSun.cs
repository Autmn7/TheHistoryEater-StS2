using Godot;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace KeineMod.KeineModCode.Cards.Rare;

public class LandOfRisingSun : KeineModCard
{
    public LandOfRisingSun() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(40, 10);
        WithVar("SunrisePower", 2);
        WithKeywords(KeineModKeywords.Hakutaku, KeineModKeywords.Recall);
    }

    protected override bool ShouldGlowRedInternal => !InHakutaku();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (InHakutaku())
        {
            NGrandFinaleVfx child = NGrandFinaleVfx.Create(Owner.Creature);
            if (child != null)
            {
                NCombatRoom instance = NCombatRoom.Instance;
                if (instance != null)
                    instance.CombatVfxContainer.AddChildSafely((Node) child);
                await Cmd.Wait(NGrandFinaleVfx.totalAnticipationDuration);
            }
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(CombatState).WithHitVfxNode((Func<Creature, Node2D>)(t => (Node2D)NFireBurstVfx.Create(t, 1.0f))).Execute(choiceContext);
            await StanceCmd.ExitStance(choiceContext, Owner, this);
            await PowerCmd.Apply<SunrisePower>(choiceContext, Owner.Creature, DynamicVars["SunrisePower"].BaseValue, Owner.Creature, this);
        }
    }
}