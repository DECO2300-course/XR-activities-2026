# Activity 1: Grab Interactables and Interactors

> **Headset badge: simulator-friendly.** Everything here can be built and tested in the
> **XR Interaction Simulator**. There is a headset checkpoint at the end, and you should
> take it — throw feel and reach are two of the things the XR Interaction Simulator
> cannot tell you.

## Objective
Build a grabbable object out of its parts rather than dropping in a prefab, then tune the
three fields that decide how it feels in the hand — attach transform, movement type, and
throw on detach. Finish by comparing the two ways a hand can reach an object: the
near-far Interactor and the poke Interactor.

## Prerequisites
- **Week 5** complete: a project made from the **VR template**, upgraded to **XRI 3.6.0**,
  switched to Android, with the **Meta Quest Support** feature group ticked and
  **Project Validation** clean
- **Read [XR Interaction Toolkit — Core Concepts](../Guides/XRInteractionToolkit.md)** first,
  especially *Interactors, Interactables, Transformers*. This activity uses that
  vocabulary and does not re-explain it
- **Packages you'll add this week:** none. **XR Interaction Toolkit** 3.6.x and its
  **Starter Assets** and **XR Interaction Simulator** samples came in with Week 5. If the
  samples are missing, import them from **Window → Package Manager → XR Interaction
  Toolkit → Samples**
- Hardware: none required to build this. A **Meta Quest 2 / 3 / 3S** for the closing
  checkpoint

> **Keyboard shortcuts.** Where this activity says `Ctrl`, macOS users press `Cmd` —
> `Ctrl+S` becomes `Cmd+S`, `Ctrl+D` becomes `Cmd+D`.

## Instructions

### Step 1: A scene worth grabbing in

1. Create a new scene and save it as `Week06_Interactions` in `Assets/Scenes/`
   (`Ctrl+S`).
2. Delete the **Main Camera** the new scene came with. The rig brings its own.
3. Drag the **`XR Origin (XR Rig)`** prefab from
   `Assets/Samples/XR Interaction Toolkit/3.6.0/Starter Assets/Prefabs/` into the scene.
   Same rig as Week 5: Interactors on both hands, and an **Input Action Manager** holding
   `XRI Default Input Actions`.
4. Add a floor: **GameObject → 3D Object → Plane** at `(0, 0, 0)`.
5. Add a table: **GameObject → 3D Object → Cube**, name it `Table`, position
   `(0, 0.725, 0.75)`, scale `(1.2, 0.05, 0.6)`. Its top surface lands at `y = 0.75`, the
   same desk height you measured in Week 5. Every object below is placed to rest on it.
6. **File → Build Profiles → Scene List → Add Open Scenes**, and make sure
   `Week06_Interactions` is ticked. Do it now rather than at the first headset checkpoint —
   a scene that is not in the list is not in the build.
7. Press **Play**. You should be standing on the plane, looking at a table, with a control
   panel on screen telling you which device your mouse is currently driving. Spend a minute
   learning to take control of the left and right hand — you will need both in Activity 2.

> **No control panel?** The **XR Interaction Simulator** is switched on in
> **Edit → Project Settings → XR Plug-in Management → XR Interaction Toolkit**, under
> **Use XR Interaction Simulator in scenes**. Week 5 Activity 1 set it for the Week 5
> project; this is a new project setting in a new project.

