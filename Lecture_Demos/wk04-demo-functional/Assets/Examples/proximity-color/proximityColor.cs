using UnityEngine;

public class proximityColor : MonoBehaviour
{
    [Header("Target Cube")]
    public Transform otherCube;

    [Header("Settings")]
    public float triggerDistance = 3f;

    private Renderer cubeRenderer;
    private Color originalColor;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color;
    }

    void Update()
    {
        if (otherCube == null)
            return;

        float distance = Vector3.Distance(transform.position, otherCube.position);

        if (distance <= triggerDistance)
        {
            cubeRenderer.material.color = Color.blue;
        }
        else
        {
            cubeRenderer.material.color = originalColor;
        }
    }
}