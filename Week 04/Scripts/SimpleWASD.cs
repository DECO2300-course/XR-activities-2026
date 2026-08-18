using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleWASD : MonoBehaviour
{
    public float moveSpeed = 5f;

    InputAction moveAction;

    void Start()
    {
        // Find the Move action from the project's input actions asset
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        // Read the action as a 2D direction (-1 to 1 on each axis)
        Vector2 input = moveAction.ReadValue<Vector2>();

        // The action's Y (W and S) drives forward/backward movement
        Vector3 movement = new Vector3(0f, 0f, input.y);

        // The action's X (A and D) drives rotation (turning left and right)
        transform.Rotate(0f, input.x * 90f * Time.deltaTime, 0f);

        // For efficiency, we check to make sure that we need to move before calling Translate
        if (movement != Vector3.zero)
        {
            transform.Translate(movement.normalized * moveSpeed * Time.deltaTime, Space.Self);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a debug ray pointing forward from the player, dropped to the height of an
        // object resting on the ground so it shows where a grab ray would really go
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + Vector3.down * 0.5f, transform.forward * 3f);
    }
}
