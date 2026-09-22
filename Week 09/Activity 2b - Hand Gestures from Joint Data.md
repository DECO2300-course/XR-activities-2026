# Activity 2b: Hand Gestures from Joint Data

> **This activity is optional, and it follows directly on from
> [Activity 2a](Activity%202a%20-%20Hand%20Gestures%2C%20Configured.md).** Do 2a first. This one
> is the code route, for gestures a static shape cannot describe and for seeing exactly why a
> gesture fired.
>
> **Headset required**, same as 2a: hand tracking cannot be simulated, so every test is a build
> and a deploy. Step 6 shows you how to get four answers out of one build instead of one.

## What you already have from Activity 2a

Everything in this list is done. If any of it is not, go back to 2a.

- **XR Hands** installed, written against **1.7.x**, and the **five hand-tracking settings**
  enabled across the three panels, with Project Validation clear on the **Android** tab
- The **HandVisualizer** sample imported, so you can see your hands on device
- **`GestureCube`** in your scene at about `(0, 1.2, 1)`, scaled to `(0.2, 0.2, 0.2)`, with
  **[MaterialSwapper](../Week%2006/Scripts/MaterialSwapper.cs)** on it and the `CubeNeutral` and
  `CubeRed` materials assigned
- An **XR Origin (VR)** rig with its Tracking Origin Mode on **Floor**, from Week 5

You do **not** need the Gestures sample, `XRHandTrackingEvents` or `StaticHandGesture` here. You
are replacing all three with one script.

**New for this activity:** comfort with `Vector3.Distance`, `Vector3.Dot` and normalised
directions. If dot products are hazy, the one fact you need is in Step 5.

---

## Objective

The same outcome as 2a, a thumbs-up turning `GestureCube` red, reached by reading the tracked
hand yourself: subscribing to the hand subsystem, pulling joint poses out of the right hand each
frame, and deciding **in C#** what counts as a thumbs-up. Distances, directions and thresholds
are all yours, all tunable, and all things you can print out and inspect.

---

## Instructions

> **The steps below build one script up a piece at a time, and the pieces do not compile until
> Step 5 puts the last one in.** A complete, working copy is in
> **[Scripts/ThumbsUpDetector.cs](Scripts/ThumbsUpDetector.cs)**. Build your own and check it
> against that when something will not compile, or start from it and read the steps as an
> explanation of what each part is for.

### Step 1: Fields, and getting hold of the hand subsystem

Everything starts with the **`XRHandSubsystem`**, the running service that owns the tracked hand
data. You do not create it, since OpenXR starts it once the settings from 2a are in place. You
find it and ask it questions.

Create a script in `Assets/Scripts/` called **`ThumbsUpDetector`**. Start with the fields, since
every later step refers to one of them. The four thresholds are the dials you will tune in
Step 6, and each is explained where it is first used:

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;

public class ThumbsUpDetector : MonoBehaviour
{
    [Header("Scene references")]
    [Tooltip("Drag the XR Origin here. Joint poses are relative to it.")]
    [SerializeField] Transform m_XROrigin;

    [Header("Thresholds, all multiples of hand scale")]
    [SerializeField] float m_CurlThreshold = 1.3f;
    [SerializeField] float m_ThumbExtendThreshold = 1.4f;
    [SerializeField] float m_UpAlignment = 0.6f;
    [SerializeField] float m_HoldSeconds = 0.2f;

    [Header("Events")]
    public UnityEvent onGestureDetected;
    public UnityEvent onGestureEnded;

    static readonly List<XRHandSubsystem> s_Subsystems = new List<XRHandSubsystem>();

    XRHandSubsystem m_Subsystem;
    bool m_Detected;
    float m_StableTime;
}
```

Now find the subsystem. Everything from here goes inside that class:

```csharp
    void Update()
    {
        // Returns immediately once we are subscribed, so it is cheap to call every frame.
        TryFindSubsystem();
    }

    void TryFindSubsystem()
    {
        if (m_Subsystem != null && m_Subsystem.running)
            return;

        SubsystemManager.GetSubsystems(s_Subsystems);

        foreach (var subsystem in s_Subsystems)
        {
            if (!subsystem.running)
                continue;

            m_Subsystem = subsystem;
            m_Subsystem.updatedHands += OnUpdatedHands;
            return;
        }
    }
