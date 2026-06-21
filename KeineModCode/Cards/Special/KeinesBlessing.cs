using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class KeinesBlessing : KeineModCard, DualBlessing.IChoosable
{
    public KeinesBlessing() : base(-1, CardType.Power, CardRarity.Event, TargetType.None)
    {
        WithPower<KnowledgePower>(3);
    }

    public override int MaxUpgradeLevel => 0;

    public override bool CanBeGeneratedInCombat => false;

    public async Task OnChosen()
    {
        await PowerCmd.Apply<KnowledgePower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["KnowledgePower"].BaseValue, Owner.Creature, this);
    }
}