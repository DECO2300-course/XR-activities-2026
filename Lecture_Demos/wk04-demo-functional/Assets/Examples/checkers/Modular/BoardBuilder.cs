using UnityEngine;

/// <summary>
/// CONCERN: building the visible board.
///
/// Spawns the checkerboard of cubes and then does nothing for the rest of the
/// game. It never touches a piece, a rule or a turn.
/// </summary>
[RequireComponent(typeof(BoardSettings))]
public class BoardBuilder : MonoBehaviour
{
    BoardSettings settings;
    Transform root;

    void Awake() => settings = GetComponent<BoardSettings>();

    void Start()
    {
        root = new GameObject("Board").transform;
        root.SetParent(transform, false);

        for (int row = 0; row < settings.size; row++)
        {
            for (int col = 0; col < settings.size; col++)
            {
                var cell = new Vector2Int(col, row);
                Color color = settings.IsPlayable(cell) ? settings.darkSquare : settings.lightSquare;

                var tile = PrimitiveFactory.Create(PrimitiveType.Cube, $"Tile {col},{row}", root, color);
                tile.transform.localScale =
                    new Vector3(settings.tileSize, settings.tileThickness, settings.tileSize);
                tile.transform.position =
                    settings.CellToWorld(cell) + Vector3.down * settings.tileThickness * 0.5f;
            }
        }
    }
}