```

> **Do not do this once in `Start` and give up.** The subsystem is generally not running on the
> first frame, because OpenXR is still bringing the session up. That is why the search runs from
> `Update` rather than once at startup.

Unsubscribe when you are torn down, or a domain reload in the Editor leaves you with a dead
handler:

```csharp
    void OnDisable()
    {
        if (m_Subsystem != null)
            m_Subsystem.updatedHands -= OnUpdatedHands;

        m_Subsystem = null;
    }
```

### Step 2: Take delivery of a hand

The subsystem raises an event each time it has new hand data. Subscribe, and read the right hand
off the subsystem inside the handler.

```csharp
    void OnUpdatedHands(XRHandSubsystem subsystem,
        XRHandSubsystem.UpdateSuccessFlags successFlags,
        XRHandSubsystem.UpdateType updateType)
    {
        // This event fires twice per frame, once for game logic and once before rendering.
        // A gesture is game logic, so ignore the render-time call.
        if (updateType != XRHandSubsystem.UpdateType.Dynamic)
            return;

        var hand = subsystem.rightHand;
        if (!hand.isTracked)
        {
            SetDetected(false);
            return;
        }

        SetDetected(EvaluateThumbsUp(hand));
    }
```

Two things this handler gets right, both worth copying:

- **It handles "not tracked" explicitly.** Hands leave the tracking volume constantly. Handle
  only the "hand is there" case and your cube stays red forever the first time your hand goes
  out of view.
- **It does not run in `Update`.** New hand data arrives when the subsystem says it does, so
  doing the work in the event means you evaluate exactly once per hand update and never on
  stale data.

### Step 3: Read joints out of the hand

A tracked hand is a **skeleton of joints**, each with a position and a rotation: a wrist, a palm,
and a chain of joints per digit from knuckle to tip. Reading one is two operations, identify the
joint and ask for its pose, and the second can fail, because a joint can be occluded even when
the hand as a whole is tracked.

```csharp
    bool TryGetJointPosition(XRHand hand, XRHandJointID jointID, out Vector3 position)
    {
        position = Vector3.zero;

        var joint = hand.GetJoint(jointID);
        if (!joint.TryGetPose(out var pose))
            return false;

        position = ToWorld(pose.position);
        return true;
    }
```

The eight joints this activity reads, and the `XRHandJointID` for each:

| Joint | `XRHandJointID` | Why you want it |
|---|---|---|
| Wrist | `Wrist` | The fixed end of the hand, and the reference point for every distance |
| Middle knuckle | `MiddleProximal` | Its distance from the wrist is a decent measure of hand size |
| Thumb base and tip | `ThumbMetacarpal`, `ThumbTip` | Together they give you the direction the thumb points |
| The four fingertips | `IndexTip`, `MiddleTip`, `RingTip`, `LittleTip` | The curls you are testing |

> **`TryGetPose` returning `false` is information, not an error.** It means that joint is not
> confidently tracked this frame. Treat a failed read as "gesture not detected" rather than
> ignoring it, or you will be testing a thumbs-up using last frame's thumb.

### Step 4: Which space are these poses in?

**Getting this wrong costs more time than anything else in this activity.**

A position is only meaningful relative to something, and there are two candidates. **XR Origin
space** is measured relative to your rig, so the player walks three metres left and the numbers
do not change. **World space** is measured in Unity's world, so every joint position changes by
three metres.

**XR Hands gives you the first one.** Joint poses are measured relative to the tracking origin,
so you transform them by the XR Origin before comparing them with anything in world space, such
as `Vector3.up`. This assumes the rig's **Tracking Origin Mode** is **Floor**, as Week 5 set it;
on any other mode the tracking origin sits at the Camera Offset rather than at the XR Origin
root.

Write the conversion so it is obvious what you assumed:

```csharp
    [SerializeField]
    [Tooltip("Drag the XR Origin here. Joint poses are relative to it.")]
    Transform m_XROrigin;

    Vector3 ToWorld(Vector3 position) =>
        m_XROrigin != null ? m_XROrigin.TransformPoint(position) : position;
