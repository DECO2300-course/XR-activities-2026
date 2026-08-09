# Activity 1: Introduction to Editor Debugging

## Objective
Learn Unity debugging techniques using the Console window, public variables, and the Inspector to monitor and modify script values in real-time.

## Prerequisites
- Basic familiarity with Unity Editor windows (Scene, Game, Hierarchy, Inspector, Project)
- Understanding of basic C# scripting concepts from Week 02

## Instructions

### Step 1: Project Setup
1. Open Unity Hub and create a new project with the **Universal 3D** template, **inside your course
   repository**. Week 3 gets its own project — Activities 1 to 4 all build on this one, so make it
   somewhere you are happy to keep working
2. **Save your scene immediately** (Ctrl+S) and name it "Week3_Activity1"
3. **Create an organised folder structure** in the Project window:
   - Right-click in Project window → Create → Folder
   - Name it "Scripts"
   - Create additional folders: "Materials", "Textures"

### Step 2: Create a Basic Scene
1. **Add a cube to your scene**:
   - GameObject → 3D Object → Cube
   - Rename it to "DebugCube"
   - Position it at (0, 0, 0) using the Inspector

### Step 3: Create a Rotator Script
1. **Create a new script**:
   - In the Project window, right-click in the Scripts folder
   - Select `Create > C# Script`
   - Name it `DebugRotator` (Script names can be anything. Keep it descriptive. If you change it you will need to change it inside the code too)

2. **Write the script**:
```csharp
using UnityEngine;

public class DebugRotator : MonoBehaviour
{
    public float rotationSpeed = 45f;
    private float currentRotation = 0f;

    void Update()
    {
        // Calculate the rotation value and store it in a variable
        float rotationThisFrame = rotationSpeed * Time.deltaTime;
        currentRotation += rotationThisFrame;
        
        // Apply the rotation using currentRotation as absolute angle
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    
        // This is different from last week where we were rotating it by an amount with each call.
        // transform.Rotate(Vector3.up * rotationThisFrame);
        
        // Debug: Print the current rotation value to console
        Debug.Log("Current Rotation: " + currentRotation);
    }
}
```

### Step 4: Test Basic Rotation
1. **Attach the script to your cube**:
   - Select the DebugCube in the Hierarchy
   - In the Inspector, click `Add Component`
   - Search for "DebugRotator" and add it

### Step 5: Explore the Console Window
1. **Open the Console window**:
   - Window → General → Console
   - Position it where you can see it while playing

2. **Observe the debug output**:
   - Click the Play button
   - Watch your cube rotate and check the Console window
   - Each frame prints the current rotation value
   - Click the Clear button to clean up the console

### Step 6: Make Variables Public
1. **Modify the script** to make currentRotation public:
```csharp
using UnityEngine;

public class DebugRotator : MonoBehaviour
{
    public float rotationSpeed = 45f;
    public float currentRotation = 0f;  // Now public!

    void Update()
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
```

2. **Observe the Inspector changes**:
   - Save the script and return to Unity
   - Select your DebugCube
   - In the Inspector, you'll now see both `Rotation Speed` and `Current Rotation` fields
   - The `Current Rotation` value updates in real-time while playing

### Step 7: Experiment with Inspector Values
1. **Test changing values in the Inspector**:
   - While the scene is playing, try changing the `Rotation Speed` value
   - Watch how the rotation speed changes immediately
   - Try setting `Current Rotation` to different values

2. **Test with rotation disabled**:
   - Comment out the rotation update calculation in your script:
```csharp
// currentRotation += rotationThisFrame;
```
   
   - Now you can manually set the rotation in the Inspector
   - Try setting `Current Rotation` to 90, 180, 360 degrees

### Step 8: Debug Different Variable Types
1. **Add more debug variables to your script**:
```csharp
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
```

2. **Test different variable types**:
   - Vector3: Position coordinates
   - String: Object name
   - Bool: Toggle rotation on/off
   - Try changing these values in the Inspector

3. **Experiment with position control**:
   - Try modifying the script to use `objectPosition` to control the object's position
   - Instead of just updating `objectPosition = transform.position;`, try setting `transform.position = objectPosition;`
   - This way you can manually set the object's position in the Inspector
   - Experiment with different position values.

## Understanding Debugging Concepts

### Console Output
- `Debug.Log()` prints messages to the Console window
- Useful for tracking variable values and program flow
- Can help identify when code is running and what values are being calculated

### Public Variables
- Variables marked as `public` appear in the Inspector
- Allow you to modify values without changing code
- Great for testing different values and behaviours

### Variable Types
- **float**: Decimal numbers (rotation speed, position values)
- **Vector3**: 3D coordinates (position, rotation, scale)
- **string**: Text values (object names, messages)
- **bool**: True/false values (toggles, switches)

## Extension Activities

### **Advanced Debugging**
- Add more variable types (int, Color, Material)
- Create debug buttons using `[ContextMenu]` attribute
[ https://docs.unity3d.com/6000.3/Documentation/ScriptReference/ContextMenu.html ]
- Use `Debug.LogWarning()` and `Debug.LogError()` for different message types

### **Conditional Debugging**
- Only print debug messages when certain conditions are met (like the rotation is between certain values)
- Add debug toggles to control what information is displayed

### **Inspector Customization**
- Use `[Header]` and `[Space]` attributes to organise Inspector fields [ https://docs.unity3d.com/6000.3/Documentation/ScriptReference/HeaderAttribute.html ]
- Add `[Range]` attributes to create sliders for numeric values
- Use `[Tooltip]` to add helpful descriptions

## Outcome
A solid understanding of Unity's debugging tools, including Console output, public variables, and Inspector manipulation. You should be able to monitor script behaviour and modify values in real-time to test different scenarios.

## Save Your Work
**Don't forget to save your scene and project!**
- Press Ctrl+S (Windows) or Cmd+S (Mac) to save your scene
- Go to File → Save Project to save all your work
