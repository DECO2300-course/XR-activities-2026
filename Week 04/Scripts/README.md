# Week 04 Scripts

Reference copies of the C# scripts written across the two Week 4 activities. Write your own
first — these are for checking against, not for pasting.

## Activity 1 — Proximity Reactions

### **SimpleWASD.cs** — Player Movement
Drives the player capsule around the scene. Both activities assume it is on the Player.

- Reads the project-wide `Move` action through the **Input System**
- Forward and back on the action's Y, turn on the action's X
- Draws a forward gizmo ray at the height a grab ray fires from, so you can see where you are aiming

### **ProximityHighlighter.cs** — Basic Proximity Highlighting
The core proximity concept, and the script Activity 1 builds step by step.

- Changes the object's colour when the player comes within `proximityDistance`
- Acts on the frame the player arrives or leaves, not on every frame in between
- Finds the player once in `Start()` rather than every frame

### **ProximityMover.cs** — Movement Extension
Objects retreat when you approach and return when you leave.

- Picks its retreat position once, on the frame the player arrives
- Returns to its starting position with `Vector3.MoveTowards` once you go

### **ProximitySound.cs** — Sound Effects Extension
Plays a clip when the player enters the proximity zone.

- Needs an AudioSource on the object and a clip assigned in the Inspector
- Does nothing, quietly, if either is missing

### **ProximityEffects.cs** — Visual Effects Extension
Rotation and scaling while the player is nearby.

- `rotateOnProximity` and `scaleOnProximity` are both on by default — untick either in the Inspector
- Scales relative to the object's starting scale, so a resized object keeps its proportions

## Activity 2 — Object Selection and Throwing

### **Grabbable.cs** — Marks an Object as Holdable
Put this on anything you want the player to pick up.

- Tracks held state with a single `isGrabbed` flag
- Adds a Rigidbody at startup if the object does not already have one

### **PlayerGrabber.cs** — Distance-Based Selection
The main grab and throw system, built step by step in Activity 2.

- Reads the project-wide `Interact` action through the **Input System**
- Picks up the first eligible object it finds within `grabDistance` — the results come back unsorted, so this is not necessarily the closest one
- Parents the object to the player and makes its Rigidbody kinematic while it is held
- Hands the object back to physics before throwing it with `AddForce`

### **PlayerGrabberRaycast.cs** — Raycast-Based Selection
Grabs only what the player is actually facing. The optional Step 9 variant.

- Same input, holding and throw behaviour as `PlayerGrabber`
- Selects with `Physics.Raycast` along the player's forward direction
- Fires from half a metre below the capsule's centre, level with objects resting on the ground

### **PlayerGrabberVelocity.cs** — Velocity-Based Throwing
A throw with a more natural arc. The velocity-throwing extension activity.

- Same input and selection behaviour as `PlayerGrabber`
- Sets `linearVelocity` directly instead of applying an impulse, which makes the throw speed independent of the object's mass

## How to Use

1. **Pick the script that matches what you are adding**:
   - Start with **ProximityHighlighter** for basic highlighting
   - Add **ProximityMover**, **ProximitySound** or **ProximityEffects** for the extensions
   - Use one — and only one — of the **PlayerGrabber** variants on the Player
2. **Attach it** to a GameObject in your scene
3. **Configure it** in the Inspector:
   - Proximity Distance: how close the player needs to be
   - Normal Color / Highlight Color: the two colours the highlighter swaps between
   - Extension-specific settings (movement speed, audio clips, scale multiplier)
4. **Test in Play mode**

## Requirements

- Unity 6.3 LTS (`6000.3.x`)
- The Player object tagged "Player" — every proximity script looks it up by tag
- A Renderer on the object, which every 3D primitive already has
- The project-wide input actions asset, with its `Move` and `Interact` actions — every new
  Unity project ships with one

Extension-specific: **ProximitySound** needs an AudioSource on the object and a clip
assigned. **ProximityMover** and **ProximityEffects** need nothing extra.

## Learning Notes

- Proximity detection here is `Vector3.Distance()` and a threshold, nothing more
- A pair of boolean flags turns "the player is near" into "the player just arrived"
- Between them the scripts cover component access, material changes, parenting, and physics forces
- They read named **Input System** actions rather than individual keys, which is the same
  approach that later drives VR controllers
- Extension activities in the main documents provide conceptual challenges for further learning