```

> **The symptom that tells you which way round it is.** Test the gesture twice: once standing
> where you started, and once after walking a couple of metres and turning ninety degrees.
> **A gesture that works in one spot and not in another has a space bug**, not a threshold bug,
> and no amount of tuning will fix it. Check that the **XR Origin** field is assigned.

The distance comparisons in Step 5 are immune to this, because a distance between two joints is
the same in either space. **Only the direction test cares**, because it is the only test that
compares against something outside the hand.

### Step 5: The three tests that make a thumbs-up

In one sentence: **find the thumb tip, find the four fingertips, and test that the thumb points
up relative to the hand while the fingers are curled.**

This is `EvaluateThumbsUp`, the method Step 2 called. Read the eight joints first, bail out if
any of them failed, then run the three tests in order:

```csharp
    bool EvaluateThumbsUp(XRHand hand)
    {
        // Every joint must read this frame. A joint the runtime is not confident about is a
        // reason to say "no gesture", not a reason to carry on with last frame's thumb.
        if (!TryGetJointPosition(hand, XRHandJointID.Wrist, out var wrist) ||
            !TryGetJointPosition(hand, XRHandJointID.MiddleProximal, out var middleProximal) ||
            !TryGetJointPosition(hand, XRHandJointID.ThumbMetacarpal, out var thumbMetacarpal) ||
            !TryGetJointPosition(hand, XRHandJointID.ThumbTip, out var thumbTip) ||
            !TryGetJointPosition(hand, XRHandJointID.IndexTip, out var indexTip) ||
            !TryGetJointPosition(hand, XRHandJointID.MiddleTip, out var middleTip) ||
            !TryGetJointPosition(hand, XRHandJointID.RingTip, out var ringTip) ||
            !TryGetJointPosition(hand, XRHandJointID.LittleTip, out var littleTip))
            return false;

        // Wrist to middle knuckle: a stable measure of this hand's size.
        var handScale = Vector3.Distance(wrist, middleProximal);
        if (handScale <= 0f)
            return false;

        // Test 1: all four fingers curled.
        var fingersCurled =
            IsCurled(indexTip, wrist, handScale) &&
            IsCurled(middleTip, wrist, handScale) &&
            IsCurled(ringTip, wrist, handScale) &&
            IsCurled(littleTip, wrist, handScale);

        if (!fingersCurled)
            return false;

        // Test 2: the thumb is extended.
        var thumbExtended =
            Vector3.Distance(thumbTip, wrist) > m_ThumbExtendThreshold * handScale;

        if (!thumbExtended)
            return false;

        // Test 3: the thumb points up.
        var thumbDirection = (thumbTip - thumbMetacarpal).normalized;
        return Vector3.Dot(thumbDirection, Vector3.up) > m_UpAlignment;
    }

    bool IsCurled(Vector3 fingertip, Vector3 wrist, float handScale) =>
        Vector3.Distance(fingertip, wrist) < m_CurlThreshold * handScale;
