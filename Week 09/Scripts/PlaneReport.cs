using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Week 9, Activity 1. Reports how many planes the runtime currently knows about, so you get
/// a number rather than an impression. Read it with Meta Quest Developer Hub's log view.
/// Zero planes means Space Setup has not been run in this room, or the scene permission was
/// dismissed, rather than a bug in your code.
/// </summary>
public class PlaneReport : MonoBehaviour
{
    [Tooltip("The ARPlaneManager on your XR Origin.")]
    [SerializeField] ARPlaneManager planeManager;

    void Update()
    {
        if (planeManager == null)
            return;

        // trackables is the live collection of planes the runtime currently knows about.
        Debug.Log($"Planes: {planeManager.trackables.count}");
    }
}
