# Activity 2a: Hand Gestures, Configured

> **Headset required.** Hand tracking is not available in the **XR Interaction Simulator**, so
> none of this can be finished at your desk. You need a **Meta Quest 2 / 3 / 3S** and a USB-C
> cable that carries data. Any headset on the course works; nothing here needs the newer
> hardware.
>
> **The package:** XR Hands (`com.unity.xr.hands`). You installed this in **Week 7 Activity 3**,
> so you may already have it. This activity was written against **1.7.x**; if you are on an
> earlier version, carry on, and only update if something here misbehaves.
>
> **Expect a slow loop.** You cannot enter Play mode to test hands, so **every change needs a
> build and a deploy**. That is minutes, not seconds. Batch your changes and test several
> variations per build. Step 7 shows you how.
>
> `Ctrl` is `Cmd` on macOS. Everything here works on macOS: the workflow is
> **build an `.apk` through File → Build Profiles and deploy it with Meta Quest Developer Hub**,
> for everyone.

---

## Objective

Show your right hand to the headset in a **thumbs-up**, thumb up and four fingers curled, and a
cube in your scene **turns red**. Drop the gesture and it goes back.

You will do this **without writing any gesture-detection code**. The gesture is described in two
ScriptableObject assets you author in the Inspector, an **`XRHandShape`** for the finger
positions and an **`XRHandPose`** that adds an orientation constraint, and detected by a
**`StaticHandGesture`** component that raises ordinary UnityEvents when the hand matches.

The only script in the scene is **`MaterialSwapper`** from Week 6, unchanged.

---

## Prerequisites

**From Weeks 1–8, and nothing else:**

- **Unity 6.3 LTS (`6000.3.x`)** and a project that already builds and runs on a headset,
  see [Software and Frameworks](../Guides/Software_and_Frameworks.md)
- An **XR Origin (VR)** rig in your scene, working, from Week 5
- Comfortable wiring a UnityEvent to a public method in the Inspector, from Week 8
- **Meta Quest Developer Hub** installed, for deploying the build
- A **Meta Quest 2 / 3 / 3S** and a USB-C cable that carries data

### The package

**Window → Package Manager → Unity Registry → XR Hands.** If Week 7 already installed it, it is
there and there is nothing to do. Written against **1.7.x**.

### The hand-tracking setup: five settings in three panels

Week 7 switched most of this on already, so treat it as a checklist rather than new work, and go
through all of it. **Missing any one of these produces "my hands don't work" with no error
message, no warning, and nothing in the console.** There is nothing to debug because nothing has
gone wrong. A switch is off.

1. Enable **OpenXR** in **Project Settings → XR Plug-in Management**.
2. Enable the **Hand Tracking Subsystem** under **OpenXR Feature Groups**.
3. Enable **Meta Quest Support**, ***on the Android tab only***. This is the step people miss:
   the Android tab and the desktop tab are configured separately.
4. Enable **Meta Hand Tracking Aim**.
5. Add an **Interaction Profile**, the **Oculus Touch Controller Profile**. You need one even
   though this activity is about hands, because a project with no profile can fail to initialise
   at all, taking hand tracking down with it.

> **Checkpoint.** In **Project Settings → XR Plug-in Management → OpenXR**, on the **Android**
> tab specifically: hand tracking ticked, **Meta Quest Support** ticked, **Meta Hand Tracking
> Aim** ticked, and at least one interaction profile listed. Run **Project Validation** while
> you are there and clear anything it flags.

---

## Instructions

### Step 1: Import the Gestures sample

**You cannot use `StaticHandGesture` without this.** The component ships in a sample rather than
in the package's runtime assembly, so if you skip this step it will not exist in the Add
Component menu.

1. **Window → Package Manager → XR Hands → Samples**.
2. Import **Gestures**. It lands in `Assets/Samples/XR Hands/<version>/Gestures`, with your
   installed version number in the path.
3. Import **HandVisualizer** as well. The Gestures sample will not pass Project Validation
   without it, and debugging a gesture with invisible hands is guesswork.
4. If Unity offers to import **TMP Essential Resources**, accept. The sample's on-screen
   readouts do not build without them.

> **If a sample's components do not appear in Add Component, restart Unity.**

Two things worth opening before you build anything of your own:

