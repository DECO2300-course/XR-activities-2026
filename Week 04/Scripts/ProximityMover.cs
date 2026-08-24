using UnityEngine;

public class ProximityMover : MonoBehaviour
{
    public float proximityDistance = 3f;

    public float moveSpeed = 5f;
    public float moveDistance = 2f;

    private bool isPlayerNearby = false;
    private GameObject player;
    private Vector3 originalPosition;
    private Vector3 retreatPosition;

    void Start()
    {
        // Store original position for movement
        originalPosition = transform.position;

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

        // Work out where to retreat to once, on the frame the player arrives. Recalculating
        // it every frame would drag the target along behind the object as it moved
        if (isPlayerNearby && !wasNearby)
        {
            Vector3 directionFromPlayer = (transform.position - player.transform.position).normalized;
            retreatPosition = originalPosition + (directionFromPlayer * moveDistance);
        }

        // Move towards the retreat position, or back to where we started
        Vector3 targetPosition = isPlayerNearby ? retreatPosition : originalPosition;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
