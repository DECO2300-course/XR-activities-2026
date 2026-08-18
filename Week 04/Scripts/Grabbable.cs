using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public bool isGrabbed = false;

    void Start()
    {
        // Make sure object has a Rigidbody
        if (GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }
    }
}
