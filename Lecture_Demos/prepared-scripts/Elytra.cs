using UnityEngine;
using UnityEngine.InputSystem;

// Minecraft-style elytra flight. Drop this onto the same capsule as KeyboardMover.
//
//   Space (on the ground) : launch straight up
//   Space (in the air)    : rocket boost along the nose
//   W / S                 : nose down / nose up
//   A / D                 : turn
//   Q                     : bail out — drop to the ground and walk again
//
// Dive to gain speed, climb to trade that speed back for height.
// While flying this switches KeyboardMover off so the two scripts are not
// both writing to the transform in the same frame.
[RequireComponent(typeof(KeyboardMover))]
public class Elytra : MonoBehaviour
{
    public float launchSpeed = 18f;    // initial kick straight up
    public float boostSpeed = 12f;     // added along the nose per air-Space
    public float gravity = -12f;
    public float glideStrength = 1.5f; // how quickly velocity swings toward the nose
    public float pitchSpeed = 70f;     // degrees per second
    public float turnSpeed = 90f;
    public float drag = 0.15f;         // stops a long dive accelerating forever

    KeyboardMover mover;
    Vector3 velocity;
    float groundY;
    float pitch;
    bool flying;

    void Start()
    {
        mover = GetComponent<KeyboardMover>();
        groundY = transform.position.y; // flat ground only — we land back at our start height
    }

    void Update()
    {
        var k = Keyboard.current;
        if (k == null) return;

        if (!flying)
        {
            if (k.spaceKey.wasPressedThisFrame) Launch();
            return;
        }

        Fly(k);
    }

    void Launch()
    {
        flying = true;
        mover.enabled = false;
        pitch = 0f;
        velocity = Vector3.up * launchSpeed;
    }

    void Fly(Keyboard k)
    {
        float dt = Time.deltaTime;

        // Escape hatch. Handy if you have drifted somewhere the landing check
        // cannot reach you, which the flat-ground assumption makes possible.
        if (k.qKey.wasPressedThisFrame)
        {
            Land();
            return;
        }

        // Steer. Positive X rotation is nose-down in Unity, so W increases pitch.
        float pitchInput = (k.wKey.isPressed ? 1 : 0) - (k.sKey.isPressed ? 1 : 0);
        float turnInput = (k.dKey.isPressed ? 1 : 0) - (k.aKey.isPressed ? 1 : 0);

        pitch = Mathf.Clamp(pitch + pitchInput * pitchSpeed * dt, -70f, 70f);
        float yaw = transform.eulerAngles.y + turnInput * turnSpeed * dt;
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        if (k.spaceKey.wasPressedThisFrame)
            velocity += transform.forward * boostSpeed;

        velocity += Vector3.up * gravity * dt;
        velocity *= 1f - drag * dt;

        // The whole glide in one line: rotate the velocity vector toward wherever
        // the nose points while keeping its length. Falling therefore turns into
        // forward speed when you dive, and back into height when you pull up.
        float speed = velocity.magnitude;
        if (speed > 0.01f)
            velocity = Vector3.Slerp(velocity.normalized, transform.forward, glideStrength * dt) * speed;

        transform.position += velocity * dt;

        if (transform.position.y <= groundY) Land();
    }

    void Land()
    {
        flying = false;
        velocity = Vector3.zero;

        // Sit back down flat, or KeyboardMover inherits our pitch and walks diagonally.
        transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        mover.enabled = true;
    }
}
