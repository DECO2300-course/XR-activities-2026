using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabberRaycast : MonoBehaviour
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
        // Cast level with an object resting on the ground, not from the capsule's centre
        Ray ray = new Ray(transform.position + Vector3.down * 0.5f, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            Grabbable grabbable = hit.collider.GetComponent<Grabbable>();
            if (grabbable != null && !grabbable.isGrabbed)
            {
                // Grab the object we're looking at
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

                Debug.Log("Grabbed: " + grabbedObject.name + " using raycast");
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

            // Apply throwing force
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Hand the object back to physics before applying force
                rb.isKinematic = false;
                rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
            }

            Debug.Log("Threw: " + grabbedObject.name);
            grabbedObject = null;
        }
    }
}