```

**The scale reference is the important line.** People's hands differ by a factor of about 1.4
from smallest to largest, so **every threshold is a multiple of `handScale`, not a distance in
metres**. Skip that and your gesture works for you and fails for the next person who puts the
headset on.

What each test is doing, and where its starting value came from:

- **Test 1, the fingers are curled.** A curled fingertip is pulled back towards the wrist, so a
  fingertip closer to the wrist than `m_CurlThreshold` times hand scale counts as curled. At
  `1.3`: an extended index fingertip sits at roughly twice hand scale from the wrist, a curled
  one at roughly one.
- **Test 2, the thumb is extended.** The same measurement in the opposite direction, at `1.4`.
- **Test 3, the thumb points up.** This is what stops a fist counting, and what stops a
  thumbs-*down* counting.

> **The one dot-product fact you need.** For two normalised directions, `Vector3.Dot` returns
> `1` when they point the same way, `0` at right angles, and `-1` when opposed. So `0.6` means
> "within about 53° of straight up". Loosen towards `0.4` (66°) if the gesture feels fussy;
> tighten towards `0.8` (37°) if it fires when it should not.

`Vector3.up` rather than the player's head, because a thumbs-up is a gesture about gravity. This
is also the test Step 4's coordinate space affects, because `Vector3.up` is world-space.

**Then debounce it.** Tracked hands are noisy, so require the answer to be stable for
`m_HoldSeconds` before acting on it. **If your cube strobes, this is the field.**

```csharp
    void SetDetected(bool conditionMet)
    {
        if (conditionMet == m_Detected)
        {
            m_StableTime = 0f;
            return;
        }

        m_StableTime += Time.deltaTime;
        if (m_StableTime < m_HoldSeconds)
            return;

        m_StableTime = 0f;
        m_Detected = conditionMet;

        if (m_Detected)
            onGestureDetected.Invoke();
        else
            onGestureEnded.Invoke();
    }
