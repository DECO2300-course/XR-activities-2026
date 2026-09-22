using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Week 9, Activity 1. Casts a ray from a controller at the planes the headset knows about
/// and places a prefab where it lands. This is the Step 6 version; Step 7's classification
/// filter is included below, commented out.
/// </summary>
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField] ARRaycastManager raycastManager;
    [SerializeField] Transform rayOrigin;              // a controller transform on your rig
    [SerializeField] GameObject prefabToPlace;
    [SerializeField] InputActionReference placeAction; // e.g. the Select action from XRI

    // Reused every cast so we are not allocating a new list each time.
    static readonly List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void OnEnable()
    {
        placeAction.action.Enable();
        placeAction.action.performed += OnPlace;
    }

    void OnDisable()
    {
        placeAction.action.performed -= OnPlace;
    }

    void OnPlace(InputAction.CallbackContext context)
    {
        var ray = new Ray(rayOrigin.position, rayOrigin.forward);

        if (!raycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            return;   // pointing at nothing the room knows about

        // Step 7: uncomment to restrict placement to tables, refusing floors and walls.
        // The bitwise and matters, because a plane can carry more than one classification,
        // so == would reject a table that is also marked as something else.
        //
        // if (s_Hits[0].trackable is not ARPlane plane)
        //     return;
        //
        // if ((plane.classifications & PlaneClassifications.Table) == 0)
        //     return;

        // Hits come back sorted nearest-first.
        var hitPose = s_Hits[0].pose;
        Instantiate(prefabToPlace, hitPose.position, hitPose.rotation);
    }
}
