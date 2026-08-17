using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// THE MONOLITH.
///
/// One MonoBehaviour that owns the entire game: it builds the board, spawns the
/// pieces, reads the mouse, drags a piece, validates the move, removes jumped
/// pieces, promotes kings, tracks whose turn it is and draws the on-screen text.
///
/// Everything works, and everything is welded together. Compare it with the
/// scripts in scripts_checkers/Modular, which do exactly the same job split
/// across one-concern-per-file components.
///
/// Drop this on an empty GameObject and press Play - no prefabs required.
/// </summary>
[AddComponentMenu("Checkers/Checkers Monolith")]
public class CheckersMonolith : MonoBehaviour
{
    // Square contents. Stored as plain ints so the whole game state is three arrays.
    const int Empty = -1;
    const int Red = 0;
    const int Black = 1;

    [Header("Board")]
    public int boardSize = 8;
    public float tileSize = 1f;
    public float tileThickness = 0.2f;
    public Color lightSquare = new Color(0.86f, 0.83f, 0.74f);
    public Color darkSquare = new Color(0.24f, 0.21f, 0.19f);

    [Header("Pieces")]
    public float pieceDiameter = 0.7f;
    public float pieceThickness = 0.18f;
    public Color redPiece = new Color(0.76f, 0.14f, 0.14f);
    public Color blackPiece = new Color(0.09f, 0.09f, 0.11f);
    public Color kingCrown = new Color(0.95f, 0.78f, 0.25f);

    [Header("Interaction")]
    public float dragHeight = 0.6f;

    // ---- game state -------------------------------------------------------
    int[] squareOwner;          // Red / Black / Empty, indexed [col + row * boardSize]
    bool[] squareIsKing;
    GameObject[] squarePiece;

    Transform boardRoot;
    Transform pieceRoot;

    int currentPlayer = Red;
    bool gameOver;
    string status = "";

    // ---- drag state -------------------------------------------------------
    Transform dragged;
    Vector2Int dragFrom;
    bool hasChainJump;          // a jump just happened and the same piece can jump again
    Vector2Int chainCell;

    GUIStyle labelStyle;

    // =======================================================================
    // Setup
    // =======================================================================
    void Start()
    {
        int cells = boardSize * boardSize;
        squareOwner = new int[cells];
        squareIsKing = new bool[cells];
        squarePiece = new GameObject[cells];
        for (int i = 0; i < cells; i++) squareOwner[i] = Empty;

        boardRoot = new GameObject("Board").transform;
        boardRoot.SetParent(transform, false);
        pieceRoot = new GameObject("Pieces").transform;
        pieceRoot.SetParent(transform, false);

        BuildBoard();
        SpawnPieces();
        status = "Red to move";
    }

