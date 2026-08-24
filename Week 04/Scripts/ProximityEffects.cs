using UnityEngine;

public class ProximityEffects : MonoBehaviour
{
    public float proximityDistance = 3f;

    public bool rotateOnProximity = true;
    public bool scaleOnProximity = true;
    public float rotationSpeed = 90f;
    public float scaleMultiplier = 1.5f;

    private bool isPlayerNearby = false;
    private GameObject player;
    private Vector3 originalScale;

    void Start()
    {
        // Store original scale for scaling effects
        originalScale = transform.localScale;

        // Find the player once and store the reference
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Check if player is within proximity distance
        isPlayerNearby = distanceToPlayer <= proximityDistance;

        // Handle rotation (if enabled)
        if (rotateOnProximity && isPlayerNearby)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }

        // Handle scaling (if enabled)
        if (scaleOnProximity)
        {
            if (isPlayerNearby)
            {
                transform.localScale = originalScale * scaleMultiplier;
            }
            else
            {
                transform.localScale = originalScale;
            }
        }
    }
}
