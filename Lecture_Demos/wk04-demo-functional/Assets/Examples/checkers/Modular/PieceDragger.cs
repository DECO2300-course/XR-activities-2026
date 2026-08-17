using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// CONCERN: the mouse.
///
/// Picks a piece up, drags it over the board and announces where it was
/// dropped. It knows nothing about turns, legality or capturing - it just
/// reports "this piece was dropped on that square" and lets someone else
/// decide what that means.
/// </summary>
[RequireComponent(typeof(BoardSettings))]
public class PieceDragger : MonoBehaviour
{
    [Tooltip("How high above the board a piece floats while it is being dragged.")]
    public float dragHeight = 0.6f;

    /// <summary>Piece + the square it was released over.</summary>
    public event System.Action<Piece, Vector2Int> PieceDropped;

    BoardSettings settings;
    Piece held;

    void Awake() => settings = GetComponent<BoardSettings>();

    void Update()
    {
        var mouse = Mouse.current;
        var cam = Camera.main;
        if (mouse == null || cam == null) return;

        Ray ray = cam.ScreenPointToRay(mouse.position.ReadValue());

        if (mouse.leftButton.wasPressedThisFrame)
            TryPickUp(ray);
        else if (held != null && mouse.leftButton.wasReleasedThisFrame)
            Drop();
        else if (held != null && mouse.leftButton.isPressed)
            DragAlongBoard(ray);
    }

    void TryPickUp(Ray ray)
    {
        if (!Physics.Raycast(ray, out RaycastHit hit, 500f)) return;

        // Hitting a tile returns null here, which is exactly what we want.
        held = hit.collider.GetComponentInParent<Piece>();
    }

    void DragAlongBoard(Ray ray)
    {
        var plane = new Plane(Vector3.up, transform.position + Vector3.up * dragHeight);
        if (plane.Raycast(ray, out float distance))
            held.MoveTo(ray.GetPoint(distance));
    }

    void Drop()
    {
        Piece piece = held;
        held = null;

        PieceDropped?.Invoke(piece, settings.WorldToCell(piece.transform.position));
    }
}