> **Nothing responds at all?** Click once into the Game view — the simulator only receives
> input while the Game view has focus. The full six-item checklist is at the end of
> [XRInteractionToolkit.md](../Guides/XRInteractionToolkit.md#example-what-a-working-grab-is-actually-made-of).

### Step 2: Build an Interactable by hand

You are going to assemble the grabbable object from its three parts, because knowing the
parts is what lets you debug it later.

1. **GameObject → 3D Object → Cube**, name it `Prop`, position `(0.3, 0.825, 0.75)`,
   scale `(0.15, 0.15, 0.15)`. It rests on the table.
2. It already has a **Box Collider**. That is part one — without a Collider nothing can
   reach it.
3. **Add Component → Rigidbody**. Part two — this is what lets it be moved and thrown by
   physics.
4. **Add Component → XR Grab Interactable**. Part three.

Notice what Unity did for you: adding the `XRGrabInteractable` was enough. You did not
add a Grab Transformer, and yet the object will move correctly when held, because
**Unity adds a default transformer set automatically**. Activity 3 is where you switch
that off and supply your own.

5. Press **Play** and grab the cube. In the XR Interaction Simulator, take control of a
   hand, move it to the cube, and press the grip control shown on the on-screen panel.

> **You built this by hand in Week 4.** Your `Grabbable` script parented the object to the
> player and toggled `isKinematic`. Three components have just replaced it. The value of
> having written it is that you know what each of these three is standing in for.

> **Checkpoint.** The cube follows your hand and drops when you let go. If it does not
> move at all, you are almost certainly missing the Rigidbody or the Collider.

### Step 3: Attach Transform — where the hand holds it

Right now the cube is held about its own origin, which for a cube is its centre. That is
fine for a cube and wrong for almost everything else. A sword held about its centre floats
awkwardly mid-blade; you want it held by the grip.

1. Make a long prop: duplicate `Prop` (`Ctrl+D`), name it `Mallet`, position it at
   `(-0.3, 0.925, 0.75)`, scale `(0.06, 0.35, 0.06)`.
2. Right-click `Mallet` → **Create Empty**, name the child `Attach Point`.
3. Set `Attach Point` to local position `(0, -0.5, 0)`.

   A child's local position is measured in the parent's **unscaled** local units, and a
   Unity cube runs from `-0.5` to `+0.5` on each axis. So `-0.5` is the bottom face of the
   mallet whatever you scale the mallet to. This catches people, because `-0.5` looks like
   it ought to depend on that `0.35`.
4. Select `Mallet`, find the **XR Grab Interactable**, and drag `Attach Point` into the
   **Attach Transform** field.
5. Play and grab the mallet. It now hangs from your hand by its handle rather than
   balancing on its middle.

The attach transform also controls **rotation**: the object rotates so that the attach
point's axes line up with the hand's. Rotate `Attach Point` 90° about X and grab again to
see the mallet come up horizontal.

> **The mallet falls over when you press Play.** It is a tall, thin, loose object standing
> on a table. Pick it up from wherever it lands.

### Step 4: Movement Type — how physical the held object is

**Movement Type** decides how a held object feels, and how much damage it does to
everything else in the scene. Set up a test that shows the difference:

1. Build a small stack at the right-hand end of the table: three cubes at
   `(0.55, 0.77, 0.75)`, `(0.55, 0.82, 0.75)` and `(0.55, 0.87, 0.75)`, scale
   `(0.08, 0.04, 0.08)`, each with a **Rigidbody**. These are scenery, not Interactables —
   no `XRGrabInteractable`. They will settle by a centimetre when you press Play.
2. Select `Prop`, and work through the **Movement Type** values one at a time, playing
   after each and dragging the prop straight through the stack.

| Movement type | What it does | What it feels like |
|---|---|---|
| **Velocity Tracking** | Sets the Rigidbody's linear and angular velocity each `FixedUpdate` to carry it towards the target pose | The most physical. The prop is stopped by walls, jostles when it collides, and lags slightly behind your hand |
| **Kinematic** | Switches the Rigidbody to kinematic and moves it to the target pose each physics step | Rock solid in the hand. Pushes other objects out of the way but is never pushed back. Will shove a stack across the room |
| **Instantaneous** | Writes the transform directly each frame | Perfectly precise and completely unphysical. It passes through the stack as if it were not there |

3. Decide which you want and write down *why*. "It felt best" is not the answer —
   the answer is about whether the object should be able to push, be pushed, or neither.

> **This is the field people blame on bugs.** A held object that violently launches every
> prop it touches is not broken physics; it is a kinematic grab doing exactly what it was
> asked to do.

### Step 5: Throw On Detach

Press Play, grab `Prop`, swing your hand and let go. Depending on your settings it either
sails away or falls straight down.

1. Select `Prop` and find **Throw On Detach** on the **XR Grab Interactable**.
2. With it **off**, a released object keeps no memory of your hand's motion. It drops
   from wherever it was.
3. With it **on**, XRI averages the hand's recent motion and writes it into the
   Rigidbody's **`linearVelocity`** at the moment of release. That is all a throw is.
4. Two fields beneath it control the feel. **Throw Velocity Scale** multiplies the
   inherited velocity; **Throw Smoothing Duration** sets how far back the averaging
   reaches, up to twenty previous frames. A scale above `1` makes the object leave the
   hand faster than the hand was actually moving, which is a common cheat — real throwing
   in VR under-delivers, because people let go late.

> **Throw On Detach does nothing on a kinematic Rigidbody.** If you left **Movement Type**
> on **Kinematic** in Step 4, switch back to **Velocity Tracking** before you test this.

> **The property is `linearVelocity`.** If you find a script that reads the Rigidbody's
> old `velocity` property instead, it was written for an older Unity and will not compile
> in Unity 6.3 LTS (`6000.3.x`).

### Step 6: Compare the Interactors — near-far versus poke

Everything so far was the Interactable half. Now look at the reaching half.

1. In the Hierarchy, expand `XR Origin (XR Rig)` → `Camera Offset` → `Left Controller`.
   Three Interactors hang off each hand:

   ```
   Left Controller
   ├── Near-Far Interactor   ← grabs up close, casts a ray at distance
   ├── Poke Interactor       ← presses things it touches
   └── Teleport Interactor   ← Week 7
   ```

2. Play and notice what the **Near-Far Interactor** gives you *from one component*: close
   to the cube you grab it directly; further away a ray appears and you can pull it in.
   That handover is what `NearFarInteractor` exists to do.
3. Now see what it is doing for you. Disable the `Near-Far Interactor` on one hand, and on
   that same hand **Add Component → XR Ray Interactor**. Play. Nothing happens at all, not
   even a ray you can select with.

   A freshly added Interactor has no input. Select it, find **Select Input**, set
   **Input Source Mode** to **Input Action Reference**, and assign
   `XRI Left Interaction/Select` from `XRI Default Input Actions`. Play again: now you
   have a laser pointer and nothing else, with no close-range grab at all.

   That is two lessons in one step. Interactors read named actions rather than buttons,
   and the Starter Assets prefabs arrive with those references already assigned — which is
   why the rig worked the moment you dragged it in.
4. Undo both changes and re-enable the `Near-Far Interactor`.

Now the other kind of reach. Each hand already carries a **Poke Interactor**, so this half
is about giving it something to press.

5. **GameObject → 3D Object → Cube**, name it `Button`, position `(0, 0.76, 0.55)`,
   scale `(0.1, 0.02, 0.1)`. It sits flush on the table, near the front edge.
6. Give it an **XR Simple Interactable**. A simple Interactable can be hovered and
   selected but is never moved by the interaction — which is exactly right for a button.
7. Create two materials, `ButtonIdle` and `ButtonPressed`, in colours you can tell apart.
8. Add the **[MaterialSwapper](Scripts/MaterialSwapper.cs)** script from the `Scripts/`
   folder to `Button`. Set **Target Object** to `Button` itself, **Material A** to
   `ButtonIdle` and **Material B** to `ButtonPressed`.
9. On the **XR Simple Interactable**, expand **Interactable Events → Select**, and wire
   **Select Entered** to `MaterialSwapper.SwapToMaterialB` and **Select Exited** to
   `MaterialSwapper.SwapToMaterialA`.
10. Play, drive a hand into the button, and watch it change colour. Then try to press it
    with the ray from across the room — you cannot. A poke is a physical prod at a point,
    not a pointer.

> **Checkpoint.** You can grab and throw the prop with the near-far Interactor, and press
> the button only by touching it with the poke Interactor. Two Interactors on one hand,
> two different Interactables, no conflict.

## Understanding the two halves

Everything you built above is one sentence from
[XRInteractionToolkit.md](../Guides/XRInteractionToolkit.md#key-concept-interactors-interactables-transformers)
made concrete: **an interaction is always a pair.**

The Interactable side owns *what can be done to the object*: a Collider so it can be
found, a Rigidbody so it can be moved, an `XRGrabInteractable` so it can be selected, and
an attach transform so it is held in the right place.

The Interactor side owns *how the hand reaches*: `NearFarInteractor` for a general hand,
`XRRayInteractor` when you want a pointer and nothing else, `XRPokeInteractor` for things
that should be pressed. None of these know about each other — they are matched up by the
**`XRInteractionManager`**, each frame, and filtered by interaction layers.

Movement type is worth one more sentence, because it is the field students most often set
by accident. The held object has a *target pose* every frame, and movement type decides
how the Rigidbody is persuaded to reach it: driven by velocity (velocity tracking),
teleported (kinematic), or bypassed entirely (instantaneous). Whether your object can
shove the scenery around follows directly from that choice.

## Extension Activities

### **A socket to put the prop back in**
Add an **`XRSocketInteractor`** on a small empty object above the table, with a trigger
Collider. A socket is an Interactor that does not move and accepts Interactables — drop
`Prop` near it and it snaps home.

Key detail: **Attach Ease In Time** lives on the `XRGrabInteractable`, not on the socket.
Set it to a small non-zero value rather than `0`. At exactly zero, an object transferring
between the socket and a hand can show a one-frame visual skip as it teleports to the new
attach point.

### **A pulse when you touch something**
Each `Left Controller` and `Right Controller` already has a **`HapticImpulsePlayer`**. Add
a **`SimpleHapticFeedback`** component beside your Interactor, assign the controller's
`HapticImpulsePlayer` to its **Haptic Impulse Player** field, and tick **Play Hover
Entered** and **Play Select Entered**. Nothing to write — this is a wiring exercise, and
it is the cheapest improvement to how an XR scene feels. You will only feel it on the
headset.

### **Two attach points, one object**
Give the mallet a second attach point at the head, and swap which one is assigned at
runtime from a script based on which hand grabbed it. Logic: subscribe to the
Interactable's select-entered event, look at the interactor that selected, and assign the
`XRGrabInteractable`'s attach transform before the first frame of the grab.

### **Interaction layers**
Define a `Tools` interaction layer in **Edit → Project Settings → XR Plug-in Management →
XR Interaction Toolkit → Interaction Layer Settings**, then set the mallet's and one
hand's **Interaction Layer Mask** to it. Confirm the other hand can no longer pick the
mallet up — and note that you get no error message when it fails, which is exactly why
this is worth having seen once.

## Headset checkpoint

Build to the headset (**File → Build Profiles → Build and Run**) and check the three
things the XR Interaction Simulator was silent about:

- **Reach.** Can you actually get to the prop and the button from a standing position, or
  did you place them where only a mouse can go?
- **Throw.** Does the prop go where you meant it to? Almost certainly not on the first
  try — tune **Throw Velocity Scale** on device, not at your desk.
- **Weight.** Does the mallet feel like it has a handle? An attach transform that looked
  right in the Scene view can feel wrong in the hand.

## Outcome
A scene with a hand-assembled grab Interactable whose hold point, physicality and throw
you chose deliberately, plus a poke-only button — and a clear picture of which component
owns which half of an interaction.

## References
- `XRGrabInteractable`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable.html>
- `NearFarInteractor`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactors.NearFarInteractor.html>
- Starter Assets sample: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/samples-starter-assets.html>
