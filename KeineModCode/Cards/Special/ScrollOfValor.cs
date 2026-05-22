using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class ScrollOfValor : KeineModCard, IOnConsumed
{
    public ScrollOfValor() : base(-1, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithPower<ValorPower>(3, 2);
        WithKeywords(CardKeyword.Unplayable);
        WithTip(KeineModKeywords.Consume);
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<HeavenlySword>()));
    }

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel card)
    {
        if (card != this)
            return;
        await PowerCmd.Apply<ValorPower>(choiceContext, Owner.Creature, DynamicVars["ValorPower"].BaseValue, Owner.Creature, this);
        CardModel sword = CombatState.CreateCard<HeavenlySword>(Owner);
        await CardCmd.Transform(this, sword, CardPreviewStyle.None);
    }
}