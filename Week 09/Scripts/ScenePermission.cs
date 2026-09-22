using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Week 9, Activity 1. Planes and raycasts need the scene permission, which the player grants
/// at runtime, so the managers start disabled and are enabled once it is granted.
/// </summary>
public class ScenePermission : MonoBehaviour
{
    const string k_ScenePermission = "com.oculus.permission.USE_SCENE";

    [SerializeField] ARPlaneManager planeManager;
    [SerializeField] ARRaycastManager raycastManager;

    void Start()
    {
        if (Permission.HasUserAuthorizedPermission(k_ScenePermission))
        {
            Enable();
            return;
        }

        var callbacks = new PermissionCallbacks();
        callbacks.PermissionGranted += _ => Enable();
        Permission.RequestUserPermission(k_ScenePermission, callbacks);
    }

    void Enable()
    {
        if (planeManager != null)
            planeManager.enabled = true;

        if (raycastManager != null)
            raycastManager.enabled = true;

        Debug.Log("ScenePermission: scene permission granted, managers enabled.");
    }
}
