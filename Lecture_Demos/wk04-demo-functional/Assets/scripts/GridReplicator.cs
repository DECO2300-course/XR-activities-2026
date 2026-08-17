using UnityEngine;

// Tiles copies of this object in a square grid around it, so one Plane or Cube
// becomes a whole floor. radius 1 gives 3x3, radius 2 gives 5x5, and so on.
//
// Leave Spacing at zero and it measures the object's own renderer bounds, so a
// default Plane (10 units) or Cube (1 unit) tiles edge to edge with no setup.
// Right-click the component for Build Grid / Clear Grid to work in the Editor
// without entering Play mode.
public class GridReplicator : MonoBehaviour
{
    public int radius = 2;
    public Vector2 spacing = Vector2.zero; // x and z gap; zero means "measure me"
    public bool buildOnStart = true;

    [SerializeField, HideInInspector] GameObject grid; // holder for the copies

    void Start()
    {
        if (buildOnStart) Build();
    }

    [ContextMenu("Build Grid")]
    void Build()
    {
        Clear();
        if (radius < 1) return;

        Vector2 step = spacing;
        if (step.x <= 0f || step.y <= 0f)
        {
            Vector3 size = MeasureWholeObject();
            if (step.x <= 0f) step.x = size.x;
            if (step.y <= 0f) step.y = size.z;
        }

        grid = new GameObject(name + " Grid");
        grid.transform.position = transform.position;

        for (int x = -radius; x <= radius; x++)
        {
            for (int z = -radius; z <= radius; z++)
            {
                if (x == 0 && z == 0) continue; // that one is us

                Vector3 position = transform.position + new Vector3(x * step.x, 0f, z * step.y);
                GameObject copy = Instantiate(gameObject, position, transform.rotation, grid.transform);
                copy.name = name + " (" + x + ", " + z + ")";

                // Without this, every copy builds its own grid and those grids build
                // grids. Disabling first matters too: Start() never runs on a disabled
                // component, so the copy cannot replicate before it is removed.
                // Children are searched as well, in case one of them also carries this.
                GridReplicator[] clones = copy.GetComponentsInChildren<GridReplicator>(true);
                foreach (GridReplicator clone in clones)
                {
                    clone.enabled = false;
                    Remove(clone);
                }
            }
        }
    }

    // Every renderer under this object, wrapped in one box. Measuring only the
    // first one would size the grid to a single sub-object rather than the whole
    // assembly, and the tiles would overlap.
    Vector3 MeasureWholeObject()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return Vector3.one;

        Bounds combined = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            combined.Encapsulate(renderers[i].bounds);

        return combined.size;
    }

    [ContextMenu("Clear Grid")]
    void Clear()
    {
        if (grid != null) Remove(grid);
        grid = null;
    }

    // Play mode and the Editor need different destroy calls.
    static void Remove(Object target)
    {
        if (Application.isPlaying) Destroy(target);
        else DestroyImmediate(target);
    }
}
