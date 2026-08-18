using UnityEngine;

public class ProximitySound : MonoBehaviour
{
    public float proximityDistance = 3f;

    public AudioSource audioSource;
    public AudioClip proximitySound;

    private bool isPlayerNearby = false;
    private GameObject player;

    void Start()
    {
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

        // Play sound when player enters proximity
        if (isPlayerNearby && !wasNearby)
        {
            if (audioSource && proximitySound)
            {
                audioSource.PlayOneShot(proximitySound);
            }
        }
    }
}
