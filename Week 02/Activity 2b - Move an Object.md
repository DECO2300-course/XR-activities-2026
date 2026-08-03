# Activity 2b: Explore Movement with Translate

## Objective
Learn to use the `Translate` method to move objects in Unity, building on the rotation concepts from Activity 2a.

## Prerequisites
- Complete Activity 2a to understand basic scripting and the `Rotate` method
- Basic understanding of Unity Editor and C# scripting

## Instructions

### Step 1: Create the Mover Script
1. In the Project window, right-click and select `Create > C# Script`
2. Name it `Mover`
3. Double-click to open it and replace the code with:

```csharp
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 moveDirection = Vector3.forward;

    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}
```

### Step 2: Test Basic Movement
1. Create a new Cube in your scene (right-click in Hierarchy > 3D Object > Cube)
2. Rename it to "MovingCube"
3. Attach the `Mover` script to the cube
4. Play the scene and watch it move forward

### Step 3: Experiment with Movement Directions
In the Inspector, try different `Move Direction` values:
- `(0, 0, 1)` - moves forward (Z-axis)
- `(1, 0, 0)` - moves right (X-axis)
- `(0, 1, 0)` - moves up (Y-axis)
- `(1, 0, 1)` - moves diagonally forward and right
- `(-1, 0, 0)` - moves left (negative X)
- `(0, -1, 0)` - moves down (negative Y)

### Step 4: Understand the Differences
Compare `Rotate` vs `Translate`:
- **Rotate**: Changes the object's orientation (which way it faces)
- **Translate**: Changes the object's position (where it is in the world)
- **Space.World**: Movement relative to world coordinates
- **Space.Self**: Movement relative to the object's own rotation

### Step 5: Combine Rotation and Movement
1. Attach both `Rotator` and `Mover` scripts to the same object
2. Watch it rotate while moving in a straight line
3. Try changing the movement direction to see how rotation affects movement
4. Experiment with different rotation and movement speeds



## Understanding the Code

### `public Vector3 moveDirection = Vector3.forward;`
- `Vector3` represents a 3D direction (X, Y, Z)
- `Vector3.forward` is shorthand for `(0, 0, 1)`
- You can adjust this in the Inspector

### `transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);`
- `Translate()` changes the object's position
- `Space.World` means move relative to world coordinates
- `Space.Self` would move relative to the object's rotation




## Extension Activities

### Create a Circular Path
Modify the script to make the object move in a circle:

```csharp
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float radius = 3f;
    
    private Vector3 centerPosition;

    void Start()
    {
        centerPosition = transform.position;
    }

    void Update()
    {
        float angle = Time.time * moveSpeed;
        float x = centerPosition.x + Mathf.Cos(angle) * radius;
        float z = centerPosition.z + Mathf.Sin(angle) * radius;
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
```

### Create Multiple Moving Objects
1. Duplicate your moving object (Ctrl+D)
2. Give each object different movement directions
3. Create a scene with objects moving in various patterns
4. Try combining rotation and movement on different objects

### Combined Movement Script
Create a single script that handles both rotation and movement:

#### Challenge Steps:
1. Create a new script called `CombinedMovement.cs`
2. Write a script that includes both rotation and translation variables
3. Make the object rotate while moving in a customizable direction
4. Add Inspector controls for both rotation speed and movement speed

#### Example Solution:
```csharp
using UnityEngine;

public class CombinedMovement : MonoBehaviour
{
    public float rotationSpeed = 45f;
    public Vector3 rotationAxis = Vector3.up;
    
    public float moveSpeed = 5f;
    public Vector3 moveDirection = Vector3.forward;
    
    void Update()
    {
        // Handle rotation
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
        
        // Handle movement
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}
```

#### Advanced Challenge:
- Add a toggle to enable/disable rotation or movement independently
- Create different movement patterns (circular, zigzag, etc.)


## Outcome
Understanding of how to use `Translate` for object movement, ability to create complex movement patterns, and experience combining multiple behaviors on a single object. 