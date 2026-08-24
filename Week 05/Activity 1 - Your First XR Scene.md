# Activity 1: Your First XR Scene

> **Headset badge: simulator-friendly.** Everything in this activity happens in the Editor.
> You will not need a headset until Activity 2.

## Objective

Build a small XR scene — an **`XR Origin (XR Rig)`** standing on a real floor, objects sized in
metres rather than by eye, and one object you can pick up — and understand why you move the rig
and never the camera.

## Prerequisites

- The **Week 3 checkpoint** finished: [Getting Set Up for XR](../Week%2003/Activity%205%20-%20Getting%20Set%20Up%20for%20XR.md).
  Your project builds to a headset and the known-good scene runs on it. **This week does not
  revisit setup.** If something in your configuration is wrong, fix it against
  [the OpenXR setup guide](../Guides/OpenXR_Unity_Setup_Guide.md), not against this activity
- **Packages you'll add this week:** none, but you will **upgrade one and import one sample**.
  Step 2 covers both
- Skim [XR Interaction Toolkit — Core Concepts](../Guides/XRInteractionToolkit.md), at
  least *Interactors, Interactables, Transformers* and *The rig*. This activity uses that
  vocabulary and does not re-explain it
- **Hardware: none.** A keyboard and mouse are enough

> **Keyboard shortcuts.** Where this activity says `Ctrl`, macOS users press `Cmd` —
> `Ctrl+S` becomes `Cmd+S`.

## Instructions

### Step 1: Start from the VR template

Create this week's project in **Unity Hub → New project**, choosing the **VR** template on
**Unity 6.3 LTS (`6000.3.x`)**.

The template is the end state of the setup you did in Week 3, prepared for you. It arrives with
XR Plugin Management, the **XR Interaction Toolkit** (**XRI**), the **Starter Assets** sample
already imported, and a sample scene with a working rig.

> **Template versions vary.** Depending on which VR template your Unity Hub offers, you will get
> XRI 3.4.1 or 3.5.1. Step 2 moves you to 3.6.0 from either, so it does not matter which you
> started with.

> **Check the settings anyway.** The template targets OpenXR generally, and it does not know you
> are building for Android. Section 7 of
> [the OpenXR setup guide](../Guides/OpenXR_Unity_Setup_Guide.md) lists exactly what to confirm,
> and **Project Validation** finds the rest. Two minutes here saves a black screen in Activity 2.

### Step 2: Upgrade XRI and import the simulator

The VR template installs **XRI 3.4.1**, and Package Manager marks **3.3.2** as *Recommended*
because that is the version bundled with this Editor. This course uses **3.6.0**, which is the
current release. What 3.6.0 buys you is a run of **XR Interaction Simulator** fixes, and you are about
to spend the rest of the course in that simulator.

1. **Window → Package Manager → Unity Registry → XR Interaction Toolkit → Update to 3.6.0**.
2. With the toolkit still selected, open the **Samples** tab and import both **Starter Assets**
   and **XR Interaction Simulator**. They land under
   `Assets/Samples/XR Interaction Toolkit/3.6.0/`.
3. **Delete the old sample folder** — `Assets/Samples/XR Interaction Toolkit/3.4.1/` — in the
   Project window.

> **Step 3 is not tidying up.** Samples are copied into `Assets/` and do not move when the
> package updates, so after step 2 you have two of them. Both declare an assembly called
> `Unity.XR.Interaction.Toolkit.Samples.StarterAssets`, and Unity cannot compile a project with
> two assemblies of the same name. If the Console fills with duplicate-assembly errors, this is
> why, and deleting the old folder is the whole fix.

> **`Packages/manifest.json` and `Packages/packages-lock.json` both changed.** Those two files
> are how another machine reconstructs this exact package set, so they belong in the repository.
> Step 3 commits them.

### Step 3: Create the scene

Create a new scene and save it as `FirstXRScene.unity` under `Assets/Scenes/`.

Delete the **Main Camera** the new scene came with. The rig brings its own, and two cameras in
an XR scene is a confusing hour.

Commit now — the upgraded packages and the empty scene together. This is your fall-back point
if anything below goes wrong, and it is the only commit you need until the end of the activity.

### Step 4: Put a rig in the scene

Drag **`XR Origin (XR Rig)`** into the scene from:

```
Assets/Samples/XR Interaction Toolkit/3.6.0/Starter Assets/Prefabs/
```

That prefab is the player. Expand it in the Hierarchy and read it, because the shape of it is
the whole mental model:

