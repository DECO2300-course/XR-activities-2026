using UnityEngine;

/// <summary>
/// CONCERN: whose turn it is.
///
/// Owns the current player, the "you must finish your multi-jump" lock, the
/// win check and the status line. It never moves a piece.
/// </summary>
[RequireComponent(typeof(BoardState))]
public class TurnManager : MonoBehaviour
{
    public PlayerColor CurrentPlayer { get; private set; } = PlayerColor.Red;
    public bool GameOver { get; private set; }

    /// <summary>Set while a piece is part-way through a chain of jumps.</summary>
    public Piece LockedPiece { get; private set; }

    public string Status { get; private set; } = "Red to move";

    /// <summary>Raised whenever the status changes, so the HUD can redraw.</summary>
    public event System.Action Changed;

    BoardState state;

    void Awake() => state = GetComponent<BoardState>();

    public bool CanPlay(Piece piece)
    {
        if (GameOver) return false;
        if (piece.Owner != CurrentPlayer) return false;
        return LockedPiece == null || LockedPiece == piece;
    }

    /// <summary>The same player keeps the turn because their piece can jump again.</summary>
    public void ContinueChain(Piece piece)
    {
        LockedPiece = piece;
        SetStatus($"{CurrentPlayer} jumps again!");
    }

    public void EndTurn()
    {
        LockedPiece = null;

        if (state.Count(CurrentPlayer.Opponent()) == 0)
        {
            GameOver = true;
            SetStatus($"{CurrentPlayer} wins!");
            return;
        }

        CurrentPlayer = CurrentPlayer.Opponent();
        SetStatus($"{CurrentPlayer} to move");
    }

    /// <summary>Explain to the player why nothing happened (never talks over a win).</summary>
    public void Reject(string reason)
    {
        if (GameOver) return;
        SetStatus(reason);
    }

    void SetStatus(string text)
    {
        Status = text;
        Changed?.Invoke();
    }
}