    void BuildBoard()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                var cell = new Vector2Int(col, row);
                var tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tile.name = $"Tile {col},{row}";
                tile.transform.SetParent(boardRoot, false);
                tile.transform.localScale = new Vector3(tileSize, tileThickness, tileSize);
                tile.transform.position = CellToWorld(cell) + Vector3.down * tileThickness * 0.5f;
                tile.GetComponent<Renderer>().material.color = IsPlayable(cell) ? darkSquare : lightSquare;
            }
        }
    }

    void SpawnPieces()
    {
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                var cell = new Vector2Int(col, row);
                if (!IsPlayable(cell)) continue;

                if (row < 3) CreatePiece(cell, Red);
                else if (row >= boardSize - 3) CreatePiece(cell, Black);
            }
        }
    }

    void CreatePiece(Vector2Int cell, int player)
    {
        var piece = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        piece.name = (player == Red ? "Red" : "Black") + $" {cell.x},{cell.y}";
        piece.transform.SetParent(pieceRoot, false);
        // A Unity cylinder is 2 units tall, so localScale.y is half the height.
        piece.transform.localScale = new Vector3(pieceDiameter, pieceThickness * 0.5f, pieceDiameter);
        piece.GetComponent<Renderer>().material.color = player == Red ? redPiece : blackPiece;

        int i = Index(cell);
        squareOwner[i] = player;
        squareIsKing[i] = false;
        squarePiece[i] = piece;
        PlaceOnBoard(piece.transform, cell);
    }

    // =======================================================================
    // Input + dragging
    // =======================================================================
    void Update()
    {
        if (gameOver) return;

        var mouse = Mouse.current;
        var cam = Camera.main;
        if (mouse == null || cam == null) return;

        Ray ray = cam.ScreenPointToRay(mouse.position.ReadValue());

        if (mouse.leftButton.wasPressedThisFrame)
        {
            TryPickUp(ray);
        }
        else if (dragged != null && mouse.leftButton.wasReleasedThisFrame)
        {
            Drop();
        }
        else if (dragged != null && mouse.leftButton.isPressed)
        {
            DragAlongBoard(ray);
        }
    }

    void TryPickUp(Ray ray)
    {
        if (!Physics.Raycast(ray, out RaycastHit hit, 500f)) return;
        if (hit.transform.parent != pieceRoot) return;      // clicked a tile, not a piece

        Vector2Int cell = WorldToCell(hit.transform.position);
        if (!InBounds(cell) || squarePiece[Index(cell)] != hit.transform.gameObject) return;

        if (squareOwner[Index(cell)] != currentPlayer)
        {
            status = PlayerName(currentPlayer) + " to move - that is not your piece";
            return;
        }

        if (hasChainJump && chainCell != cell)
        {
            status = "You must keep jumping with the same piece";
            return;
        }

        dragged = hit.transform;
        dragFrom = cell;
    }

    void DragAlongBoard(Ray ray)
    {
        var plane = new Plane(Vector3.up, transform.position + Vector3.up * dragHeight);
        if (plane.Raycast(ray, out float distance))
            dragged.position = ray.GetPoint(distance);
    }

    void Drop()
    {
        Transform piece = dragged;
        dragged = null;

        Vector2Int target = WorldToCell(piece.position);
        if (!TryMove(dragFrom, target))
            PlaceOnBoard(piece, dragFrom);      // illegal - snap back where it came from
    }

    // =======================================================================
    // Rules
    // =======================================================================
    bool TryMove(Vector2Int from, Vector2Int to)
    {
        if (!InBounds(to) || !IsPlayable(to) || squareOwner[Index(to)] != Empty) return false;

        int player = squareOwner[Index(from)];
        int forward = player == Red ? 1 : -1;
        bool king = squareIsKing[Index(from)];

        int stepCol = to.x - from.x;
        int stepRow = to.y - from.y;

        bool isSlide = Mathf.Abs(stepCol) == 1 && (stepRow == forward || (king && stepRow == -forward));
        bool isJump = Mathf.Abs(stepCol) == 2 && (stepRow == 2 * forward || (king && stepRow == -2 * forward));

        // Mid-chain, only another jump is allowed.
        if (hasChainJump && !isJump) return false;

        if (isSlide)
        {
            MovePiece(from, to);
            EndTurn(to, false);
            return true;
        }

        if (isJump)
        {
            var jumped = new Vector2Int((from.x + to.x) / 2, (from.y + to.y) / 2);
            int victim = squareOwner[Index(jumped)];
            if (victim == Empty || victim == player) return false;

            Capture(jumped);
            MovePiece(from, to);
            EndTurn(to, true);
            return true;
        }

        return false;
    }

    void MovePiece(Vector2Int from, Vector2Int to)
    {
        int a = Index(from), b = Index(to);
        squareOwner[b] = squareOwner[a];
        squareIsKing[b] = squareIsKing[a];
        squarePiece[b] = squarePiece[a];

        squareOwner[a] = Empty;
        squareIsKing[a] = false;
        squarePiece[a] = null;

        PlaceOnBoard(squarePiece[b].transform, to);
    }

    void Capture(Vector2Int cell)
    {
        int i = Index(cell);
        if (squarePiece[i] != null) Destroy(squarePiece[i]);
        squarePiece[i] = null;
        squareOwner[i] = Empty;
        squareIsKing[i] = false;
    }

    void EndTurn(Vector2Int landedOn, bool didJump)
    {
        bool promoted = TryPromote(landedOn);

        // A jumper that can jump again keeps the turn (promotion always ends it).
        if (didJump && !promoted && HasJumpFrom(landedOn))
        {
            hasChainJump = true;
            chainCell = landedOn;
            status = PlayerName(currentPlayer) + " jumps again!";
            return;
        }

        hasChainJump = false;

        if (CountPieces(1 - currentPlayer) == 0)
        {
            gameOver = true;
            status = PlayerName(currentPlayer) + " wins!";
            return;
        }

        currentPlayer = 1 - currentPlayer;
        status = PlayerName(currentPlayer) + " to move";
    }

    bool TryPromote(Vector2Int cell)
    {
        int i = Index(cell);
        if (squareIsKing[i]) return false;

        int lastRow = squareOwner[i] == Red ? boardSize - 1 : 0;
        if (cell.y != lastRow) return false;

        squareIsKing[i] = true;

        var crown = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        crown.name = "Crown";
        crown.transform.SetParent(squarePiece[i].transform, false);
        crown.transform.localPosition = new Vector3(0f, 1f, 0f);   // local space - sits on top
        crown.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
        crown.GetComponent<Renderer>().material.color = kingCrown;
        Destroy(crown.GetComponent<Collider>());
        return true;
    }

    bool HasJumpFrom(Vector2Int cell)
    {
        int i = Index(cell);
        int player = squareOwner[i];
        int forward = player == Red ? 1 : -1;
        bool king = squareIsKing[i];

        for (int dc = -1; dc <= 1; dc += 2)
        {
            for (int dr = -1; dr <= 1; dr += 2)
            {
                if (!king && dr != forward) continue;

                var over = new Vector2Int(cell.x + dc, cell.y + dr);
                var land = new Vector2Int(cell.x + dc * 2, cell.y + dr * 2);
                if (!InBounds(land) || squareOwner[Index(land)] != Empty) continue;

                int victim = squareOwner[Index(over)];
                if (victim != Empty && victim != player) return true;
            }
        }
        return false;
    }

    int CountPieces(int player)
    {
        int count = 0;
        for (int i = 0; i < squareOwner.Length; i++)
            if (squareOwner[i] == player) count++;
        return count;
    }

    // =======================================================================
    // Board maths + helpers
    // =======================================================================
    int Index(Vector2Int cell) => cell.x + cell.y * boardSize;

    bool InBounds(Vector2Int cell) =>
        cell.x >= 0 && cell.x < boardSize && cell.y >= 0 && cell.y < boardSize;

    bool IsPlayable(Vector2Int cell) => (cell.x + cell.y) % 2 == 0;

    Vector3 CellToWorld(Vector2Int cell)
    {
        float offset = (boardSize - 1) * 0.5f;
        return transform.position + new Vector3((cell.x - offset) * tileSize, 0f, (cell.y - offset) * tileSize);
    }

    Vector2Int WorldToCell(Vector3 world)
    {
        Vector3 local = world - transform.position;
        float offset = (boardSize - 1) * 0.5f;
        return new Vector2Int(
            Mathf.RoundToInt(local.x / tileSize + offset),
            Mathf.RoundToInt(local.z / tileSize + offset));
    }

    void PlaceOnBoard(Transform piece, Vector2Int cell)
    {
        piece.position = CellToWorld(cell) + Vector3.up * pieceThickness * 0.5f;
    }

    static string PlayerName(int player) => player == Red ? "Red" : "Black";

    void OnGUI()
    {
        labelStyle ??= new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold };
        GUI.Label(new Rect(14f, 10f, 700f, 40f), status, labelStyle);
    }
}