```
XR Origin (XR Rig)              ← the player's position in the world. Move THIS.
└── Camera Offset               ← height compensation, managed for you
    ├── Main Camera             ← the headset. Driven by tracking. Never write to it.
    ├── Left Controller         ← driven by the left controller's tracked pose
    │   ├── Near-Far Interactor ← grabs up close, casts a ray at distance
    │   ├── Poke Interactor     ← presses things
    │   └── Teleport Interactor
    └── Right Controller        ← the same again
```

The prefab also carries an **Input Action Manager** holding `XRI Default Input Actions`. That
component enables the input actions at runtime, and every Interactor on the rig reads a named
action such as *Select* from that asset. Without it everything tracks correctly and no button
does anything.

> **The two XR Origins.** `XR Origin (XR Rig)` is the Starter Assets *prefab* you just used.
> **XR Origin (VR)** is a *menu item* under **GameObject → XR**, and it builds only the origin,
> the camera offset and the camera. No controllers, no Interactors, no hands. It is the right
> tool when you want to assemble a rig yourself; it is not what this course uses. Dragging in
> the prefab *and* using the menu item leaves you with two rigs and two audio listeners.

### Step 5: Stand the player on the floor

Select **XR Origin (XR Rig)** and, on the **XR Origin** component, set **Tracking Origin Mode**
to **Floor**.

This is the setting that makes metres mean something. On **Floor**, `y = 0` is the real floor
the player is standing on, so a 2-metre doorway is 2 metres of real height in front of a real
person. The other two values are **Device**, where `y = 0` is wherever the headset happened to
be when tracking started, and **Not Specified**, which leaves the choice to the runtime. On
either of those your scene sits at an arbitrary height, differently for every player.

### Step 6: Give it a floor

**GameObject → 3D Object → Plane**, at position `(0, 0, 0)`. Rename it `Floor`.

A default Unity plane is **10 metres by 10 metres**. That is a generously sized room and a
useful ruler — if your scene needs more than a quarter of it, ask whether the player is
expected to walk further than a real room allows.

Give it a material with some colour. Flat grey is harder to judge depth against than you
expect, and depth judgement is most of what you are about to test.

### Step 7: Place objects at deliberate real-world sizes

A default Unity cube is **1 metre on every side**. Every scale value below is therefore a
measurement, not a guess. Build these three:

| Object | Scale | Position | Why this size |
|---|---|---|---|
| `Table` — Cube | `(1.2, 0.75, 0.6)` | `(0, 0.375, 0.8)` | A desk is 0.75 m high. Its top lands at `y = 0.75`, and its near edge sits 0.5 m in front of you |
| `DoorFrameLeft` — Cube | `(0.1, 2.0, 0.1)` | `(-0.5, 1.0, 3.0)` | A door is 2 m tall |
| `DoorFrameRight` — Cube | `(0.1, 2.0, 0.1)` | `(0.5, 1.0, 3.0)` | The posts are 0.1 m thick and their centres are 1.0 m apart, so the opening between them is 0.9 m — a doorway you can walk through |

The table tells you whether your hands are where your hands should be. The doorway tells you
whether the world is the size you think it is, and a doorway that is subtly wrong is obvious to
a body and invisible on a monitor.

Note the position values you chose. You will compare them against how the scene actually feels
in Activity 2, and *"the table was too low"* is a more useful observation when you can say what
you set it to.

> **Do the arithmetic, don't drag.** Type these numbers into the Inspector. Positioning by eye
> in the Scene view is how scenes end up at 1.3× real scale, which looks normal on a screen and
> feels like being a child in the headset.

### Step 8: Add one thing you can pick up

One object, understood completely.

1. **GameObject → 3D Object → Cube**, rename it `Mug`, scale `(0.08, 0.08, 0.08)`, position
   `(0, 0.79, 0.7)`. That is an 8 cm block resting on the table top, roughly the size of a mug.
2. It already has a **Box Collider**, which is what lets an Interactor find it at all.
3. **Add Component → XR Grab Interactable**. Unity adds a **Rigidbody** with it, because
   `XRGrabInteractable` declares `[RequireComponent(typeof(Rigidbody))]`.

That is the entire Interactable side. Week 6 is where you take that
apart and write your own.

Three fields on the component are worth finding now, even though you will leave them at their
defaults:

- **Movement Type** — whether the held object is moved by velocity tracking, kinematically, or
  by direct transform writes. The biggest single lever on how physical the object feels
- **Attach Transform** — the point on the object that lines up with the hand. Empty means "grab
  about my own origin", which is why a picked-up sword so often ends up held by the blade
- **Throw On Detach** — carries your hand's motion into the Rigidbody's `linearVelocity` when
  you let go. Without it, released objects drop straight down as though switched off

Save the scene (`Ctrl+S`).

### Step 9: Turn on the XR Interaction Simulator

You have built a scene. You have not tested it, and an untested scene is not finished work. The
**XR Interaction Simulator** gives you a virtual headset and two virtual controllers driven by
keyboard and mouse, so you can test right now without booking anything.

