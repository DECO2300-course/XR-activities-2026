using UnityEngine;

public class DebugRotator : MonoBehaviour
{
    public float rotationSpeed = 45f;
    public float currentRotation = 0f;
    
    // Add these new debug variables
    public Vector3 objectPosition;
    public string objectName;
    public bool isRotating = true;

    void Start()
    {
        // Initialise debug variables
        objectPosition = transform.position;
        objectName = gameObject.name;
    }

    void Update()
    {
        // Update position debug variable
        objectPosition = transform.position;
        
        if (isRotating)
        {
            // Calculate the rotation value and store it in a variable
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            currentRotation += rotationThisFrame;
            
            // Apply the rotation using currentRotation as absolute angle
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);
            
            // Debug: Print the current rotation value to console
            Debug.Log("Current Rotation: " + currentRotation);
        }
    }
}
