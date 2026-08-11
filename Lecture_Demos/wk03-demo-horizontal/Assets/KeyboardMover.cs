using UnityEngine;
using UnityEngine.InputSystem;

// Smallest working WASD mover. Drop it straight onto a capsule.
// W and S drive forward and back, A and D turn on the spot.
// No gravity and no collision — it passes through walls. That is deliberate:
// the point here is Update() and Time.deltaTime, nothing more.
public class KeyboardMover : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 120f; // degrees per second

    void Update()
    {
        var k = Keyboard.current;
        if (k == null) return;

        float forward = (k.wKey.isPressed ? 1 : 0) - (k.sKey.isPressed ? 1 : 0);
        float turn = (k.dKey.isPressed ? 1 : 0) - (k.aKey.isPressed ? 1 : 0);

        transform.Rotate(0f, turn * turnSpeed * Time.deltaTime, 0f);
        transform.Translate(0f, 0f, forward * speed * Time.deltaTime);
    }
}
