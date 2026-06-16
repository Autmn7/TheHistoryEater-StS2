using BaseLib.Utils;
using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace KeineMod.KeineModCode.Cards.Rare;

public class Deconstruction : KeineModCard
{
    public int CountNeeded;

    public Deconstruction() : base(0, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
    {
        CountNeeded = 4;
        WithCards(1);
        WithVar(new CountNeededVar(4));
        WithKeywords(KeineKeywords.Human, KeineKeywords.Consume);
        WithTip(new TooltipSource(card => HoverTipFactory.FromCard<Reconstruction>(card.IsUpgraded)));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CountNeeded > 0) CountNeeded--;
        if (InHuman())
            await ConsumeCmd.FromHand(choiceContext, Owner, DynamicVars.Cards.IntValue, this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        if (!Keywords.Contains(CardKeyword.Exhaust))
            await CardPileCmd.Add(this, PileType.Draw, CardPilePosition.Random);
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this)
            return;
        if (CountNeeded <= 0)
        {
            CardModel reconstruction = CombatState.CreateCard<Reconstruction>(Owner);
            if (IsUpgraded)
                CardCmd.Upgrade(reconstruction);
            await CardCmd.Transform(this, reconstruction);
        }
    }
}