using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collected: " + other.name);
        Destroy(other.gameObject);
    }
}
