using BaseLib.Utils;
using KeineMod.KeineModCode.Commands;
using KeineMod.KeineModCode.Powers.Reincarnation;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace KeineMod.KeineModCode.Cards.Special;

[Pool(typeof(TokenCardPool))]
public class ReturningBridge : KeineModCard
{
    private string _slayedBossIds = "";
    private string _reincarnation = "";
    
    public ReturningBridge() : base(1, CardType.Power, CardRarity.Token, TargetType.Self)
    {
        WithVar(new StringVar("Reincarnation"));
        WithCostUpgradeBy(-1);
    }

    // Dynamically shift the card's targeting behavior based on what effects are loaded
    public override TargetType TargetType => !string.IsNullOrEmpty(_slayedBossIds) && (_slayedBossIds.Contains("CRUSHER") || _slayedBossIds.Contains("ROCKET") || _slayedBossIds.Contains("THE_INSATIABLE")) ? TargetType.AnyEnemy : TargetType.Self;

    public string SlayedBossIds
    {
        get => _slayedBossIds;
        set
        {
            _slayedBossIds = value;
            UpdatePowerTooltips();
        }
    }

    public string Reincarnation
    {
        get => _reincarnation;
        set
        {
            _reincarnation = value;
            if (DynamicVars.TryGetValue("Reincarnation", out var v) && v is StringVar s)
                s.StringValue = _reincarnation;
        }
    }

