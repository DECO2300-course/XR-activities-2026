# Activity 3: Unity Math and Vector Operations

## Objective
Learn practical Unity math concepts including vector operations, distance calculations, angle measurements, and the LookAt function through hands-on examples.

## Prerequisites
- Complete Activities 1 and 2 to have a basic scene with debugging and UI setup
- Understanding of basic Unity scripting and UI concepts

## Instructions

### Step 1: Prepare Your Scene
1. Open your Unity project from Activity 2
2. Ensure you have your DebugCube with the DebugRotator script
3. Make sure your UI is working and displaying rotation values

### Step 2: Create Additional Objects for Math Examples
1. **Add a second cube**:
   - GameObject → 3D Object → Cube
   - Rename it to "TargetCube"
   - Position it at (3, 0, 0) using the Inspector

2. **Add a platform**:
   - GameObject → 3D Object → Cube
   - Rename it to "Platform"
   - Position it at (3, -2, 0)
   - Scale it to (2, 0.5, 2) to make it wider and flatter

3. **Add a third cube with Rigidbody**:
   - GameObject → 3D Object → Cube
   - Rename it to "FallingCube"
   - Position it at (-3, 2, 0)
   - Add a Rigidbody component: Add Component → Physics → Rigidbody

4. **Test your scene setup**:
   - Click Play to test your scene
   - You should see three cubes: one rotating (DebugCube), one stationary (TargetCube), and one falling (FallingCube)
   - Stop the scene when you're satisfied everything is working

### Step 3: Start with Distance Calculation
1. **Create a new script**:
   - In the Project window, right-click in the Scripts folder
   - Select `Create > C# Script`
   - Name it `UsefulMath`

2. **Write the basic distance calculator**:
```csharp
using UnityEngine;

public class UsefulMath : MonoBehaviour
{
    public Transform debugCube;
    public Transform fallingCube;
    
    public float distanceToFallingCube;

    void Update()
    {
        // Calculate distance between debug cube and falling cube
        distanceToFallingCube = Vector3.Distance(debugCube.position, fallingCube.position);
    }
}
```

3. **Set up the distance calculator**:
   - Attach the script to your DebugCube
   - Drag your DebugCube to the "Debug Cube" field
   - Drag your FallingCube to the "Falling Cube" field

4. **Test the distance calculation**:
   - Click Play
   - Watch the distance value in the Inspector as the FallingCube falls
   - The distance starts near 3.6, dips to 3.0 as the cube passes the DebugCube's height, then climbs as it keeps falling. The dip is the closest the two cubes ever get

5. **Experiment with the falling speed**:
   - Select the FallingCube in the Hierarchy
   - In the Rigidbody component, try changing the **Linear Damping** value (higher = slower fall)
   - Try changing **Mass** as well. It makes no difference on its own: gravity accelerates everything equally, so a heavy cube and a light one fall together. Mass only starts to matter once Linear Damping is above zero
   - Watch how this affects the distance calculation over time

### Step 4: Add Angle Calculation
1. **Update your UsefulMath script** to include angle calculation:
```csharp
using UnityEngine;

public class UsefulMath : MonoBehaviour
{
    public Transform debugCube;
    public Transform fallingCube;
    
    public float distanceToFallingCube;
    
    public float angleToFallingCube;
    public Vector3 directionToFallingCube;

    void Update()
    {
        // Calculate distance between debug cube and falling cube
        distanceToFallingCube = Vector3.Distance(debugCube.position, fallingCube.position);
        
        // Calculate direction vector from debug cube to falling cube
        directionToFallingCube = (fallingCube.position - debugCube.position).normalized;
        
        // Calculate angle between debug cube's forward direction and direction to falling cube
        Vector3 forward = debugCube.forward;
        angleToFallingCube = Vector3.Angle(forward, directionToFallingCube);
    }
}
```

2. **Test the angle calculation**:
   - Click Play
   - Watch both the distance and angle values in the Inspector
   - The angle will change as the FallingCube falls and as the DebugCube rotates
   - Notice how the angle changes relative to the DebugCube's rotation

3. **Experiment with rotation speed**:
   - Select the DebugCube in the Hierarchy
   - In the DebugRotator component, try changing the "Rotation Speed" value
   - Watch how this affects the angle calculation over time
   - Try setting it to 0 to see the angle change only due to the falling cube

### Step 5: Add LookAt Function
1. **Update your UsefulMath script** to include LookAt functionality:
```csharp
using UnityEngine;

public class UsefulMath : MonoBehaviour
{
    public Transform debugCube;
    public Transform fallingCube;
    public Transform targetCube;
    
    public float distanceToFallingCube;
    
    public float angleToFallingCube;
    public Vector3 directionToFallingCube;

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
}
```

2. **Connect the TargetCube**:
   - Drag your TargetCube to the "Target Cube" field in the UsefulMath component

3. **Test the LookAt function**:
   - Click Play
   - Watch the TargetCube automatically rotate to face the FallingCube
   - The TargetCube will continuously track the FallingCube as it falls