* **Example hand shapes** in `Gestures/Examples/Hand Shapes`, ready-made `XRHandShape` assets
  showing how the fields are used in practice.
* **The sample's `HandGestures` scene**, which carries the gesture debugging tools covered in
  Step 7.

### Step 2: A cube, a material, and a swap

Nothing hand-related here. Build the *outcome* first, so that when the gesture starts firing you
already trust the thing it is firing at.

1. In your scene, **GameObject → 3D Object → Cube**. Name it `GestureCube` and put it at about
   `(0, 1.2, 1)`, roughly chest height and an arm's length in front of the rig.
2. Scale it to `(0.2, 0.2, 0.2)`. A default cube is a metre across, far too large at arm's
   length in a headset.
3. Create two materials with **Assets → Create → Material**, named `CubeNeutral` and `CubeRed`.
   Set `CubeNeutral` to a plain light grey and `CubeRed` to a strong red.
4. Add **[MaterialSwapper](../Week%2006/Scripts/MaterialSwapper.cs)** to `GestureCube`, the same
   script you used in Week 6, unchanged. Drag `GestureCube` itself into **Target Object**,
   `CubeNeutral` into **Material A**, and `CubeRed` into **Material B**.

`MaterialSwapper` gives you `SwapToMaterialB()` and `SwapToMaterialA()`, which are the two
things a gesture starting and a gesture ending need to call. It also logs each swap, which will
matter when you are reading device logs later.

### Step 3: Something to raise hand events

`StaticHandGesture` does not talk to the hardware directly. It listens to a component that
represents *one hand* and raises an event each time that hand's data updates.

1. Under your **XR Origin (VR)**, create an empty GameObject named `Right Hand Events`.
2. Add an **`XRHandTrackingEvents`** component to it.
3. Set its **Handedness** to **Right**.

This component finds the hand subsystem itself, so it does not have to be a child of the
XR Origin. Keeping it there only makes the scene easier to read.

> **If the hands never appear in your build**, check `XRInputModalityManager`, an XR Interaction
> Toolkit component that switches the rig between tracked hands and controllers. It is not
> required here, since the gesture works either way.

### Step 4: Author the hand shape

An **`XRHandShape`** describes **what the fingers are doing, and nothing else**. No position, no
rotation, no direction, just curls and pinches, per finger, each with a tolerance.

Create one: **Assets → Create → XR → Hand Interactions → Hand Shape**, named `ThumbsUpShape`.

A thumbs-up in words is *the thumb is straight, and the four fingers are rolled into a fist*. In
the asset, that is five finger conditions. Under **Finger Shapes**, add an entry per finger,
each with a **Shape** dropdown, a **Target** slider, and a **Threshold** row:

| Finger | Shape | Target | Lower / upper threshold |
|---|---|---|---|
| Thumb | Full Curl | `0.0` (straight) | `0.25` / `0.25` |
| Index | Full Curl | `1.0` (fully curled) | `0.15` / `0.15` |
| Middle | Full Curl | `1.0` | `0.15` / `0.15` |
| Ring | Full Curl | `1.0` | `0.15` / `0.15` |
| Little | Full Curl | `1.0` | `0.15` / `0.15` |

The **Shape** options are **Full Curl**, **Base Curl**, **Tip Curl**, **Pinch** and **Spread**.
Two are missing for some fingers: the thumb has no **Pinch**, and the little finger has no
**Spread**.

Two things about those numbers:

- **Curl is normalised, not measured.** It runs from `0` (straight) to `1` (fully rolled up)
  regardless of finger length, which is why the same asset works for large and small hands.
- **The thresholds decide how the gesture behaves.** A condition passes while the measured value
  sits between `Target - lower` and `Target + upper`, so a gesture can be fussy in one direction
  and forgiving in the other. Too tight and it never fires, because nobody makes a perfect fist.
  Too loose and it fires during ordinary hand movement. The values above are Unity's own, but
  you will get them right in Step 7, on your own hands.

### Step 5: Add orientation with a hand pose

**The shape you just authored fires when your hand is upside down.** Thumb straight, four
fingers curled, is still true if your thumb points at the floor. A shape has no idea which way
up your hand is, and a thumbs-up that fires for a thumbs-down is not a thumbs-up. That is the
reason `XRHandPose` exists.