Go to **Edit → Project Settings → XR Plug-in Management → XR Interaction Toolkit** and tick
**Use XR Interaction Simulator in scenes**.

That is the whole setup. Nothing goes into your scene, there is no prefab to drag, and no
reference to assign. XRI creates the simulator for you when you press Play.

Two settings sit beneath it:

- **Instantiate In Editor Only** is on by default. Leave it on. It is what keeps the simulator
  out of a device build, so you never have to remember to take it out
- **Use Classic XR Device Simulator** switches to the older implementation that XRI 3.1
  replaced. You do not want it. Any tutorial whose controls do not match your on-screen panel
  is written against the classic simulator or against XRI 2.x

> **Prefer to see it in the Hierarchy?** You can drag
> `Assets/Samples/XR Interaction Toolkit/3.6.0/XR Interaction Simulator/XR Interaction Simulator.prefab`
> into the scene instead. It behaves identically, but then it is yours to disable before a
> device build. The project setting is the one less thing to forget.

### Step 10: Test the scene you just built

Press **Play**. An on-screen panel appears showing the current controls and which device you
are currently driving — your head, the left controller, or the right.

**Read the panel.** It is the authoritative reference, it updates as you change modes, and it
is faster than any list written down here could be. The one idea to get straight is *which
device am I driving right now* — mouse and keyboard are handed between the head and each
controller by modifier keys, and once that clicks the rest is muscle memory.

Then check your own work, in this order:

1. Look around. Is the doorway where you put it, and does the floor start under your feet
   rather than at your knees?
2. Move the rig up to the table.
3. Take control of a controller, reach for the `Mug`, and grab it.
4. Carry it, drop it, throw it. Watch what the Rigidbody does when you let go.

