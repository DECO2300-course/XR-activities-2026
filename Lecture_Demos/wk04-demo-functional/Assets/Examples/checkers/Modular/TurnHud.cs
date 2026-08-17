using UnityEngine;

/// <summary>
/// CONCERN: telling the player what is going on.
///
/// Reads the turn manager's status line and draws it. Deleting this script
/// removes the text and nothing else - the game keeps working.
/// </summary>
[RequireComponent(typeof(TurnManager))]
public class TurnHud : MonoBehaviour
{
    public int fontSize = 22;

    TurnManager turns;
    GUIStyle style;

    void Awake() => turns = GetComponent<TurnManager>();

    void OnGUI()
    {
        style ??= new GUIStyle(GUI.skin.label) { fontSize = fontSize, fontStyle = FontStyle.Bold };
        GUI.Label(new Rect(14f, 10f, 700f, 40f), turns.Status, style);
    }
}
