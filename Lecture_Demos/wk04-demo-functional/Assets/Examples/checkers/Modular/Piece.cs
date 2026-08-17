using UnityEngine;

/// <summary>
/// CONCERN: one piece.
///
/// Knows its owner, which square it belongs to and how to show itself
/// (sitting on a square, lifted by the mouse, wearing a crown). It does not
/// know the rules and never moves itself without being told.
/// </summary>
public class Piece : MonoBehaviour
{
    public PlayerColor Owner { get; private set; }
    public Vector2Int Cell { get; set; }
    public bool IsKing { get; private set; }

    BoardSettings settings;

    public void Initialise(BoardSettings boardSettings, PlayerColor owner, Vector2Int cell)
    {
        settings = boardSettings;
        Owner = owner;
        Cell = cell;
        name = $"{owner} {cell.x},{cell.y}";
        SnapToCell();
    }

    /// <summary>Put the piece back on the board, on whichever square it currently belongs to.</summary>
    public void SnapToCell()
    {
        transform.position = settings.PieceRestPosition(Cell);
    }

    /// <summary>Follow the mouse while being dragged.</summary>
    public void MoveTo(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }

    public void Promote()
    {
        if (IsKing) return;
        IsKing = true;

        var crown = PrimitiveFactory.Create(PrimitiveType.Cylinder, "Crown", transform, settings.kingCrown);
        crown.transform.localPosition = new Vector3(0f, 1f, 0f);     // local space: on top of the disc
        crown.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
        Destroy(crown.GetComponent<Collider>());                     // never block a click
    }
}