```

**That is the whole script.** Compare yours with
**[Scripts/ThumbsUpDetector.cs](Scripts/ThumbsUpDetector.cs)** before you build: if anything
does not compile, the usual cause is a method left outside the class or the fields from Step 1
not carried forward.

Then add `ThumbsUpDetector` to a GameObject, drag your **XR Origin (VR)** into the **XR Origin**
field, and wire the two events:

* **On Gesture Detected** → `GestureCube` → `MaterialSwapper.SwapToMaterialB`
* **On Gesture Ended** → `GestureCube` → `MaterialSwapper.SwapToMaterialA`

Raising UnityEvents rather than calling the swapper directly is worth the extra three lines:
your detector ends up knowing nothing about cubes or materials, and the next thing you want a
thumbs-up to do is wired in the Inspector rather than written into the script.

### Step 6: Build, wear it, and tune

**File → Build Profiles → Build and Run**, with **Android** as the platform. Deploy through
**Meta Quest Developer Hub**, put the controllers down, and give it a thumbs-up.

**Get four answers out of one build.** Put **four cubes** in a row, each with its own
`MaterialSwapper`, driven from four sets of events on your detector:

| Cube | Turns red when |
|---|---|
| 1 | The right hand is tracked at all |
| 2 | The four fingers are curled |
| 3 | The thumb is extended |
| 4 | The thumb points up, and therefore the whole gesture |

Now a failure tells you *where* it failed rather than *that* it failed. Cube 1 dark means a
tracking or setup problem, not a maths problem. Cubes 1 to 3 lit and cube 4 dark, only after you
have moved across the room, means Step 4. Three extra cubes and a handful of `bool` fields save
you four builds.

**Read the numbers.** `Debug.Log` output is readable on device through **Meta Quest Developer
Hub**'s log viewer. Log your three measured values, curl distance over hand scale, thumb
distance over hand scale and the dot product, a few times a second rather than every frame, and
you will be tuning against real numbers rather than guesses. Throttle it: logging every frame
will itself cost you frames.

> **Checkpoint.** Thumbs-up turns the cube red. Opening your hand turns it back. A
> thumbs-*down* does **not** turn it red. If it does, Test 3 is not running, or your alignment
> threshold is loose enough to accept anything. Then walk two metres, turn ninety degrees, and
> try again. If it stops working, that is Step 4, not tuning.

---

## Understanding what the joints give you

You have just done, by hand, what every gesture recogniser does: **take a skeleton, reduce it to
a handful of scalars, and compare those scalars with thresholds.** There is no other kind of
gesture detection. The interesting parts are which scalars you choose and where the thresholds
come from.

- **A hand is a skeleton, not a shape.** The runtime gives you per-joint positions and rotations
  and no notion of "curled", "pointing" or "thumbs-up". `IsCurled` is not a fact about hands, it
  is a sentence you wrote, and somebody else would reasonably write a different one.
- **Scale-relative thresholds are the difference between a demo and something usable.** Any time
  you compare a tracked human measurement against a constant, ask what that constant is a
  proportion of.
- **Distances are cheap, directions are not.** Distances between two joints on the same hand are
  the same number in any coordinate space. The moment you compare against something outside the
  hand, you have taken on a coordinate space assumption and must know which one.
- **Noise is not an edge case, it is the normal condition.** Optical tracking jitters, drops
  joints behind other joints, and loses the hand at the edge of view. The hold time and the
  explicit `isTracked` check are the difference between an event stream you can build on and one
  that fires forty times a second at the boundary.
- **This is why the ceiling is high.** Because you own the per-frame data, you can ask questions
  a still image cannot answer: how fast the hand is moving, whether it crossed the midline,
  whether the pinch happened before or after the raise, how far apart the two hands are. Each is
  a few lines in the same handler. What you paid for that reach is everything in this activity.

---

## Extension Activities

- **A wave, which needs time.** Detect the hand tracked, roughly open, and the wrist moving side
  to side across at least two direction changes within a second. Store the previous wrist
  position, difference it for a velocity, and count sign changes. The moment your gesture has
  "then" in its description, you need history: a small ring buffer rather than one frame.
- **Both hands, and the distance between them.** Read `leftHand` and `rightHand` in the same
  handler and compare their wrist separation against a proportion of arm span. This is the class
  of gesture a single-hand description cannot express at all, and it is about six lines here.
- **Draw what you are measuring.** Add `OnDrawGizmos` for the wrist, thumb tip and four
  fingertips, plus a line along the thumb direction. You cannot see gizmos on device, so also
  spawn four tiny spheres and move them to the joint positions each sample. Seeing where the
  runtime *thinks* your fingertips are explains more misfires than any log line.
- **Hysteresis instead of a hold time.** Keep two curl thresholds, a stricter one to start the
  gesture and a looser one to end it, picked by whether the gesture is currently detected. This
  is how real detectors avoid chatter, and it feels better: the gesture starts crisply and does
  not drop out when your hand relaxes slightly.
- **Log a session and tune offline.** Log your three measured values through one minute of
  varied hand movement, the gesture, near misses, and ordinary gesturing while talking. Pull the
  log off with Meta Quest Developer Hub and look at the ranges. Your thresholds should sit in the
  gap between "gesture" and "everything else", and you cannot see that gap without the
  distribution.

---

## Headset checkpoint

Before you call this activity done:

- **Deploy** the `.apk` and confirm your hands appear with the controllers down.
- **Give** the headset a thumbs-up, and watch the cube turn red.
- **Give** it a thumbs-down, and confirm the cube does not change.
- **Walk** across the room, turn around, and confirm the gesture still works there.
- **Commit** the script and the scene.

## Outcome

A cube that turns red when you give the headset a thumbs-up, driven by a detector you wrote,
reading joint positions off the tracked hand and reducing them to three tests and three
thresholds you chose and tuned yourself on real hardware.

You also know what hand tracking gives you, **a per-frame skeleton of joint poses, measured from
the tracking origin, with noise on it**, and therefore what any gesture system is doing
underneath. You have met the four problems that come with the territory: hand size, coordinate
space, tracking loss and jitter. They do not go away when a gesture gets more ambitious; there
are just more of them.

## References

**XR Hands**
- The hand data model: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.7/manual/hand-data/xr-hand-data-model.html>
- `XRHandSubsystem`: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.7/api/UnityEngine.XR.Hands.XRHandSubsystem.html>
- `XRHandJoint`: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.7/api/UnityEngine.XR.Hands.XRHandJoint.html>
- `XRHandJointID`: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.7/api/UnityEngine.XR.Hands.XRHandJointID.html>
- Package manual: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.7/manual/index.html>

**Course**
- Setup and build workflow: [OpenXR Unity Setup Guide](../Guides/OpenXR_Unity_Setup_Guide.md)
