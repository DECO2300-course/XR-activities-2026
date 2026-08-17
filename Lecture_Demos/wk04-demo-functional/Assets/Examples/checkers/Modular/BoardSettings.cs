using UnityEngine;

/// <summary>
/// CONCERN: board geometry.
///
/// Every dimension, colour and cell/world conversion in the game lives here.
/// Nothing in this script knows the rules of checkers - it only knows where
/// square (3, 5) is in world space and what colour a red piece should be.
/// </summary>
public class BoardSettings : MonoBehaviour
{
    [Header("Board")]
    public int size = 8;
    public float tileSize = 1f;
    public float tileThickness = 0.2f;
    public Color lightSquare = new Color(0.86f, 0.83f, 0.74f);
    public Color darkSquare = new Color(0.24f, 0.21f, 0.19f);

    [Header("Pieces")]
    public int rowsOfPieces = 3;
    public float pieceDiameter = 0.7f;
    public float pieceThickness = 0.18f;
    public Color redPiece = new Color(0.76f, 0.14f, 0.14f);
    public Color blackPiece = new Color(0.09f, 0.09f, 0.11f);
    public Color kingCrown = new Color(0.95f, 0.78f, 0.25f);

    public bool InBounds(Vector2Int cell) =>
        cell.x >= 0 && cell.x < size && cell.y >= 0 && cell.y < size;

    /// <summary>Checkers is played on the dark squares only.</summary>
    public bool IsPlayable(Vector2Int cell) => (cell.x + cell.y) % 2 == 0;

    public Vector3 CellToWorld(Vector2Int cell)
    {
        float offset = (size - 1) * 0.5f;
        return transform.position +
               new Vector3((cell.x - offset) * tileSize, 0f, (cell.y - offset) * tileSize);
    }

    public Vector2Int WorldToCell(Vector3 world)
    {
        Vector3 local = world - transform.position;
        float offset = (size - 1) * 0.5f;
        return new Vector2Int(
            Mathf.RoundToInt(local.x / tileSize + offset),
            Mathf.RoundToInt(local.z / tileSize + offset));
    }

    /// <summary>Where a piece sits when it is resting on the board.</summary>
    public Vector3 PieceRestPosition(Vector2Int cell) =>
        CellToWorld(cell) + Vector3.up * pieceThickness * 0.5f;

    public Color ColorFor(PlayerColor player) =>
        player == PlayerColor.Red ? redPiece : blackPiece;

    /// <summary>The row a player has to reach to be crowned.</summary>
    public int KingRow(PlayerColor player) =>
        player == PlayerColor.Red ? size - 1 : 0;
}
