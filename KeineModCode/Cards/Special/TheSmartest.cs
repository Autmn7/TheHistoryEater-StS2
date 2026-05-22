using BaseLib.Utils;
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
public class TheSmartest : KeineModCard, IOnConsumed
{
    public TheSmartest() : base(9, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithVar("CostReduction", 3, 6);
        WithTip(KeineModKeywords.Consume);
        WithTip(typeof(TheStrongest));
    }

    public async Task OnConsumed(PlayerChoiceContext choiceContext, Player player, CardModel card)
    {
        if (card != this)
            return;
        if (EnergyCost.GetResolved() <= 0)
        {
            CardModel strongest = CombatState.CreateCard<TheStrongest>(Owner);
            await CardCmd.Transform(this, strongest, CardPreviewStyle.None);
        }
        else
        {
            EnergyCost.AddThisCombat(-DynamicVars["CostReduction"].IntValue);
        }
    }
}