    private void UpdatePowerTooltips()
    {
        if (string.IsNullOrEmpty(_slayedBossIds)) return;

        var ids = _slayedBossIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var id in ids)
        {
            Type? powerType = id switch
            {
                "CEREMONIAL_BEAST" => typeof(OvergrowthCeremonialBeastPower),
                "KIN_PRIEST" => typeof(OvergrowthKinPriestPower),
                "VANTOM" => typeof(OvergrowthVantomPower),
                "WATERFALL_GIANT" => typeof(UnderdocksWaterfallGiantPower),
                "SOUL_FYSH" => typeof(UnderdocksSoulFyshPower),
                "LAGAVULIN_MATRIARCH" => typeof(UnderdocksLagavulinMatriarchPower),
                "KNOWLEDGE_DEMON" => typeof(HiveKnowledgeDemonPower),
                "CRUSHER" => typeof(HiveCrusherPower),
                "ROCKET" => typeof(HiveRocketPower),
                "THE_INSATIABLE" => typeof(HiveTheInsatiablePower),
                "QUEEN" => typeof(GloryQueenPower),
                "AEONGLASS" => typeof(GloryAeonglassPower),
                "TEST_SUBJECT_Phase1" => typeof(GloryTestSubjectOnePower),
                "TEST_SUBJECT_Phase2" => typeof(GloryTestSubjectTwoPower),
                "TEST_SUBJECT_Phase3" => typeof(GloryTestSubjectThreePower),
                _ => null
            };

            if (powerType != null)
            {
                WithTip(powerType);
            }
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (string.IsNullOrEmpty(_slayedBossIds)) return;

        var ids = _slayedBossIds.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var id in ids)
        {
            switch (id)
            {
                // ============================================================
                // ACT 1: OVERGROWTH
                // ============================================================
                
                case "CEREMONIAL_BEAST":
                    // RE: Ceremonial Beast [Divine Cry]
                    // Effect: Take an extra turn after this one where you can only play 1 card.
                    await Task.CompletedTask; 
                    break;

                case "KIN_PRIEST":
                    // RE: Kin Priest [Orb of Feebleness]
                    // Effect: Deal 10 damage, apply 2 Frail and Weak to ALL enemies.
                    await DamageCmd.Attack(10).FromCard(this).TargetingAllOpponents(CombatState).Execute(choiceContext);
                    await PowerCmd.Apply<FrailPower>(choiceContext, CombatState.HittableEnemies, 2, Owner.Creature, this);
                    await PowerCmd.Apply<WeakPower>(choiceContext, CombatState.HittableEnemies, 2, Owner.Creature, this);
                    break;

                case "VANTOM":
                    // RE: Vantom [Deformable Ectosymbiont]
                    // Effect: Gain 1 Slippery.
                    await PowerCmd.Apply<SlipperyPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
                    break;

                // ============================================================
                // ACT 1: UNDERDOCKS
                // ============================================================
                
                case "WATERFALL_GIANT":
                    // RE: Waterfall Giant [Healing Hot Spring]
                    // Effect: Heal 5 HP.
                    await CreatureCmd.Heal(Owner.Creature, 5);
                    break;

                case "SOUL_FYSH":
                    // RE: Soul Fysh [Optical Camouflage]
                    // Effect: Gain 1 Intangible. Create a Beckon.
                    await PowerCmd.Apply<IntangiblePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
                    var beckonCard = CombatState.CreateCard<Beckon>(Owner);
                    await CreateCmd.Execute(beckonCard, Owner);
                    break;

                case "LAGAVULIN_MATRIARCH":
                    // RE: Lagavulin Matriarch [Soul Siphon]
                    // Effect: ALL enemies lose 2 Strength. Gain 2 Strength.
                    await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, 2, Owner.Creature, this);
                    await PowerCmd.Apply<StrengthPower>(choiceContext, CombatState.HittableEnemies, -2, Owner.Creature, this);
                    break;

                // ============================================================
                // ACT 2: HIVE
                // ============================================================
                
                case "KNOWLEDGE_DEMON":
                    // RE: Knowledge Demon [Curse of Knowledge]
                    // Effect: Choose one: Gain 3 Knowledge; or Gain 6 Knowledge and 6 Disintegration.
                    List<CardModel> options =
                    [
                        Owner.Creature.CombatState.CreateCard<Past>(Owner),
                        Owner.Creature.CombatState.CreateCard<Future>(Owner)
                    ];
                    var chosenCard = await CardSelectCmd.FromChooseACardScreen(choiceContext, options, Owner);
                    await ((IChoosable)chosenCard).OnChosen();
                    break;

                case "CRUSHER":
                    // RE: Crusher [Guarded Strike]
                    // Effect: Gain 15 Block. Deal 15 damage. Gain 2 Strength.
                    await CreatureCmd.GainBlock(Owner.Creature, 15, ValueProp.Move, cardPlay);
                    if (cardPlay.Target != null)
                    {
                        await DamageCmd.Attack(15).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext);
                    }
                    await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, 2, Owner.Creature, this);
                    break;

                case "ROCKET":
                    // RE: Rocket [Piercing Laser]
                    // Effect: Deal 35 damage.
                    if (cardPlay.Target != null)
                    {
                        await DamageCmd.Attack(35).FromCard(this).Targeting(cardPlay.Target).Execute(choiceContext);
                    }
                    break;

                case "THE_INSATIABLE":
                    // RE: The Insatiable [Insatiable Hunger]
                    // Effect: Mark an enemy. Create 6 Approaching Terrors to your Discard Pile. 
                    // After playing all of them, marked enemy instantly dies (if it is a Boss, loses 100 HP instead).
                    await Task.CompletedTask;
                    break;

                // ============================================================
                // ACT 3: GLORY
                // ============================================================
                
                case "QUEEN":
                    // RE: Queen [You're Mine]
                    // Effect: Apply 99 Frail, Weak and Vulnerable to ALL enemies.
                    await PowerCmd.Apply<FrailPower>(choiceContext, CombatState.HittableEnemies, 99, Owner.Creature, this);
                    await PowerCmd.Apply<WeakPower>(choiceContext, CombatState.HittableEnemies, 99, Owner.Creature, this);
                    await PowerCmd.Apply<VulnerablePower>(choiceContext, CombatState.HittableEnemies, 99, Owner.Creature, this);
                    break;

                case "AEONGLASS":
                    // RE: Aeonglass [Sands of Time]
                    // Effect: Gain 3 Artifact. At the start of your turn, Time Shift 6.
                    await PowerCmd.Apply<ArtifactPower>(choiceContext, Owner.Creature, 3, Owner.Creature, this);
                    break;

                case "TEST_SUBJECT_Phase1":
                    // RE: Test Subject Phase #1 [Experimental Rage]
                    // Effect: Gain 1 Enrage this turn.
                    await PowerCmd.Apply<EnragePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
                    break;

                case "TEST_SUBJECT_Phase2":
                    // RE: Test Subject Phase #2 [Experimental Stabs]
                    // Effect: Whenever your Attacks deal unblocked damage, apply 1 Vulnerable, Weak, or Historical Gap at random.
                    await Task.CompletedTask;
                    break;

                case "TEST_SUBJECT_Phase3":
                    // RE: Test Subject Phase #3 [Experimental Nemesis]
                    // Effect: Gain 1 Intangible now and every 3 turns.
                    await PowerCmd.Apply<IntangiblePower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
                    break;

                default:
                    break;
            }
        }
    }

    public interface IChoosable
    {
        Task OnChosen();
    }
}