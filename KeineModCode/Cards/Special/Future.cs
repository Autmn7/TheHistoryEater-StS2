using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class Future : KeineModCard, ReturningBridge.IChoosable
{
    public Future() : base(-1, CardType.Status, CardRarity.Status, TargetType.None)
    {
        WithPower<KnowledgePower>(6);
        WithVar("DisintegrationPower", 6);
    }

    public override int MaxUpgradeLevel => 0;

    public override bool CanBeGeneratedInCombat => false;

    public async Task OnChosen()
    {
        await PowerCmd.Apply<KnowledgePower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<DisintegrationPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["DisintegrationPower"].BaseValue, Owner.Creature, this);
    }
}