# Activity 2a: Rotate an Object with a Script

## Objective
Learn to create and attach C# scripts to GameObjects in Unity, creating continuous rotation behavior.

## Prerequisites
- Complete Activity 1 to have a basic scene with objects
- Basic understanding of Unity Editor windows (Hierarchy, Inspector, Project)

## Instructions

### Step 1: Prepare Your Scene
1. Open your Unity project from Activity 1
2. In the Hierarchy window, select a Cube or Sphere object
3. If you don't have objects, create a new Cube:
   - Right-click in the Hierarchy window
   - Select `3D Object > Cube`
   - Rename it to "RotatingCube"

### Step 2: Create a New Script
1. In the Project window, right-click in an empty area
2. Select `Create > C# Script`
3. Name the script `Rotator` (double-click to rename if needed)
4. Double-click the script to open it in your code editor (VS Code or Rider — see
   [Software and Frameworks](../Guides/Software_and_Frameworks.md))

### Step 3: Write the Script Code
1. In your code editor, replace all existing code with:
```csharp
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 45f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
```

### Step 4: Save and Return to Unity
1. Save the script file (Ctrl+S)
2. Return to Unity Editor
3. Wait for Unity to compile the script

### Step 5: Attach the Script to an Object
1. Select your Cube/Sphere in the Hierarchy window
2. In the Inspector window, click the `Add Component` button
3. Type "Rotator" in the search box
4. Click on the `Rotator` script to attach it
5. You should now see the Rotator component in the Inspector

### Step 6: Test the Rotation
1. Click the Play button at the top of the Unity Editor
2. Watch your object rotate around its Y-axis
3. Click the Play button again to stop the scene

### Step 7: Adjust the Rotation Speed
1. While the scene is not playing, select your object
2. In the Inspector, find the Rotator component
3. Change the `Rotation Speed` value:
   - Higher values = faster rotation
   - Lower values = slower rotation
   - Negative values = reverse rotation
4. Play the scene again to see the difference

## Understanding the Code

### `public float rotationSpeed = 45f;`
- This creates a 'public' variable that appears in the Inspector
- You can adjust it without changing the code
- The `f` means it's a float (decimal number)

### `void Update()`
- This function runs once every frame
- How many frames per second you get is **not fixed** — it depends on the machine, the
  scene, and the display. A Quest headset runs at 72, 90, or 120 frames per second
  depending on the model and settings
- Perfect for continuous movement like rotation

### `transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);`
- `transform` refers to the object's position, rotation, and scale
- `Rotate()` changes the object's rotation
- `Vector3.up` means rotate around the Y-axis (up/down)
- `Time.deltaTime` is how many seconds the last frame took. Multiplying by it converts
  "45 degrees per *frame*" into "45 degrees per *second*"

> **Why `Time.deltaTime` matters more than it looks.** Without it, your object spins at a
> speed that depends on the frame rate — faster on a good computer, slower on a busy one,
> and different again on a headset. Multiplying by `Time.deltaTime` makes the motion take
> the same real-world time everywhere. Get into this habit now: anything you animate by
> hand in `Update()` needs it, and it is the single most common cause of "it worked on my
> laptop but not in the headset".


## Extension Activities

### Try Different Rotation Axes
Change `Vector3.up` to:
- `Vector3.right` (X-axis rotation)
- `Vector3.forward` (Z-axis rotation)
- `new Vector3(1, 1, 0)` (diagonal rotation)

### Add Multiple Rotating Objects
1. Duplicate your rotating object (Ctrl+D)
2. Give each object different rotation speeds
3. Create a variety of rotating objects in your scene

### Next Steps
After completing this activity, try Activity 2b to explore movement with the `Translate` method!

## Outcome
An object that continuously rotates around its Y-axis when the scene is played, with the ability to adjust rotation speed through the Inspector. 


