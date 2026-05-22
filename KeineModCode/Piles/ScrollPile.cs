using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace KeineMod.KeineModCode.Piles;

public class ScrollPile : CustomPile
{
    [CustomEnum] public static PileType Scroll;

    public ScrollPile() : base(Scroll)
    {
    }

    public override bool CardShouldBeVisible(CardModel card)
    {
        return false;
    }

    public override Vector2 GetTargetPosition(CardModel model, Vector2 size)
    {
        return (Vector2)NCombatRoom.Instance?.GetCreatureNode(RunManager.Instance.State?.Players.FirstOrDefault().Creature).VfxSpawnPosition;
    }
}