using UnityEngine;

/// <summary>
/// CONCERN: which piece is on which square.
///
/// The single source of truth for the game position. It stores and moves
/// references; it does not decide whether a move is legal and it never
/// destroys anything.
/// </summary>
[RequireComponent(typeof(BoardSettings))]
public class BoardState : MonoBehaviour
{
    BoardSettings settings;
    Piece[] cells;

    void Awake()
    {
        settings = GetComponent<BoardSettings>();
        cells = new Piece[settings.size * settings.size];
    }

    public Piece Get(Vector2Int cell) =>
        settings.InBounds(cell) ? cells[Index(cell)] : null;

    public bool IsEmpty(Vector2Int cell) =>
        settings.InBounds(cell) && cells[Index(cell)] == null;

    public void Place(Piece piece, Vector2Int cell)
    {
        cells[Index(cell)] = piece;
        piece.Cell = cell;
    }

    public void MovePiece(Vector2Int from, Vector2Int to)
    {
        Piece piece = cells[Index(from)];
        cells[Index(from)] = null;
        cells[Index(to)] = piece;
        if (piece != null) piece.Cell = to;
    }

    public void Remove(Vector2Int cell)
    {
        cells[Index(cell)] = null;
    }

    public int Count(PlayerColor owner)
    {
        int total = 0;
        foreach (Piece piece in cells)
            if (piece != null && piece.Owner == owner) total++;
        return total;
    }

    int Index(Vector2Int cell) => cell.x + cell.y * settings.size;
}
