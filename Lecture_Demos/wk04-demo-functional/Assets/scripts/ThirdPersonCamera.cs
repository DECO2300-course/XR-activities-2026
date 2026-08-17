using UnityEngine;

// Third person follow camera. Put this on the capsule and drag your camera into
// the Cam slot yourself — nothing is found automatically, so what you assign in
// the Inspector is exactly what moves.
//
// The camera must NOT be a child of the capsule. This script sets its position
// every frame, so being parented as well would apply the movement twice.
//
// It runs in LateUpdate, after everything else has finished moving, and it is a
// separate script on purpose: Elytra switches KeyboardMover off during flight, so
// a camera living inside KeyboardMover would freeze the moment you launched.
public class ThirdPersonCamera : MonoBehaviour
{
    public Transform cam;
    public Vector3 offset = new Vector3(0f, 3f, -6f); // relative to the capsule's own facing
    public float followSpeed = 8f;                    // lower is lazier and floatier
    public float lookHeight = 1.5f;                   // aim above the feet, at the head

    void Start()
    {
        // Said out loud once, so an empty slot is not a silent do-nothing.
        if (cam == null)
            Debug.LogWarning("ThirdPersonCamera on " + name + " has no camera assigned.", this);
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // TransformPoint puts the offset in the capsule's local space, so the camera
        // swings around behind us as we turn, and tips with us as we dive.
        Vector3 target = transform.TransformPoint(offset);

        cam.position = Vector3.Lerp(cam.position, target, followSpeed * Time.deltaTime);
        cam.LookAt(transform.position + Vector3.up * lookHeight);
    }
}
