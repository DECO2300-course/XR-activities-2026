# Activity 3: Use Inputs to Make an Object Move

## Objective
Learn to use Unity's Input System to create player-controlled movement, building on the movement concepts from Activity 2b.

## Prerequisites
- Complete Activity 2b to understand basic movement with `Translate`
- Basic understanding of Unity Editor and C# scripting
- A scene with objects to navigate around (from Activity 1)

## Instructions

### Step 1: Prepare Your Scene
1. **Open your scene from Activity 1** or create a simple test environment
2. **Ensure you have objects to navigate around** (buildings, walls, obstacles)
3. **Save your scene** (Ctrl+S) before adding new scripts

### Step 2: Meet the Input System

Unity 6 handles input through the **Input System** package. You do not need to install or
configure anything — every new project already includes an input actions asset with the
common actions defined for you.

Take a look at what you have been given:

1. Open **Edit → Project Settings → Input System Package**
2. Find the **Project-wide Actions** field near the top — it points at an asset called
   `InputSystem_Actions`
3. Click that asset to open it. Under the **Player** action map you will see actions
   already set up: `Move`, `Look`, `Jump`, `Sprint`, `Interact`, and others
4. Select **Move** and look at its bindings. It is already wired to WASD, the arrow keys,
   and a gamepad's left stick

You will read the `Move` action from your script. Because it is an *action* rather than a
specific key, the same line of code works for keyboard, gamepad, and later on, VR
controllers — you never rewrite the script to support a new device.

> **If you are following an older tutorial.** Anything using `Input.GetAxis()` or
> `Input.GetKey()` is written for Unity's legacy Input Manager. Those calls throw an error
> in a new Unity 6 project, because the legacy system is switched off by default. Use the
> action-based approach below instead.

### Step 3: Create the Player Object
1. **Create a Capsule**: GameObject → 3D Object → Capsule
2. **Rename it properly**: Select the Capsule, name it "Player"
3. **Position it appropriately**: Place it at a starting position like (0, 1, 0)

### Step 4: Create the PlayerMovement Script
1. **Create a new script**: Right-click in Project window → Create → C# Script
2. **Name it "PlayerMovement"**: Double-click to rename if needed
3. **Open the script** and replace all code with:

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
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

        // Map it onto the ground plane: X stays X, Y becomes Z
        Vector3 movement = new Vector3(input.x, 0f, input.y);

        // Apply movement using Transform
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }
}
```

### Step 5: Attach and Configure the Script
1. **Attach the script**: Select the Player object, drag the PlayerMovement script onto it
2. **Configure in Inspector**:
   - Set `Move Speed` to 5 (or adjust as needed)
3. **Save the script** (Ctrl+S) and return to Unity

### Step 6: Test the Movement
1. **Enter Play mode**: Click the Play button
2. **Test movement controls**:
   - **WASD keys**: W (forward), S (backward), A (left), D (right)
   - **Arrow keys**: Up, Down, Left, Right arrows
   - **Gamepad**: Left stick (if connected)

   You did not write a single line of code for any of these individually — they all come
   from the one `Move` action.
3. **Observe the movement**: The player should move smoothly around the scene
4. **Exit Play mode**: Click the Play button again

### Step 7: Adjust and Optimize
1. **Fine-tune the movement**:
   - Adjust `Move Speed` in the Inspector while in Play mode
   - Test different values to find the right feel
2. **Test with your scene objects**:
   - Navigate around buildings, walls, and obstacles
   - Ensure movement feels natural and responsive

## Understanding the Code

### **`InputAction` and `InputSystem.actions`**
- An **action** is something the player wants to do ("move"), not a key they press
- `InputSystem.actions` is the project-wide actions asset you looked at in Step 2
- `FindAction("Move")` looks up the action by name, once, in `Start()`
- Devices are bound to the action in the asset, not in your script — which is why adding
  gamepad support took no code at all

### **`ReadValue<Vector2>()`**
- Reads the action's current value every frame
- `Move` is a 2D action, so it returns a `Vector2` between -1 and 1 on each axis
- Returns `(0, 0)` when nothing is pressed

### **Vector3 Movement**
- `new Vector3(input.x, 0f, input.y)`: turns 2D input into 3D movement
- The action's **Y** becomes the world's **Z**, because pushing "up" on a stick should move
  you *forward* across the ground, not upward into the sky
- `0f` for the Y-axis keeps movement on the ground plane

### **Movement Methods**
- **Transform.Translate()**: Direct position changes (no physics)

### **Public Variables**
- `moveSpeed`: Adjustable in Inspector for easy tuning



## Extension Activities

### **Add Rotation to Movement**
Modify the script to make the player face the direction of movement:

```csharp
// Add this inside the Update() method after movement
if (movement != Vector3.zero)
{
    transform.rotation = Quaternion.LookRotation(movement);
}
```

### **Add Running**
The actions asset already has a `Sprint` action bound to Shift. Find it alongside
`moveAction`, then use it to scale your speed:

```csharp
// Add at class level
InputAction sprintAction;

// Add to Start()
sprintAction = InputSystem.actions.FindAction("Sprint");

// Add to Update(), before you move
float currentSpeed = sprintAction.IsPressed() ? moveSpeed * 2f : moveSpeed;
```

Then use `currentSpeed` instead of `moveSpeed` in your `Translate` call.

- `IsPressed()` is true for as long as the key is held down
- Compare with `WasPressedThisFrame()`, which is true only on the frame it goes down —
  that is the one you want for things that should happen once, like jumping or firing

### **Add Jumping**
The `Jump` action is already bound to Space. Jumping properly needs physics, which comes
in Activity 4 — for now, try making the object hop upward for a moment when `Jump` fires:

```csharp
// Add at class level
InputAction jumpAction;

// Add to Start()
jumpAction = InputSystem.actions.FindAction("Jump");

// Add to Update()
if (jumpAction.WasPressedThisFrame())
{
    Debug.Log("Jump!");
}
```

Get the message appearing first, then work out what to do with it.

### **Add Your Own Action**
Open the `InputSystem_Actions` asset and add a brand new action of your own:
1. Select the **Player** action map and click **+** to add an action
2. Name it something like `Honk`, and set its **Action Type** to **Button**
3. Add a binding, click **Listen**, and press the key you want
4. Save the asset, then read it in your script exactly like the others

This is the workflow you will use for VR controllers later in the semester — the asset
changes, the script does not.

### **Create a Camera Follow**
Add a camera that follows the player:
1. Create an empty GameObject named "CameraHolder"
2. Make the Main Camera a child of CameraHolder
3. Position the camera behind and above the player
4. Make CameraHolder follow the player's position

## Outcome
A player object that responds smoothly to keyboard input, with configurable movement speed. The player can navigate around the scene using WASD or arrow keys. 