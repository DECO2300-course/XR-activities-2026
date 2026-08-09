using UnityEngine;

public class UsefulMath : MonoBehaviour
{
    public Transform debugCube;
    public Transform fallingCube;
    public Transform targetCube;
    
    public float distanceToFallingCube;
    
    public float angleToFallingCube;
    public Vector3 directionToFallingCube;
    
    private Vector3 fallingCubeStartPosition;

    void Start()
    {
        // Store the starting position of the falling cube
        fallingCubeStartPosition = fallingCube.position;
    }

    void Update()
    {
        // Calculate distance between debug cube and falling cube
        distanceToFallingCube = Vector3.Distance(debugCube.position, fallingCube.position);
        
        // Calculate direction vector from debug cube to falling cube
        directionToFallingCube = (fallingCube.position - debugCube.position).normalized;
        
        // Calculate angle between debug cube's forward direction and direction to falling cube
        Vector3 forward = debugCube.forward;
        angleToFallingCube = Vector3.Angle(forward, directionToFallingCube);
        
        // Make target cube look at falling cube
        targetCube.LookAt(fallingCube);
    }
    
    void OnDrawGizmos()
    {
        // Draw the debug cube's forward direction (green ray)
        Gizmos.color = Color.green;
        Gizmos.DrawRay(debugCube.position, debugCube.forward * 2f);
        
        // Draw the path from falling cube's start position to current position (blue ray)
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(fallingCubeStartPosition, fallingCube.position);
        
        // Draw the line from falling cube to target cube (red ray)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(fallingCube.position, targetCube.position);
    }
}