4. **Experiment with different positions**:
   - Try moving the TargetCube to different positions
   - Watch how it rotates to track the FallingCube from different angles
   - Try moving the FallingCube to different starting positions
   - Notice how the LookAt function always makes the TargetCube face the FallingCube

### Step 6: Visualise Vectors with Debug Rays
Debug rays are visual lines that help you see mathematical relationships in 3D space. They're drawn in the Scene view and show you the direction and magnitude of vectors in real-time. This makes it much easier to understand how objects relate to each other and how mathematical calculations work in 3D.

1. **Update your UsefulMath script** to add visual debug rays:
```csharp
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
```

2. **Test the visual debug rays**:
   - Click Play
   - In the Scene view, you'll see three different coloured rays:
     - **Green ray**: Shows the DebugCube's forward direction
     - **Blue line**: Shows the path the FallingCube has traveled from its start position
     - **Red line**: Shows the direct line from FallingCube to TargetCube
   - Watch how these rays change as the cubes move and rotate

3. **Experiment with the visual rays**:
   - Try changing the DebugCube's rotation speed to see how the green ray rotates
   - Move the FallingCube to different starting positions to see how the blue path changes
   - Move the TargetCube to different positions to see how the red line updates
   - Notice how the LookAt function makes the TargetCube always face the FallingCube

> **Expect null reference errors here, and understand why.** The moment you add this script,
> the Console will start filling with `NullReferenceException` from `OnDrawGizmos` — before you
> have touched Play.
>
> `OnDrawGizmos` is not like `Update`. Unity calls it continuously in the **Editor**, whenever it
> redraws the Scene view, whether or not the game is running. So it runs in the gap between you
> adding the component and you dragging the three Transforms into the Inspector — and in that gap
> `debugCube`, `fallingCube` and `targetCube` are all still `null`. Asking a `null` for its
> `.position` is the error.
>
> Fill in the three fields and it stops. This is a whole family of Unity bugs: **code that runs 
> against references that are not ready yet.** You will see it again with `Awake` versus `Start`, 
> and with prefabs that instantiate before their dependencies exist.

### Step 7: Understanding Dot and Cross Products
1. **Create a new scene**:
   - In the Project window, right-click in an empty area
   - Select Create → Scene
   - Name it "VectorProducts"
   - Double-click the new scene to open it

2. **Add a rotating cube**:
   - GameObject → 3D Object → Cube
   - Rename it to "RotatingCube"
   - Position it at (0, 0, 0)
   - Add the DebugRotator script from Activity 1
   - Set the rotation speed to 30

3. **Add a target cube**:
   - GameObject → 3D Object → Cube
   - Rename it to "TargetCube"
   - Position it at (3, 0, 0)

4. **Create a new script** for dot and cross product visualisation:
```csharp
using UnityEngine;

public class VectorProducts : MonoBehaviour
{
    public Transform vectorA;
    public Transform vectorB;
    
    public float dotProduct;
    public float crossProductMagnitude;
    public Vector3 crossProductVector;
    
    void Update()
    {
        // Calculate dot product (measures how aligned two vectors are)
        Vector3 directionA = vectorA.forward;
        Vector3 directionB = (vectorB.position - vectorA.position).normalized;
        dotProduct = Vector3.Dot(directionA, directionB);
        
        // Calculate cross product (creates a perpendicular vector)
        crossProductVector = Vector3.Cross(directionA, directionB);
        crossProductMagnitude = crossProductVector.magnitude;
    }
    
    void OnDrawGizmos()
    {
        // Draw vector A (blue)
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(vectorA.position, vectorA.forward * 2f);
        
        // Draw vector B (red)
        Gizmos.color = Color.red;
        Gizmos.DrawRay(vectorA.position, (vectorB.position - vectorA.position).normalized * 2f);
        
        // Draw cross product vector (green) - perpendicular to both A and B
        Gizmos.color = Color.green;
        Gizmos.DrawRay(vectorA.position, crossProductVector * 1f);
        
        // Draw a small sphere at the end of the cross product vector
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(vectorA.position + crossProductVector * 1f, 0.2f);
        
        // Visualise dot product with colour intensity and sphere size
        // The closer the dot product is to 1, the more aligned the vectors are
        float alignment = (dotProduct + 1f) / 2f; // Convert from [-1,1] to [0,1]
        Color dotColor = Color.Lerp(Color.red, Color.green, alignment);
        Gizmos.color = dotColor;
        
        // Draw a sphere whose size and colour show the dot product
        float sphereSize = 0.3f + (alignment * 0.5f); // Size varies from 0.3 to 0.8
        Gizmos.DrawWireSphere(vectorA.position, sphereSize);
        
        // Draw a line connecting the vectors when they're well-aligned
        if (dotProduct > 0.7f)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(vectorA.position, vectorB.position);
        }
    }
}
```

5. **Set up the vector products script**:
   - Attach the script to your RotatingCube
   - Drag your RotatingCube to the "Vector A" field
   - Drag your TargetCube to the "Vector B" field

