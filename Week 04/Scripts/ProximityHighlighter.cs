using UnityEngine;

public class ProximityHighlighter : MonoBehaviour
{
    public float proximityDistance = 3f;

    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private Renderer objectRenderer;
    private bool isPlayerNearby = false;

    private GameObject player;

    void Start()
    {
        // Get the renderer component for colour changes
        objectRenderer = GetComponent<Renderer>();

        // Set initial colour
        objectRenderer.material.color = normalColor;

        // Find the player once and store the reference
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if player is within proximity distance
        bool wasNearby = isPlayerNearby;
        isPlayerNearby = distanceToPlayer <= proximityDistance;

        // Handle proximity highlighting
        if (isPlayerNearby && !wasNearby)
        {
            // Player just entered proximity - highlight the object
            objectRenderer.material.color = highlightColor;
            Debug.Log(gameObject.name + " is now highlighted!");
        }
        else if (!isPlayerNearby && wasNearby)
        {
            // Player just left proximity - return to normal colour
            objectRenderer.material.color = normalColor;
            Debug.Log(gameObject.name + " returned to normal colour");
        }
    }
}
