# Activity 4: Detecting Interactions Between Objects

## Objective
Learn to use Unity's collision detection system to create interactive objects and trigger events when objects touch each other.

## Prerequisites
- Complete Activity 3 to understand player movement
- Basic understanding of Unity components (Rigidbody, Collider)
- A scene with a player object that can move around

## Instructions

### Step 1: Prepare Your Scene
1. **Open your scene from Activity 3** with the player object
2. **Ensure you have space to test** collision interactions
3. **Save your scene** (Ctrl+S) before adding new components

### Step 2: Add a Rigidbody to Your Player

You already have a Player from Activity 3 — the Capsule with the `PlayerMovement` script.
Keep using it. Do not create a second one.

1. **Select the Player** in the Hierarchy
2. **Add a Rigidbody component**:
   - In Inspector, click "Add Component"
   - Search for "Rigidbody" and add it
3. **Check the "Is Kinematic" checkbox** on the Rigidbody

> **Why kinematic?** Unity only reports trigger events when at least one of the two objects
> has a Rigidbody, so the Player needs one. But a normal Rigidbody means "physics moves
> this object" — and your `PlayerMovement` script moves it by setting the Transform
> directly. Leave it non-kinematic and the two fight each other: your Player falls through
> the floor, drifts, or refuses to move. **Is Kinematic** tells Unity "I will move this
> myself, just keep detecting collisions for me", which is exactly what you want.

### Step 3: Create a Collectible
1. **Create a Cube**: GameObject → 3D Object → Cube
2. **Rename it "Collectible"**
3. **Position it** somewhere the Player can reach, at the same height as the Player
4. **Set it up as a Trigger**:
   - In Inspector, find the Box Collider component
   - Check the "Is Trigger" checkbox
   - This lets the Player pass through it instead of bumping into it, while still
     reporting the overlap to your script

### Step 4: Create the CollisionHandler Script
1. **Create a new script**: Right-click in Project window → Create → C# Script
2. **Name it "CollisionHandler"**: Double-click to rename if needed
3. **Open the script** and replace all code with:

```csharp
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collected: " + other.name);
        Destroy(other.gameObject);
    }
}
```

### Step 5: Attach the Script
1. **Attach the script**: Select the Player object, drag the CollisionHandler script onto it
2. **Save the script** (Ctrl+S) and return to Unity

### Step 6: Test the Collision System
1. **Enter Play mode**: Click the Play button
2. **Move the Player** using WASD or arrow keys
3. **Collide with the Collectible**: Move the Player into the Cube
4. **Observe the results**:
   - Check the Console for debug messages
   - Watch the Collectible disappear
   - Verify collision detection is working

### Step 7: Expand Your Scene
1. **Add more collectibles**:
   - Duplicate the Collectible (Ctrl+D)
   - Position them around the scene
   - Each will disappear when touched
2. **Test with multiple objects**: Move around and collect all objects
3. **Observe Console output**: Each collision generates a debug message

## Understanding the Code

### **OnTriggerEnter()**
- Called automatically when this object enters a trigger collider
- You never call it yourself — Unity finds it by name and runs it for you
- `Collider other` is the thing you touched, which is how you know what was collected
- Requires a Rigidbody on one of the two objects, and `Is Trigger` on the other's collider

### **Trigger vs Collision**
- **Trigger** (`OnTriggerEnter`): objects pass through each other, and you get told about
  it — right for pickups, checkpoints, and detection zones
- **Collision** (`OnCollisionEnter`): objects physically block each other and bounce — right
  for walls, floors, and anything solid
- Same idea, different question: "did we overlap?" versus "did we hit?"

### **Debug.Log()**
- Outputs messages to Unity's Console window
- Useful for debugging and understanding what's happening
- Can display variable values and object names

### **Destroy()**
- Removes the GameObject from the scene entirely
- Compare with `gameObject.SetActive(false)`, which only hides it — the object is still
  there, and can be switched back on later. Use that one if you want it to come back


## Extension Activities

### **Add Visual Feedback**
Modify the script to change the Player's colour on collision:

```csharp
// Add to OnTriggerEnter method
GetComponent<Renderer>().material.color = Color.green;
```

### **Create Collectible Types**
Add different types of collectibles with different behaviors:

```csharp
// Check the object's tag or name
if (other.CompareTag("PowerUp"))
{
    // Special behavior for power-ups
    Debug.Log("Power-up collected!");
}
```

### **Add Score System**
Create a simple scoring system:

```csharp
// Add at class level
public int score = 0;

// Add to OnTriggerEnter method
score += 10;
Debug.Log("Score: " + score);
```

### **Create Respawn System**
Make collectibles come back after a delay. A **coroutine** is a method that can pause part
way through and carry on later — perfect for "do this, wait, then do that".

You cannot destroy the collectible this time, or there would be nothing to bring back, so
hide it instead:

```csharp
// This goes at the very top of the file, with the other using line
using System.Collections;

// Replace Destroy(other.gameObject) with:
StartCoroutine(RespawnObject(other.gameObject));

// Add this method to the class
IEnumerator RespawnObject(GameObject obj)
{
    obj.SetActive(false);
    yield return new WaitForSeconds(3f);
    obj.SetActive(true);
}
```

> Forgetting `using System.Collections;` gives you an error about `IEnumerator` not being
> found. It catches almost everyone the first time.

## Outcome
A functional collision detection system where the player can interact with objects in the scene. Objects disappear when touched, and debug messages provide feedback about collision events. This foundation can be expanded to create more complex interaction systems. 