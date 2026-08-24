using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabberVelocity : MonoBehaviour
{
    public float grabDistance = 3f;
    private GameObject grabbedObject = null;

    InputAction interactAction;

    void Start()
    {
        // Find the Interact action from the project's input actions asset
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if (interactAction.WasPressedThisFrame())
        {
            if (grabbedObject == null)
            {
                GrabObject();
            }
            else
            {
                ThrowObject();
            }
        }
    }

    void GrabObject()
    {
        // Find objects with Grabbable script
        Grabbable[] grabbables = FindObjectsByType<Grabbable>(FindObjectsSortMode.None);

        foreach (Grabbable grabbable in grabbables)
        {
            if (!grabbable.isGrabbed)
            {
                float distance = Vector3.Distance(transform.position, grabbable.transform.position);
                if (distance <= grabDistance)
                {
                    // Grab the first object we find
                    grabbedObject = grabbable.gameObject;
                    grabbable.isGrabbed = true;

                    // Take the object out of the physics simulation while it is held
                    Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }

                    // Make object follow player
                    grabbedObject.transform.SetParent(transform);
                    grabbedObject.transform.localPosition = new Vector3(0, 1, 1);

                    Debug.Log("Grabbed: " + grabbedObject.name);
                    break;
                }
            }
        }
    }

    void ThrowObject()
    {
        if (grabbedObject != null)
        {
            // Release object
            grabbedObject.transform.SetParent(null);

            // Get the Grabbable component and mark as not grabbed
            Grabbable grabbable = grabbedObject.GetComponent<Grabbable>();
            if (grabbable != null)
            {
                grabbable.isGrabbed = false;
            }

            // Apply throwing velocity for a more natural arc
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Hand the object back to physics before setting its velocity
                rb.isKinematic = false;

                // Calculate throw velocity
                Vector3 throwDirection = transform.forward;
                float throwSpeed = 10f;

                // Add upward component for natural arc, then normalise so the speed you
                // asked for is the speed you get
                Vector3 throwVelocity = (throwDirection + Vector3.up * 0.3f).normalized * throwSpeed;

                // Apply velocity directly
                rb.linearVelocity = throwVelocity;

                Debug.Log("Threw: " + grabbedObject.name + " with velocity: " + throwVelocity);
            }

            grabbedObject = null;
        }
    }
}
