using UnityEngine;

/// <summary>
/// CONCERN: joining the other scripts together.
///
/// This is the whole game loop, and it is short because every step is somebody
/// else's job: the dragger reports a drop, the turn manager says whether that
/// player may move, the rules approve the move, the executor commits it and
/// the turn manager closes the turn.
///
/// If a drop is rejected for any reason the piece simply snaps back.
/// </summary>
[RequireComponent(typeof(PieceDragger), typeof(MoveRules), typeof(MoveExecutor))]
[RequireComponent(typeof(TurnManager))]
public class MoveCoordinator : MonoBehaviour
{
    PieceDragger dragger;
    MoveRules rules;
    MoveExecutor executor;
    TurnManager turns;

    void Awake()
    {
        dragger = GetComponent<PieceDragger>();
        rules = GetComponent<MoveRules>();
        executor = GetComponent<MoveExecutor>();
        turns = GetComponent<TurnManager>();
    }

    void OnEnable() => dragger.PieceDropped += OnPieceDropped;
    void OnDisable() => dragger.PieceDropped -= OnPieceDropped;

    void OnPieceDropped(Piece piece, Vector2Int target)
    {
        if (!turns.CanPlay(piece))
        {
            turns.Reject(turns.LockedPiece != null
                ? "You must keep jumping with the same piece"
                : $"{turns.CurrentPlayer} to move - that is not your piece");
            piece.SnapToCell();
            return;
        }

        bool mustJump = turns.LockedPiece != null;

        if (!rules.TryBuildMove(piece, target, out Move move) || (mustJump && !move.IsJump))
        {
            piece.SnapToCell();
            return;
        }

        bool promoted = executor.Apply(piece, move);

        // A jumper that can jump again keeps the turn - unless it was just crowned.
        if (move.IsJump && !promoted && rules.HasJumpAvailable(piece))
            turns.ContinueChain(piece);
        else
            turns.EndTurn();
    }
}