6. **Test the dot and cross products**:
   - Click Play and watch the Inspector values
   - **Dot Product**: Ranges from -1 to 1
     - **1**: Vectors point in the same direction
     - **0**: Vectors are perpendicular (90° angle)
     - **-1**: Vectors point in opposite directions
     - **Visual**: A wire sphere around Vector A changes colour and size based on alignment
       - **Red sphere (small)**: Vectors point in opposite directions
       - **Green sphere (large)**: Vectors point in the same direction
       - **Cyan line**: Appears when vectors are well-aligned (dot product > 0.7)
   - **Cross Product**: Creates a vector perpendicular to both input vectors
     - The green ray shows this perpendicular vector
     - The yellow sphere marks the end of the cross product vector

7. **Experiment with the products**:
   - Watch the RotatingCube rotate and see how the dot product changes
   - Move the TargetCube to different positions
   - Watch how the cross product vector always stays perpendicular to both input vectors
   - Try making the vectors parallel (dot product = 1 or -1) and see the cross product become zero

8. **Go back to your main scene**:
   - Save the VectorProducts scene (Ctrl+S)
   - In the Project window, double-click your main scene to reopen it
   - Activity 4 carries on with the DebugCube, TargetCube, Platform and FallingCube, which live in
     that scene and not in this one

## Understanding Unity Math Concepts

### Vector3 Operations
- **Vector3.Distance()**: Calculates the straight-line distance between two points
- **Vector3.Angle()**: Calculates the angle between two vectors
- **Vector3.Dot()**: Measures how aligned two vectors are (-1 to 1)
- **Vector3.Cross()**: Creates a perpendicular vector (useful for normals)

### Dot and Cross Product Applications
- **Dot Product Uses**:
  - **Collision Detection**: Check if objects are facing each other
  - **Lighting**: Calculate how much light hits a surface
  - **AI Behaviour**: Determine if an enemy can see the player
  - **Movement**: Check if player is moving toward a target
- **Cross Product Uses**:
  - **Surface Normals**: Calculate the direction a surface is facing
  - **Camera Orientation**: Determine up direction for cameras
  - **Physics**: Calculate torque and angular momentum
  - **3D Modeling**: Create perpendicular vectors for coordinate systems

### Transform Operations
- **LookAt()**: Rotates an object to face another object
- **position**: The 3D coordinates of an object
- **forward**: The direction the object is facing
- **up**: The upward direction of the object

### Practical Applications
- **Distance**: Used for collision detection, AI behaviour, and UI positioning
- **Angles**: Used for rotation, aiming, and movement calculations
- **LookAt**: Used for cameras, AI targeting, and object orientation

## Extension Activities

### **Display Math Values in UI**
1. **Add new UI elements**:
   - Right-click on your Canvas → UI → Text - TextMeshPro
   - Create elements for:
     - "Distance Display"
     - "Angle Display"

2. **Update your UIManager script**:
```csharp
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI rotationDisplay;
    public TextMeshProUGUI positionDisplay;
    public TextMeshProUGUI speedDisplay;
    public TextMeshProUGUI statusDisplay;
    
    // New math display elements
    public TextMeshProUGUI distanceDisplay;
    public TextMeshProUGUI angleDisplay;
    
    public DebugRotator targetRotator;
    public UsefulMath usefulMath;

    void Update()
    {
        // Update rotation display
        rotationDisplay.text = "Rotation: " + targetRotator.currentRotation.ToString("F1");
        
        // Update position display
        positionDisplay.text = "Position: " + targetRotator.objectPosition.ToString("F2");
        
        // Update speed display
        speedDisplay.text = "Speed: " + targetRotator.rotationSpeed.ToString("F1");
        
        // Update status display
        statusDisplay.text = "Status: " + (targetRotator.isRotating ? "Rotating" : "Stopped");
        
        // Update math displays
        distanceDisplay.text = "Distance to Falling Cube: " + usefulMath.distanceToFallingCube.ToString("F2");
        
        angleDisplay.text = "Angle to Falling Cube: " + usefulMath.angleToFallingCube.ToString("F1") + "°";
    }
}
```

3. **Connect the new UI elements**:
   - Drag each new Text element to its corresponding field in the UIManager component
   - Drag your UsefulMath component to the "Useful Math" field

**Note**: If you're getting null reference errors, make sure you've properly connected all the variable references in the Inspector.

### **Look at Vector Products**
- Watch the values of vector products in the demo. 
- What does the dot product do? 
- what can it tell you about things in 3d space? 
- How could you use it in XR? 
- What does the cross product do?
- what can it tell you about things in 3d space? 
- How could you use it in XR? 


## Outcome
A basic understanding of Unity's mathematical tools, including vector operations, distance calculations, angle measurements, and the LookAt function. You should be able to use these concepts to create dynamic, interactive behaviours and understand how mathematical concepts apply to game development.

## Save Your Work
**Don't forget to save your scene and project!**
- Press Ctrl+S (Windows) or Cmd+S (Mac) to save your scene
- Go to File → Save Project to save all your work