While you are in there, stop Play mode and look at the Hierarchy during a run: an **XR
Interaction Manager** appears under **DontDestroyOnLoad**. XRI creates one the first time an
Interactor or Interactable goes looking for it, which is why you never had to add one. See
[XRInteractionToolkit.md](../Guides/XRInteractionToolkit.md#the-xr-interaction-manager).

> **Nothing responds?** Click once into the Game view — the **XR Interaction Simulator** only
> receives input while the Game view has focus, and this catches nearly everybody once. If it
> is still dead, the causes are listed under *Simulator* in the *Troubleshooting* section of
> [the OpenXR setup guide](../Guides/OpenXR_Unity_Setup_Guide.md).
>
> **Grab does nothing, no errors?** Work down the six pieces of a working grab in
> [XRInteractionToolkit.md](../Guides/XRInteractionToolkit.md#example-what-a-working-grab-is-actually-made-of).
> It is a checklist and it is the debugging procedure.

Fix anything that is broken, save (`Ctrl+S`) and commit.

> **Checkpoint.** Your Hierarchy has exactly one **`XR Origin (XR Rig)`**, a floor, a table, a
> doorway, and one `Mug` carrying a Collider, a Rigidbody and an `XRGrabInteractable` — and you
> have picked that `Mug` up and thrown it with your own hands. The scene works.

## Understanding real-world scale

In a flat game, scale is a style choice. In XR it is a fact you are asserting about the
player's own body, and they will catch you being wrong in about two seconds.

The reason is that the headset reports **real** distances. When the player leans forward 20 cm,
the camera moves 20 cm in your scene, whatever you intended a "unit" to mean. Your scene has a
true scale whether you chose one or not, and the only question is whether you chose it
deliberately.

Two habits follow, and they are worth adopting permanently:

- **Model in metres, from real measurements.** A door is 2.0 m. A desk is 0.75 m. A handle is
  at 1.0 m. Look things up rather than eyeballing them
- **Put one known object in every scene.** A doorway, a chair, a coffee mug — something whose
  real size everybody already knows. It turns "does this feel right?" into a question with an
  answer

## Understanding the camera as an output, not an input

### The rule

**You move the XR Origin. You never move the camera.**

### Why

The **Main Camera** inside the rig is not a camera you place. It is a *report*. Every frame,
the OpenXR runtime reads where the player's head physically is and writes that pose onto the
camera's transform. The camera's position is an **output** of the tracking system, in the way
that a thermometer's reading is an output of the temperature.

So when your script sets the camera's position, one of two things happens, and both are bad.
Either tracking overwrites you on the next frame and your code appears to do nothing, or you
win the fight and the virtual head stops corresponding to the real one.

That second outcome is the one that makes people ill. A person's sense of balance comes from
the inner ear, which knows the head did not move. Their eyes are being told the world just slid
two metres sideways. The brain receives two confident, contradictory accounts of the same event,
and the response it has evolved for that situation is nausea. You cannot tune it out or ask the
player to get used to it. You can only avoid causing it.

The **XR Origin** is the fix. It is the frame of reference the head is tracked *inside*. Move
the origin and the player's whole world moves with them, head offset intact, real head motion
still producing exactly the visual change the inner ear expects.

```
World
└── XR Origin (XR Rig)  ← YOUR code writes here. This is the input.
    └── Camera Offset
        └── Main Camera ← TRACKING writes here. This is the output. Read it; never write it.
```

### In code

```csharp
using UnityEngine;

// Attach this to the XR Origin (XR Rig) GameObject itself.
public class OriginNudge : MonoBehaviour
{
    public Vector3 nudge = new Vector3(0f, 0f, 0.5f);

    // Call this from a UI button or an input action.
    public void StepForward()
    {
        // Correct. The rig moves; the head keeps its tracked offset inside it.
        transform.position += nudge;
    }
}
```

And the version that costs someone an afternoon and a headache:

```csharp
// Do NOT do this. Ever. In any script, for any reason.
Camera.main.transform.position += nudge;
```

Reading the camera is fine, and often exactly what you want — where the player is looking, how
far they are from something, whether an object is behind them:

```csharp
// Fine. Reading an output is what outputs are for.
Vector3 headPosition = Camera.main.transform.position;
Vector3 gazeDirection = Camera.main.transform.forward;
```

Every locomotion component XRI ships — teleport, continuous move, turn — does what
`OriginNudge` above does. They move the origin. There is no hidden mechanism you are missing.

> **The symptom, so you recognise it later.** *"The scene moves when I move my head and it makes
> me feel ill."* That is always something writing to the camera's transform. Search your scripts
> for `Camera.main` before you look anywhere else. It is listed under *Troubleshooting* in
> [the OpenXR setup guide](../Guides/OpenXR_Unity_Setup_Guide.md).

## Extension Activities

### **The measured room**

Replace the three test objects with a real room you can measure — the lab you are sitting in,
or your kitchen. Take a tape measure to it: door height, bench height, ceiling, the width of a
walkway. Build it at those numbers.

You will find out in Activity 2 how good your eye actually is. This is the cheapest calibration
exercise in the course.

### **Head-height readout**

Show the player's real standing height on screen, to prove to yourself that the camera is
reporting rather than obeying.

Logic: read the camera's world-space `y` each frame and display it. With **Tracking Origin
Mode** set to **Floor** and the rig at `y = 0`, that number *is* the height of the person's
eyes above their real floor.

```csharp
// In Update(), on a script with a TMP_Text reference, attached to the XR Origin (XR Rig):
float eyeHeight = Camera.main.transform.position.y - transform.position.y;
```

Subtracting the rig's own `y` keeps it honest if you later move the origin. Ask two people of
different heights to try it.

### **Reach-envelope markers**

Place three small markers on the table at 0.3 m, 0.5 m and 0.7 m in front of the origin, and
label each one. Move the `Mug` onto the middle marker.

Carry a prediction into Activity 2 about which markers a real arm can reach, then find out.

### **A second Interactable, deliberately wrong**

Duplicate the `Mug` and scale the copy to `(0.4, 0.4, 0.4)`. Leave both in the scene.

Note down which one you expect to feel right. Do not correct it now. An object that is
obviously too big on a monitor shows you how *unobvious* scale error is when it is only slightly
wrong, and Activity 2 is where you see both.

## Outcome

A saved, committed XR scene containing one **`XR Origin (XR Rig)`** with **Tracking Origin
Mode** set to **Floor**, a floor, three objects placed at measured real-world sizes, and one
object carrying a Collider, a Rigidbody and an `XRGrabInteractable` — tested in the **XR
Interaction Simulator**, with the `Mug` picked up and thrown.

You also have the rule that prevents the most common and most unpleasant failure in student XR
work: **the camera's position is an output. Move the XR Origin.**

## Headset checkpoint

The simulator has told you the scene *works*. It cannot tell you whether it is any good. It has
no opinion on whether your table is desk height, whether a real arm reaches the `Mug`, whether
moving through that doorway is comfortable, or whether the whole thing holds framerate on a
mobile chip — and those are not small questions. They are the difference between a scene with a
bug in it and a scene that does not work.

That is what **[Activity 2](Activity%202%20-%20The%20Two%20Workflows.md)** is for. Take this
scene to a Meta Quest 2 / 3 / 3S, and find out what you were wrong about.

## References

- `XRGrabInteractable`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable.html>
- Starter Assets sample: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/samples-starter-assets.html>
- `XROrigin` and Tracking Origin Mode: <https://docs.unity3d.com/Packages/com.unity.xr.core-utils@2.6/manual/index.html>
