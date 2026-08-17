using UnityEngine;

/// <summary>
/// CONCERN: the rules of checkers.
///
/// Pure questions and answers - "is this move legal?", "can this piece jump
/// again?", "should this piece be crowned?". It changes nothing, so you can
/// call it as often as you like (and unit test it without a mouse).
/// </summary>
[RequireComponent(typeof(BoardSettings), typeof(BoardState))]
public class MoveRules : MonoBehaviour
{
    BoardSettings settings;
    BoardState state;

    void Awake()
    {
        settings = GetComponent<BoardSettings>();
        state = GetComponent<BoardState>();
    }

    /// <summary>Turns "this piece was dropped on that square" into a legal Move, or fails.</summary>
    public bool TryBuildMove(Piece piece, Vector2Int target, out Move move)
    {
        move = default;

        if (!settings.InBounds(target) || !settings.IsPlayable(target)) return false;
        if (!state.IsEmpty(target)) return false;

        int forward = piece.Owner.ForwardRow();
        int stepCol = target.x - piece.Cell.x;
        int stepRow = target.y - piece.Cell.y;

        bool slide = Mathf.Abs(stepCol) == 1 &&
                     (stepRow == forward || (piece.IsKing && stepRow == -forward));

        bool jump = Mathf.Abs(stepCol) == 2 &&
                    (stepRow == 2 * forward || (piece.IsKing && stepRow == -2 * forward));

        if (slide)
        {
            move = new Move { From = piece.Cell, To = target, IsJump = false };
            return true;
        }

        if (jump)
        {
            var over = new Vector2Int((piece.Cell.x + target.x) / 2, (piece.Cell.y + target.y) / 2);
            Piece victim = state.Get(over);
            if (victim == null || victim.Owner == piece.Owner) return false;

            move = new Move { From = piece.Cell, To = target, IsJump = true, Captured = over };
            return true;
        }

        return false;
    }

    /// <summary>Used for multi-jumps: does this piece still have a jump from where it stands?</summary>
    public bool HasJumpAvailable(Piece piece)
    {
        int forward = piece.Owner.ForwardRow();

        for (int dc = -1; dc <= 1; dc += 2)
        {
            for (int dr = -1; dr <= 1; dr += 2)
            {
                if (!piece.IsKing && dr != forward) continue;

                var landing = new Vector2Int(piece.Cell.x + dc * 2, piece.Cell.y + dr * 2);
                if (TryBuildMove(piece, landing, out Move move) && move.IsJump) return true;
            }
        }

        return false;
    }

    public bool ShouldPromote(Piece piece) =>
        !piece.IsKing && piece.Cell.y == settings.KingRow(piece.Owner);
}
