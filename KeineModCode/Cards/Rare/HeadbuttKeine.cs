using Godot;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace KeineMod.KeineModCode.Cards.Rare;

public class HeadbuttKeine : KeineModCard
{
    public HeadbuttKeine() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(30, 6);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithKeywords(KeineKeywords.Hakutaku, CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creatureNode = NCombatRoom.Instance?.GetCreatureNode(cardPlay.Target);
        if (creatureNode != null)
        {
            var child = NLargeMagicMissileVfx.Create(creatureNode.GetBottomOfHitbox(), new Color("50b598"));
            NCombatRoom.Instance.CombatVfxContainer.AddChildSafely((Node)child);
            await Cmd.Wait(child.WaitTime);
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx(tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        if (InHakutaku())
            await CreatureCmd.Stun(cardPlay.Target);
    }
}