An **`XRHandPose`** is a shape **plus** an **`XRHandRelativeOrientation`**: the same finger
description, with a constraint on how the hand is oriented relative to the user or to the
XR Origin.

1. **Assets → Create → XR → Hand Interactions → Hand Pose**, named `ThumbsUpPose`.
2. Set its **Hand Shape** to `ThumbsUpShape`.
3. Under **Relative Orientation → User Conditions**, press **+** and set **Hand Axis** to
   **Thumb Extended Direction**, **Alignment Condition** to **Aligns With**, **Reference
   Direction** to **Origin Up**, and **Angle Tolerance** to about **60** degrees, which is the
   value Unity uses in its own thumbs-up pose.

The relative orientation lives inside the pose asset; there is no second asset to create. The
other hand axes are **Palm Direction** and **Fingers Extended Direction**, the other alignment
conditions are **Perpendicular To** and **Opposite To**, and the other reference directions are
**Hand To Head**, **Nose Direction**, **Chin Direction** and **Ear Direction**.

> **The orientation check needs an XR Origin and a camera in the scene.** Without both it
> reports no match and says nothing, which looks exactly like a gesture you described badly.

> **"Relative to what" decides what the gesture means.** **Origin Up** is gravity's up, so the
> gesture means the same thing whichever way the player faces but stops working if they lie
> down. User-relative up follows the player's head. A thumbs-up is about gravity, so
> origin-relative is what people expect.

### Step 6: Detect it, and wire the events

1. Add a **`StaticHandGesture`** component to `Right Hand Events`, alongside the tracking events
   component.
2. Point its **Hand Tracking Events** field at the `XRHandTrackingEvents` on the same GameObject.
3. Point its **Hand Shape Or Pose** field at `ThumbsUpPose`. One field takes either kind of
   asset, which is how the mistake in the checkpoint below happens.
4. Set **Minimum Hold Time** to about `0.2` seconds, and leave the detection interval at its
   default.
5. Assign a UI **Image** to its **Background** field. The component colours that image to show
   its state, and it does not check whether the field is empty, so **an unassigned Background
   throws an error on the first frame and no gesture is ever detected**. If you would rather not
   build a UI, drag in the sample's `Gestures/Examples/Prefabs/One Hand Static Gesture` prefab,
   which has one already.
6. Wire the events:
   * **Gesture Performed** → `GestureCube` → `MaterialSwapper.SwapToMaterialB`
   * **Gesture Ended** → `GestureCube` → `MaterialSwapper.SwapToMaterialA`

> **Minimum hold time is your anti-flicker setting.** Tracked hands are noisy, and a gesture
> tested every frame with no hold time fires and un-fires as your hand passes through the shape
> on its way somewhere else. A fifth of a second removes almost all of that and is short enough
> that nobody perceives a delay. **If your cube strobes, this is the field.**

### Step 7: Build, wear it, and tune

**File → Build Profiles → Build and Run**, with **Android** as the platform. Deploy through
**Meta Quest Developer Hub**. Put the headset on, put the controllers down, hold your hands up
until you see them, and give it a thumbs-up.

Three outcomes, all normal on a first build:

* **The cube turns red and stays red.** Thresholds too loose. Tighten them toward `0.1`.
* **Nothing ever happens.** Tolerances too tight, or your hands are not tracking at all. Check
  you can see your hands first; if you cannot, that is a setup problem, so go back to the five
  settings above.
* **It works, but only sometimes.** The good outcome, and the one worth tuning.

> **The gesture debugger is a scene, not a window.** Build and deploy the sample's
> **HandGestures** scene. On the headset it shows live per-finger values beside a hand shape's
> targets and thresholds, which turns "the gesture does not fire" into "my ring finger reads
> 0.62 and I asked for 1.0". You can point it at your own `ThumbsUpShape`.

**Tune several values per build.** Make **three cubes** side by side, each with its own
`MaterialSwapper`, and **three `StaticHandGesture` components** pointing at three copies of your
pose asset with thresholds of `0.1`, `0.15` and `0.25`. One build tells you which is right and
lets you see the wrong ones failing in the two different ways they fail. Then delete the two you
do not want.

> **Checkpoint.** Thumbs-up turns the cube red. Opening your hand turns it back. A
> thumbs-*down* does **not** turn it red. If it does, you have wired the gesture to the shape
> rather than to the pose, so the orientation constraint is not being applied.

