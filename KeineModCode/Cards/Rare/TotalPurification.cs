using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers;
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
        WithKeywords(CardKeyword.Retain, KeineKeywords.Human, KeineKeywords.Consume, KeineKeywords.Hakutaku, CardKeyword.Exhaust);
        WithTip(typeof(KnowledgePower));
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var count = 0;
        if (InHuman())
        {
            var hand = PileType.Hand.GetPile(Owner).Cards.ToList();
            var draw = PileType.Draw.GetPile(Owner).Cards.ToList();
            var discard = PileType.Discard.GetPile(Owner).Cards.ToList();
            foreach (var card in hand)
                if (card is { Type: CardType.Status or CardType.Curse })
                {
                    count++;
                    await ConsumeCmd.SpecificCard(choiceContext, card, Owner, this);
                }
            foreach (var card in draw)
                if (card is { Type: CardType.Status or CardType.Curse })
                {
                    count++;
                    await ConsumeCmd.SpecificCard(choiceContext, card, Owner, this);
                }
            foreach (var card in discard)
                if (card is { Type: CardType.Status or CardType.Curse })
                {
                    count++;
                    await ConsumeCmd.SpecificCard(choiceContext, card, Owner, this);
                }
        }

        if (InHakutaku())
            foreach (var power in Owner.Creature.Powers.ToList().Where(power => power.Type == PowerType.Debuff || (power is StrengthPower or DexterityPower or FocusPower && power.Amount < 0)))
            {
                count++;
                await PowerCmd.Remove(power);
            }

        if (count > 0)
            await PowerCmd.Apply<KnowledgePower>(choiceContext, Owner.Creature, count, Owner.Creature, this);
    }
}