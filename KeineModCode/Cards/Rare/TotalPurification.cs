using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace KeineMod.KeineModCode.Cards.Rare;

public class TotalPurification : KeineModCard
{
    public TotalPurification() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithKeywords(KeineModKeywords.Human, KeineModKeywords.Consume, KeineModKeywords.Hakutaku, CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (InHuman())
        {
            var hand = PileType.Hand.GetPile(Owner).Cards.ToList();
            var draw = PileType.Draw.GetPile(Owner).Cards.ToList();
            var discard = PileType.Discard.GetPile(Owner).Cards.ToList();
            foreach (var card in hand)
                if (card is { Type: CardType.Status or CardType.Curse })
                    await ConsumeCmd.SpecificCard(choiceContext, card, Owner, this);
            foreach (var card in draw)
                if (card is { Type: CardType.Status or CardType.Curse })
                    await ConsumeCmd.SpecificCard(choiceContext, card, Owner, this);
            foreach (var card in discard)
                if (card is { Type: CardType.Status or CardType.Curse })
                    await ConsumeCmd.SpecificCard(choiceContext, card, Owner, this);
        }

        if (InHakutaku())
            foreach (var power in Owner.Creature.Powers.ToList().Where(power => power.Type == PowerType.Debuff || (power is StrengthPower or DexterityPower or FocusPower && power.Amount < 0)))
                await PowerCmd.Remove(power);
    }
}