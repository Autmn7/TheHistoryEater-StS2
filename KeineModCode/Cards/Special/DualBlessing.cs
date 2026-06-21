using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class DualBlessing : KeineModCard
{
    public DualBlessing() : base(0, CardType.Power, CardRarity.Event, TargetType.Self)
    {
        WithKeywords(CardKeyword.Innate);
        WithPower<KnowledgePower>(3);
        WithTip(KeineKeywords.Consume);
        WithPower<DupRekindlePower>(1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> options =
        [
            Owner.Creature.CombatState.CreateCard<KeinesBlessing>(Owner),
            Owner.Creature.CombatState.CreateCard<MokousBlessing>(Owner)
        ];
        if (IsUpgraded)
        {
            foreach (var card in options)
            {
                await ((IChoosable)card).OnChosen();
            }
        }
        else
        {
            var chosenCard = await CardSelectCmd.FromChooseACardScreen(choiceContext, options, Owner);
            await ((IChoosable)chosenCard).OnChosen();
        }
    }

    public interface IChoosable
    {
        Task OnChosen();
    }
}