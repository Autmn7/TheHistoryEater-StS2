using KeineMod.KeineModCode.Cards.Special;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Scripts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace KeineMod.KeineModCode.Cards.Rare;

public class Reincarnation : KeineModCard
{
    private string _slayedBossIds = "";

    [SavedProperty]
    public string SlayedBossIds
    {
        get => _slayedBossIds;
        set
        {
            AssertMutable();
            _slayedBossIds = value;
            UpdateSlayedBossesUi();
        }
    }

    public Reincarnation() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(2000, 4);
        WithKeyword(CardKeyword.Retain, UpgradeType.Add);
        WithKeywords(KeineKeywords.Reincarnation, KeineKeywords.Create, CardKeyword.Exhaust);
        WithVar(new StringVar("SlayedBosses"));
        WithTip(typeof(ReturningBridge));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        var attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target).WithHitFx(tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        // Check if the target monster died from this specific processing frame
        if (cardPlay.Target.Monster != null && attackCommand.Results.SelectMany(r => r).Any(r => r.WasTargetKilled) && Owner.RunState.CurrentRoom is { RoomType: RoomType.Boss } && !cardPlay.Target.HasPower<MinionPower>())
        {
            // Track on the current card copy in play
            RecordBossSlay(cardPlay.Target.Monster);
            if (DeckVersion is Reincarnation deckVersion)
                deckVersion.RecordBossSlay(cardPlay.Target.Monster);
        }
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner || combatState.RoundNumber != 1 || !PileType.Deck.GetPile(player).Cards.Contains(DeckVersion) || string.IsNullOrEmpty(SlayedBossIds))
            return;

        if (DynamicVars.TryGetValue("SlayedBosses", out var v) && v is StringVar s && !string.IsNullOrEmpty(s.StringValue))
        {
            var returningBridge = CombatState.CreateCard<ReturningBridge>(Owner);
            returningBridge.SlayedBossIds = SlayedBossIds; 
            returningBridge.Reincarnation = s.StringValue;
            await CreateCmd.Execute(returningBridge, Owner);
        }
    }

    private void RecordBossSlay(MonsterModel boss)
    {
        var currentList = string.IsNullOrEmpty(SlayedBossIds) ? [] : SlayedBossIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        // Establish the baseline identifier key
        var bossKey = boss.Id.Entry;

        // Isolate phase data explicitly if the boss is the multi-stage TestSubject
        if (boss is TestSubject testSubject)
        {
            // Respawns tracks previous phase loops (0 = Phase 1, 1 = Phase 2, 2 = Phase 3)
            var phaseNumber = testSubject.Respawns + 1;
            bossKey = $"{boss.Id.Entry}_Phase{phaseNumber}";
        }


        // Save the language-neutral database entry key
        if (!currentList.Contains(bossKey))
        {
            currentList.Add(bossKey);
            SlayedBossIds = string.Join(",", currentList);
        }
    }

    private void UpdateSlayedBossesUi()
    {
        if (!DynamicVars.TryGetValue("SlayedBosses", out var v) || v is not StringVar s)
            return;
        if (string.IsNullOrEmpty(_slayedBossIds))
        {
            s.StringValue = "";
            return;
        }

        var localizedNames = _slayedBossIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(id =>
        {
            // Dynamically intercept custom Phase tracking keys for clean display text
            if (id.StartsWith(ModelDb.Monster<TestSubject>().Id.Entry + "_Phase"))
            {
                var baseName = MonsterModel.L10NMonsterLookup(ModelDb.Monster<TestSubject>().Id.Entry + ".name").GetFormattedText().Replace(" #C0", "");
                var phaseNum = id.Replace(ModelDb.Monster<TestSubject>().Id.Entry + "_Phase", "");
                return $"{baseName} #{phaseNum}";
            }

            // Standard baseline database lookup localization rule
            return MonsterModel.L10NMonsterLookup(id + ".name").GetFormattedText();
        });

        s.StringValue = string.Join(", ", localizedNames);
    }
}