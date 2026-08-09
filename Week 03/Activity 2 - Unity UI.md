# Activity 2: Unity UI Elements

## Objective
Learn to create and manipulate Unity UI elements, including Text components, and understand the difference between Screen Space and World Space UI canvases.

## Prerequisites
- Complete Activity 1 to have a basic scene with debugging setup
- Understanding of Unity Editor windows and basic scripting concepts

## Instructions

### Step 1: Prepare Your Scene
1. Open your Unity project from Activity 1
2. Ensure you have your DebugCube with the DebugRotator script attached
3. Make sure the scene is saved

### Step 2: Create a UI Canvas
1. **Create a Text - TextMeshPro element**:
   - Right-click in the Hierarchy window
   - Select `UI > Text - TextMeshPro`
   - If prompted to import TMP Essentials, click "Import TMP Essentials" (But don't import the Extras unless you want to play with them)
   - **Note**: If there's no Canvas in your scene, Unity will automatically create one for you

2. **Understand the Canvas**:
   - The Canvas is the container for all UI elements
   - Unity automatically creates a "Canvas" object and an "EventSystem" when needed
   - The EventSystem handles user input like mouse clicks and touch events for UI elements
   - UI elements must be children of a Canvas to work properly

### Step 3: Configure the Text Element
1. **Configure the Text element**:
   - Rename it to "OnScreenText"
   - In the Inspector, set the text to "Rotation: 0"
   
2. **Position the text in the top-left corner:**
   - Select the OnScreenText object in the Hierarchy
   - In the Inspector, find the "Rect Transform" component (it's usually at the top)
   - Look for the anchor preset button (it looks like a square with arrows, currently showing "middle center")
   - Hold Alt (Windows) or Option (Mac) AND Shift at the same time
   - Click the top-left corner preset (top-left position in the anchor grid)
   - This automatically sets everything up correctly for top-left positioning
   - Finally, set the position to (10, -10, 0) to give it a small margin from the screen edge
   
3. **Style the text:**
   - Adjust the font size to 24
   - Change the colour to white for visibility
   - Press play and look at your on screen text!

### Step 4: Create a UI Manager Script
1. **Create a new script**:
   - In the Project window, right-click in the Scripts folder
   - Select `Create > C# Script`
   - Name it `UIManager`

2. **Write the UI Manager script**:
```csharp
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI onScreenText;
    public DebugRotator targetRotator;

    void Update()
    {
        // Update the UI text with the current rotation value
        onScreenText.text = "Rotation: " + targetRotator.currentRotation.ToString("F1");
    }
}
```

**Note**: If you're getting null reference errors, make sure you've properly connected the variable references in the Inspector.

### Step 5: Connect the UI to Your Script
1. **Attach the UIManager script**:
   - Select the Canvas in the Hierarchy
   - In the Inspector, click `Add Component`
   - Search for "UIManager" and add it

2. **Connect the references**:
   - In the UIManager component, drag your OnScreenText Text element to the "On Screen Text" field
   - Drag your DebugCube (with DebugRotator script) to the "Target Rotator" field
   
   **Why we assign references this way:**
   - Unity needs to know which specific objects your script should work with
   - By dragging objects into these fields, you're telling the script "use this specific Text element" and "use this specific DebugRotator script"
   - This is called "reference assignment" - you're connecting your script to the actual objects in your scene
   - Without these connections, the script doesn't know which objects to update

   **Alternative: Set up references in code (Start function):**
   ```csharp
   void Start()
   {
       // Find the Text element by name
       onScreenText = GameObject.Find("OnScreenText").GetComponent<TextMeshProUGUI>();
       
       // Find the DebugRotator script by name
       targetRotator = GameObject.Find("DebugCube").GetComponent<DebugRotator>();
   }
   ```
   *Note: The drag-and-drop method is usually preferred because it's easier to do. but if the reference is lost, it can be harder to see.*

3. **Test the connection**:
   - Click Play
   - You should see the rotation value updating in real-time on the UI

4. **Now turn the Console logging off**:
   - Open `DebugRotator` and comment out the `Debug.Log` line:
   ```csharp
   // Debug.Log("Current Rotation: " + currentRotation);
   ```
   - You have a better readout now. The UI shows you the same number, once, in a place you can
     read at a glance.
   - More importantly, that line was printing **every frame**. A Console scrolling past at sixty
     lines a second is not a debugging tool, it is a wall: the one warning that actually matters
     scrolls off the top before you see it. Log when you need to, then slow it down or stop.

### Step 6: Explore Screen Space vs World Space
1. **Current Setup (Screen Space - Overlay)**:
   - Your UI is currently in "Screen Space - Overlay" mode
   - This means UI elements are drawn on top of everything else
   - They stay in the same position regardless of camera movement

2. **Switch to World Space**:
   - Select the Canvas in the Hierarchy
   - In the Inspector, find the Canvas component
   - Change "Render Mode" from "Screen Space - Overlay" to "World Space"
   
   **Why World Space for XR:**
   - In VR/AR, World Space UI allows users to interact with interface elements as if they were physical objects in 3D space
   - Users can point at UI elements with controllers or hands, making interactions more natural and intuitive
   - World Space UI can be positioned anywhere in the 3D environment, allowing for spatial organisation of information and controls

3. **Position the World Space UI**:
   - The Canvas will now appear as a 3D object in your scene
   - Use the Transform tools to position it beyond your cube, so the cube sits between you and it
   - Scale it down to a reasonable size (try Scale = 0.01, 0.01, 0.01)
   - Position it at something like (0, 2, 3) to sit above and beyond your cube. The default camera
     sits at negative Z looking towards positive Z, so a larger Z puts the canvas *past* the cube;
     a negative Z would drop it between you and the cube instead

4. **Test World Space UI**:
   - Click Play
   - Move your camera around to see how the UI behaves in 3D space
   - The UI now exists as a 3D object that can be viewed from different angles

### Step 7: Create Multiple UI Elements
1. **Add more UI elements**:
   - Right-click on the Canvas → UI → Text - TextMeshPro
   - Create elements for:
     - "Position Display" (shows object position)
     - "Speed Display" (shows rotation speed)
     - "Status Display" (shows if rotating or not)

2. **Update the UIManager script**:
```csharp
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI rotationDisplay;
    public TextMeshProUGUI positionDisplay;
    public TextMeshProUGUI speedDisplay;
    public TextMeshProUGUI statusDisplay;
    public DebugRotator targetRotator;

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
    }
}
```

> **Your working text display just went blank. That is expected.** The `onScreenText` field is
> gone — it is called `rotationDisplay` now. Unity stores Inspector references against the *field
> name*, so renaming a field means Unity has nothing to match the old reference to, and it quietly
> drops it. No error, no warning, just an empty slot and a display that stopped updating.
>
> This is why a rename can "break" a scene that compiles perfectly well. When something you had
> working goes dead right after you edited a script, check the Inspector for empty fields first.

3. **Connect all the new UI elements**:
   - Drag your existing **OnScreenText** object into the new **Rotation Display** field — this is
     the reference the rename dropped. Rename the object to "RotationDisplay" too, so the Hierarchy
     matches the script
   - Drag each new Text element to its corresponding field in the UIManager component

### Step 8: Create Interactive UI Elements
1. **Add a Button**:
   - Right-click on the Canvas → UI → Button - TextMeshPro
   - Rename it to "ToggleRotationButton"
   - Set the button text to "Toggle Rotation"

2. **Create a Button Handler script**:
```csharp
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public DebugRotator targetRotator;
    public Button toggleButton;

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleRotation);
    }

    void ToggleRotation()
    {
        targetRotator.isRotating = !targetRotator.isRotating;
    }
}
```


3. **Connect the button**:
   - Attach the ButtonHandler script to the Canvas
   - Drag your DebugCube to the "Target Rotator" field
   - Drag the ToggleRotationButton to the "Toggle Button" field

**Note**: If you're getting null reference errors, make sure you've properly connected the variable references in the Inspector.


## Understanding UI Concepts

### Canvas Render Modes
- **Screen Space - Overlay**: UI drawn on top of everything, always visible
- **Screen Space - Camera**: UI drawn relative to a specific camera
- **World Space**: UI exists as 3D objects in the scene

### UI Components
- **Text**: Displays text information
- **Button**: Interactive clickable elements
- **Image**: Displays images or coloured rectangles
- **Panel**: Container for organising other UI elements

### UI Positioning
- **Anchors**: Control how UI elements position relative to screen edges
- **Pivot**: The center point around which UI elements rotate/scale
- **Rect Transform**: Special transform for UI elements

## Extension Activities

### **Display More Information**
- Add UI elements to show the cube's scale, rotation speed, and current position
- Create a status panel that shows whether the cube is moving or stationary


### **Add More Interactive Buttons**
- Create a button to reset the cube's position to (0, 0, 0)
- Add a button to change the cube's colour or material
- Make a button that toggles the cube's visibility on/off
- Create buttons to change the rotation speed (slow, normal, fast)

### **Organise Your UI**
- Group related information into separate panels
- Use different colours for different types of information
- Arrange UI elements in a logical order (position info at top, controls at bottom)

### **Test Different UI Positions**
- Try moving your UI elements to different screen positions
- Your canvas is an object. Try making a script to move it around. 

## Outcome
A good understanding of Unity UI system, including creating text displays, connecting UI to scripts, and understanding the differences between Screen Space and World Space UI. You should be able to create dynamic UI that updates in real-time and responds to user interactions.

## Save Your Work
**Don't forget to save your scene and project!**
- Press Ctrl+S (Windows) or Cmd+S (Mac) to save your scene
- Go to File → Save Project to save all your work