---

## Understanding the shape → pose → gesture chain

Three assets, three jobs:

- **`XRHandShape` answers "what are the fingers doing?"** Pure finger configuration, normalised
  so it is body-size independent. It knows nothing about where the hand is or which way it
  points, which is why one shape asset is reusable across every direction a hand can face.
- **`XRHandPose` answers "and which way is the hand facing?"** A shape plus an
  `XRHandRelativeOrientation`. The same fist shape becomes a thumbs-up, a thumbs-down or a
  fist-bump depending only on the orientation you pair it with.
- **`StaticHandGesture` answers "is it happening right now?"** It compares hand data from
  `XRHandTrackingEvents` to your shape or pose within tolerance, applies the hold time so noise
  does not become events, and raises a UnityEvent when the answer changes.
- **What you get by configuring assets is somebody else's definition of "close enough"**,
  already debugged. That is a real saving, and also the thing you cannot change: you tune
  through tolerance fields, and when a gesture misfires the Inspector tells you *that* it
  matched but not *why*.

The word to notice is **static**. This chain describes a hand frozen in time, with no vocabulary
for a wave, a swipe or a pinch-then-drag. If you can describe your gesture in one sentence with
no verb of motion in it, this chain will do it quickly. If your sentence needs "then" or
"while", you have hit the ceiling, and no amount of tolerance tuning moves it.

---

> **Activity 2a ends here.** It is a complete route to a working gesture, with no
> gesture-detection code in it.
> [Activity 2b](Activity%202b%20-%20Hand%20Gestures%20from%20Joint%20Data.md) is optional. Take
> it when a gesture involves motion, timing or both hands, or when you need to see why a
> gesture fired.

## Extension Activities

- **Both hands.** Add a second `XRHandTrackingEvents` set to **Left**, a second
  `StaticHandGesture` on the same `ThumbsUpPose`, wired to the same cube. When both hands do it
  and one drops, the second hand's "ended" event turns the cube back even though the first is
  still holding. Count how many hands are performing the gesture rather than treating each
  event as the whole truth.
- **A second gesture that means something different.** Author an `OpenPalmShape`, all five
  fingers at Full Curl `0.0`, paired with a pose whose palm faces the user, and wire it to
  `SwapToMaterialA` explicitly. That turns two events from "on and off" into two commands.
- **Use a shipped shape.** Wire one of the assets in `Gestures/Examples/Hand Shapes` to your
  cube, to see which fields the package authors actually use.
- **Find the tolerance floor and ceiling.** Build with every tolerance at `0.05` and see that
  nothing fires; build again at `0.5` and see it fire at almost anything. Two builds, and the
  middle becomes meaningful.
- **Gate something that matters.** Replace the cube with something in your own prototype: a door
  that opens, a menu that appears, a tool that arms itself. A gesture that fires by accident
  during ordinary hand movement is worse than a button, and you only find out which of yours do
  that by using them.

---

## Headset checkpoint

Before you call this activity done:

- **Deploy** the `.apk` and confirm your hands appear with the controllers down.
- **Give** the headset a thumbs-up, and watch the cube turn red.
- **Give** it a thumbs-down, and confirm the cube does not change.
- **Record** which threshold values you settled on, and why.
- **Commit** the scene and both assets, along with the `manifest.json` change if XR Hands was
  not already installed.

## Outcome

A cube that turns red when you give the headset a thumbs-up, built out of two ScriptableObject
assets, one component, and one script you did not write for this, plus tolerances you tuned on a
real headset with your own hands, which is the only place they can be tuned.

You also have the vocabulary of hand poses: **curl, tolerance, orientation, hold time**. Those
four words cover most of what a static gesture is, and how much of this work is configuration
rather than code.

## References

**XR Hands**
- Hand shapes: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.7/manual/gestures/hand-shapes.html>
- Hand poses: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.7/manual/gestures/hand-poses.html>
- Static Hand Gesture component: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.7/manual/gestures/static-hand-gesture.html>
- Gesture debugger: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.7/manual/gestures/gesture-debugger.html>
- Package manual: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.7/manual/index.html>

**Course**
- Setup and build workflow: [OpenXR Unity Setup Guide](../Guides/OpenXR_Unity_Setup_Guide.md)
