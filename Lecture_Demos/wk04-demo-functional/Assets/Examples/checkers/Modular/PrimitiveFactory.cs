using UnityEngine;

/// <summary>
/// CONCERN: making a coloured primitive.
///
/// The board builder and the piece spawner both need "a cube/cylinder of colour
/// X parented to Y", so that job lives in exactly one place.
/// </summary>
public static class PrimitiveFactory
{
    public static GameObject Create(PrimitiveType shape, string name, Transform parent, Color color)
    {
        var go = GameObject.CreatePrimitive(shape);
        go.name = name;
        go.transform.SetParent(parent, false);
        Tint(go, color);
        return go;
    }

    /// <summary>Renderer.material returns a per-object clone, so this never edits a shared asset.</summary>
    public static void Tint(GameObject go, Color color)
    {
        go.GetComponent<Renderer>().material.color = color;
    }
}
