using UnityEngine;

/// <summary>
/// CONCERN: the opening position.
///
/// Creates the cylinders, hands each one a Piece component and registers it
/// with the BoardState. Runs once and is then finished.
/// </summary>
[RequireComponent(typeof(BoardSettings), typeof(BoardState))]
public class PieceSpawner : MonoBehaviour
{
    BoardSettings settings;
    BoardState state;
    Transform root;

    void Awake()
    {
        settings = GetComponent<BoardSettings>();
        state = GetComponent<BoardState>();
    }

    void Start()
    {
        root = new GameObject("Pieces").transform;
        root.SetParent(transform, false);

        for (int row = 0; row < settings.size; row++)
        {
            for (int col = 0; col < settings.size; col++)
            {
                var cell = new Vector2Int(col, row);
                if (!settings.IsPlayable(cell)) continue;

                if (row < settings.rowsOfPieces)
                    Spawn(cell, PlayerColor.Red);
                else if (row >= settings.size - settings.rowsOfPieces)
                    Spawn(cell, PlayerColor.Black);
            }
        }
    }

    void Spawn(Vector2Int cell, PlayerColor owner)
    {
        var go = PrimitiveFactory.Create(PrimitiveType.Cylinder, "Piece", root, settings.ColorFor(owner));
        // A Unity cylinder is 2 units tall, so localScale.y is half the height.
        go.transform.localScale = new Vector3(
            settings.pieceDiameter, settings.pieceThickness * 0.5f, settings.pieceDiameter);

        var piece = go.AddComponent<Piece>();
        piece.Initialise(settings, owner, cell);
        state.Place(piece, cell);
    }
}
