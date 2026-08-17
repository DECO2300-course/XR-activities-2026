using UnityEngine;

/// <summary>
/// CONCERN: making a legal move actually happen.
///
/// Takes a Move that MoveRules has already approved and commits it: the board
/// state is updated, the jumped piece is destroyed, the mover is crowned if it
/// earned it. It never decides anything - it only carries out the decision.
/// </summary>
[RequireComponent(typeof(BoardState), typeof(MoveRules))]
public class MoveExecutor : MonoBehaviour
{
    BoardState state;
    MoveRules rules;

    /// <summary>Raised after a move is committed - handy for sounds, scoring or logging.</summary>
    public event System.Action<Piece, Move> MoveApplied;

    void Awake()
    {
        state = GetComponent<BoardState>();
        rules = GetComponent<MoveRules>();
    }

    /// <returns>True if the piece was crowned by this move.</returns>
    public bool Apply(Piece piece, Move move)
    {
        if (move.IsJump)
        {
            Piece victim = state.Get(move.Captured);
            state.Remove(move.Captured);
            if (victim != null) Destroy(victim.gameObject);
        }

        state.MovePiece(move.From, move.To);
        piece.SnapToCell();

        bool promoted = rules.ShouldPromote(piece);
        if (promoted) piece.Promote();

        MoveApplied?.Invoke(piece, move);
        return promoted;
    }
}
