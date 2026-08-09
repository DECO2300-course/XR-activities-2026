using UnityEngine;

public class SimpleRaycast : MonoBehaviour
{
    public float raycastDistance = 10f;

    void Update()
    {
        // Create a ray from the object's position in the forward direction
        Ray ray = new Ray(transform.position, transform.forward);
        
        // Perform the raycast
        RaycastHit hit;
        bool hitDetected = Physics.Raycast(ray, out hit, raycastDistance);
        
        if (hitDetected)
        {
            // Log the name of the hit object
            Debug.Log("Hit: " + hit.transform.name);
        }
    }
    
    void OnDrawGizmos()
    {
        // Check if we hit something
        RaycastHit hit;
        bool hitDetected = Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance);
        
        // Draw the raycast line - red if no hit, green if hit
        Gizmos.color = hitDetected ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
        
        // Draw a wire sphere at the hit point if we hit something
        if (hitDetected)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point, 0.1f);
        }
    }
}
