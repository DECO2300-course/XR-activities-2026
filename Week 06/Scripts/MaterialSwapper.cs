using UnityEngine;

/// <summary>
/// Week 6, Activities 1 and 3. Swaps a Renderer between two materials from Interactable
/// events. Wire SwapToMaterialB to Select Entered and SwapToMaterialA to Select Exited.
/// </summary>
public class MaterialSwapper : MonoBehaviour
{
    public GameObject targetObject;

    public Material materialA;
    public Material materialB;

    private Renderer targetRenderer;

    void Start()
    {
        if (targetObject == null)
            targetObject = gameObject;

        targetRenderer = targetObject.GetComponent<Renderer>();

        if (targetRenderer == null)
        {
            Debug.LogError($"{name}: Target Object has no Renderer to swap.", this);
            return;
        }

        targetRenderer.material = materialA;
    }

    public void SwapToMaterialB()
    {
        if (targetRenderer != null)
            targetRenderer.material = materialB;
    }

    public void SwapToMaterialA()
    {
        if (targetRenderer != null)
            targetRenderer.material = materialA;
    }
}
