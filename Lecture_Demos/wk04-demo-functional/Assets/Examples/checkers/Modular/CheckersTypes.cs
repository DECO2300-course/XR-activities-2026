using UnityEngine;

/// <summary>Shared vocabulary. No behaviour lives here - just the words every
/// other checkers script uses so they can talk to each other.</summary>
public enum PlayerColor
{
    Red,
    Black
}

/// <summary>A single validated move, produced by MoveRules and applied by MoveExecutor.</summary>
public struct Move
{
    public Vector2Int From;
    public Vector2Int To;
    public bool IsJump;
    public Vector2Int Captured;     // only meaningful when IsJump is true
}

public static class PlayerColorExtensions
{
    public static PlayerColor Opponent(this PlayerColor player) =>
        player == PlayerColor.Red ? PlayerColor.Black : PlayerColor.Red;

    /// <summary>Red advances up the rows, black advances down them.</summary>
    public static int ForwardRow(this PlayerColor player) =>
        player == PlayerColor.Red ? 1 : -1;
}
