using KeineMod.KeineModCode.Powers.Reincarnation;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;

namespace KeineMod.KeineModCode.Commands;

public static class ReincarnationPowerCmd
{
    public static async Task ApplyDisplayPower(MonsterModel? monster)
    {
        if (monster == null) return;

        // 1. Determine the exact key variant based on monster type and phase
        var monsterKey = monster.Id.Entry;
        if (monster is TestSubject testSubject)
        {
            var phaseNumber = testSubject.Respawns + 1;
            monsterKey = $"{monster.Id.Entry}_Phase{phaseNumber}";
        }

        // 2. Evaluate the switch expression and await the resulting application Task
        var task = monsterKey switch
        {
            // ==================== ACT 1: OVERGROWTH ====================
            "CEREMONIAL_BEAST" =>
                PowerCmd.Apply<OvergrowthCeremonialBeastPower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            "KIN_PRIEST" =>
                PowerCmd.Apply<OvergrowthKinPriestPower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            "VANTOM" =>
                PowerCmd.Apply<OvergrowthVantomPower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            // ==================== ACT 1: UNDERDOCKS ====================
            "WATERFALL_GIANT" =>
                PowerCmd.Apply<UnderdocksWaterfallGiantPower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            "SOUL_FYSH" =>
                PowerCmd.Apply<UnderdocksSoulFyshPower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            "LAGAVULIN_MATRIARCH" =>
                PowerCmd.Apply<UnderdocksLagavulinMatriarchPower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            // ====================== ACT 2: HIVE ======================
            "KNOWLEDGE_DEMON" =>
                PowerCmd.Apply<HiveKnowledgeDemonPower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            "CRUSHER" =>
                PowerCmd.Apply<HiveCrusherPower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            "ROCKET" =>
                PowerCmd.Apply<HiveRocketPower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            "THE_INSATIABLE" =>
                PowerCmd.Apply<HiveTheInsatiablePower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            // ====================== ACT 3: GLORY ======================
            "QUEEN" =>
                PowerCmd.Apply<GloryQueenPower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            "AEONGLASS" =>
                PowerCmd.Apply<GloryAeonglassPower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            "TEST_SUBJECT_Phase1" =>
                PowerCmd.Apply<GloryTestSubjectOnePower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            "TEST_SUBJECT_Phase2" =>
                PowerCmd.Apply<GloryTestSubjectTwoPower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            "TEST_SUBJECT_Phase3" =>
                PowerCmd.Apply<GloryTestSubjectThreePower>(new ThrowingPlayerChoiceContext(), monster.Creature, 1, null, null),

            // Default fallback if an unexpected monster ID is encountered
            _ => Task.CompletedTask
        };
    }
}