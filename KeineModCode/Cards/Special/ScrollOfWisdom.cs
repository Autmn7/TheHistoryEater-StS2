using BaseLib.Utils;
using KeineMod.KeineModCode.Powers;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class ScrollOfWisdom : KeineModCard, IOnConsumed
{
    public ScrollOfWisdom() : base(-1, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithPower<WisdomPower>(1, 1);
        WithKeywords(KeineKeywords.Sacredscroll, CardKeyword.Unplayable);
        WithTip(KeineKeywords.Consume);
        WithTip(typeof(EightSpanMirror));
        WithTags(KeineTags.Sacred);
    }

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel consumedCard)
    {
        if (consumedCard != this) return;
        await PowerCmd.Apply<WisdomPower>(choiceContext, Owner.Creature, DynamicVars["WisdomPower"].BaseValue, Owner.Creature, this);
        CardModel mirror = CombatState.CreateCard<EightSpanMirror>(Owner);
        await CardCmd.Transform(this, mirror, CardPreviewStyle.None);
    }
}