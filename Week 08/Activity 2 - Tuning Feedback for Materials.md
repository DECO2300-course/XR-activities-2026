# Activity 2: Tuning Feedback for Materials

> **Headset badge: headset required.** Every comparison in this activity is made by feel.
> Set the scene up at your desk, then test it on a **Meta Quest 2 / 3 / 3S**.

## Objective
Feel how impulse duration and amplitude change a vibration, build four cubes that read as
metal, fabric, wood and glass by adding vibration, sound and look one at a time, and make a
cube vibrate for as long as it is held.

## Prerequisites
- **[Activity 1](Activity%201%20-%20Reacting%20to%20Interaction%20Events.md)** complete. You
  need `FeedbackCube` with its three **Select Entered** entries, and the rig's own haptics
  switched off
- **Packages you'll add this week:** none
- The four material clips in this week's **[Audio](Audio/)** folder. They are from Kenney's
  **Impact Sounds** pack, which is free to use under a CC0 licence
- Hardware: a **Meta Quest 2 / 3 / 3S** with controllers, and a USB-C cable that carries data

> **Keyboard shortcuts.** Where this activity says `Ctrl`, macOS users press `Cmd`.
> `Ctrl+D` becomes `Cmd+D`.

## What you can control

`HapticImpulsePlayer.SendHapticImpulse` sends one impulse to a controller. It has these
parameters:

| Parameter | What it sets | Range |
|---|---|---|
| `amplitude` | How strongly the motor vibrates | `0` to `1` |
| `duration` | How long the impulse lasts | Seconds |
| `frequency` | The vibration frequency. Only in the three-parameter version of the method | Hz, where `0` uses the controller's default |

This activity uses amplitude and duration.

<!-- VERIFY on device: whether Meta Quest 2 / 3 / 3S controllers respond to a non-zero
     frequency sent through SendHapticImpulse over OpenXR with the Starter Assets rig -->

The XR Interaction Toolkit has no haptic clip asset. A pattern longer than one impulse, such
as a steady vibration, is a series of impulses sent from a script. Step 4 does this.

## Instructions

> **Group your builds.** Steps 1 and 2 need one build between them. Set both up before you
> build.

### Step 1: Compare impulse durations

1. Select `FeedbackCube` and duplicate it five times (`Ctrl+D`). Name the copies
   `Pulse_0.05`, `Pulse_0.1`, `Pulse_0.2`, `Pulse_0.3` and `Pulse_0.5`.
2. Place the copies in a row along the back of the table, at `y = 0.825` and `z = 0.95`, with
   x at `-0.4`, `-0.2`, `0`, `0.2` and `0.4`.
3. On each copy, set **Haptic Pulse → Duration** to the number in its name, and
   **Amplitude** to `0.8`.
4. On each copy, tick **Mute** on the **Audio Source**, so that you feel the vibration on its
   own.
5. Build and deploy. Grab the cubes from left to right, several times.

Somewhere along the row, a tap becomes a continuous vibration. Try changing **Amplitude** on
a few of the cubes as well, and feel what that changes.

### Step 2: Four materials, vibration only

1. Duplicate `FeedbackCube` four times and name the copies `Metal`, `Fabric`, `Wood` and
   `Glass`.
2. Place them in a row along the front of the table, at `y = 0.825` and `z = 0.55`, with x at
   `-0.45`, `-0.15`, `0.15` and `0.45`.
3. On each copy, tick **Mute** on the **Audio Source**.
4. Set each copy's **Haptic Pulse** to these starting values:

| Cube | Amplitude | Duration |
|---|---|---|
| `Metal` | `0.9` | `0.04` |
| `Fabric` | `0.15` | `0.08` |
| `Wood` | `0.5` | `0.05` |
| `Glass` | `0.9` | `0.02` |

5. Build and deploy, or use the build from Step 1 if you set both up together. Grab the four
   cubes in turn.

All four still look the same and make no sound. Notice how much of the difference between
them comes through the vibration alone.

### Step 3: Add sound and look

1. Copy the four `.ogg` files from this week's **[Audio](Audio/)** folder into
   `Assets/Audio/` in your project. Create the folder if it does not exist.
2. On each of the four cubes, untick **Mute**, and drag its clip into the **Audio Source**'s
   **Audio Generator** field:

| Cube | Clip |
|---|---|
| `Metal` | `impactMetal_light_000` |
| `Fabric` | `impactSoft_medium_000` |
| `Wood` | `impactWood_light_000` |
| `Glass` | `impactGlass_light_000` |

3. For each cube, create a resting material and a held material that look like its name, and
   assign them to **Material A** and **Material B** on its **Material Swapper**.
4. Build and deploy. Grab the four cubes in turn again.

Notice what changed between this build and the last one. Change any values that do not suit
the material, and rebuild.

> **Glass needs a transparent material.** Select the material, set **Surface Type** to
> **Transparent**, and lower the alpha of its base colour.

### Step 4: Vibrate while the object is held

An impulse suits a moment, such as picking something up. It does not suit something that
goes on, such as holding a power tool that is running. A single long impulse ends while the
player is still holding the object.

To keep the controller vibrating, send short impulses one after another, each starting
before the previous one has finished.

1. In `Assets/Scripts/`, right-click → **Create → Scripting → MonoBehaviour Script**, and
   name it `SustainedHaptic`.
2. Replace its contents with this. A reference copy is in
   **[Scripts/SustainedHaptic.cs](Scripts/SustainedHaptic.cs)**.

```csharp
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

/// <summary>
/// Week 8, Activity 2. Keeps the selecting controller vibrating for as long as this object is
/// held. Wire StartBuzz to Select Entered and StopBuzz to Select Exited.
/// </summary>
public class SustainedHaptic : MonoBehaviour
{
    [Range(0f, 1f)]
    [Tooltip("How strongly the motor vibrates, from 0 to 1.")]
    public float amplitude = 0.3f;

    [Tooltip("How long each impulse lasts, in seconds. Keep it longer than Interval.")]
    public float pulseLength = 0.1f;

    [Tooltip("Time from the start of one impulse to the start of the next, in seconds.")]
    public float interval = 0.08f;

    HapticImpulsePlayer player;
    Coroutine buzzRoutine;

    public void StartBuzz(SelectEnterEventArgs args)
    {
        if (buzzRoutine != null)
            return;

        player = args.interactorObject.transform.GetComponentInParent<HapticImpulsePlayer>();
        if (player == null)
            return;

        buzzRoutine = StartCoroutine(Buzz());
    }

    public void StopBuzz()
    {
        if (buzzRoutine == null)
            return;

        StopCoroutine(buzzRoutine);
        buzzRoutine = null;
    }

    // Disabling a component does not stop its coroutines, so stop this one here.
    void OnDisable()
    {
        StopBuzz();
    }

    IEnumerator Buzz()
    {
        // Each impulse starts before the previous one ends, so there is no gap between them.
        while (true)
        {
            player.SendHapticImpulse(amplitude, pulseLength);
            yield return new WaitForSeconds(interval);
        }
    }
}
```

3. Add `SustainedHaptic` to `FeedbackCube`, the cube in the middle of the table.
4. On its **XR Grab Interactable**, select the **HapticPulse → Pulse** entry under
   **Select Entered** and press `-` to remove it.
5. Under **Select Entered**, press `+`, drag in `FeedbackCube`, and choose
   **SustainedHaptic → StartBuzz** from the **Dynamic SelectEnterEventArgs** section.
6. Under **Select Exited**, press `+`, drag in `FeedbackCube`, and choose
   **SustainedHaptic → StopBuzz ()**.
7. Build. Hold the cube for several seconds, then let go. The controller vibrates for as
   long as you hold it and stops when you release.

> **Stop what you start.** Without the **StopBuzz ()** entry on **Select Exited**, the
> controller keeps vibrating after you let go. The XR Interaction Simulator cannot show
> this, so check the entry is there before you build.

## Understanding haptic feedback

**Duration decides whether a vibration feels like a tap.** A short impulse feels like a
single tap. A long impulse feels like a continuous vibration. Amplitude sets how strong
either one is.

**A controller vibration mostly carries strength and length.** Amplitude and duration can
make one object feel harder or softer than another. They say much less about what the
object is made of. Sound and appearance carry most of that.

**Feedback that starts together reads as one event.** All three kinds of feedback are wired
to the same event, so they start in the same frame. A sound that plays noticeably after the
vibration is perceived as a second event.

**Strong feedback needs weaker feedback around it.** If every interaction in a scene uses
the same strong pulse, the player cannot tell important interactions from ordinary ones.
Light feedback for ordinary interactions leaves room for strong feedback to stand out.

**Some players do not get every kind of feedback.** A player may turn vibration or sound
off, and a tracked hand from Week 7 has no haptics at all. An interaction that relies on
one kind of feedback gives those players nothing.

## Extension Activities

### **Rhythm**
Duplicate `FeedbackCube`, and on the copy set **Sustained Haptic → Interval** to `0.15`,
leaving **Pulse Length** at `0.1`. The impulses no longer overlap, so there are gaps between
them. Try other values and find the point where the gaps become noticeable.

### **Impact strength from speed**
Make the controller vibrate when the held cube hits something, harder for a faster hit.

Logic: set the cube's **Movement Type** to **Velocity Tracking** (Week 6 Activity 1). The
default, **Instantaneous**, makes the Rigidbody kinematic while held, and a kinematic
Rigidbody does not receive `OnCollisionEnter` when it hits a static Collider such as the
table. In `OnCollisionEnter`, read `collision.relativeVelocity.magnitude` and turn it into a
value from `0` to `1` with `Mathf.InverseLerp` between a minimum and a maximum speed. Get the
holding Interactor from the `XRGrabInteractable`'s `firstInteractorSelecting`, find its
`HapticImpulsePlayer` the same way `HapticPulse` does, and send the impulse. Only send it
while the cube is held, and set a minimum amplitude, because a very weak impulse may not be
felt.

### **Materials as data**
Move each material's values into a `ScriptableObject` asset, so every metal object in a
project can share one set of values:

```csharp
using UnityEngine;

[CreateAssetMenu(menuName = "Feedback/Material Feel")]
public class MaterialFeel : ScriptableObject
{
    [Range(0f, 1f)] public float amplitude = 0.5f;
    public float duration = 0.1f;
    public AudioClip clip;
    public Material heldMaterial;
}
```

Logic: write one feedback script that takes a `MaterialFeel` and drives all three kinds of
feedback from it. Create four assets. Changing the `Metal` asset then changes every object
that uses it.

### **Feedback settings**
Add three settings, for visual, audio and haptic feedback, that every feedback script checks
before it acts. Logic: a `ScriptableObject` or a static class holding three `bool` values.
Try the scene with each kind of feedback turned off in turn, and note which interactions
become hard to understand.

### **Feedback on a socket**
Add an `XRSocketInteractor` to the table, as in
[Week 6 Activity 4](../Week%2006/Activity%204%20-%20Sockets.md), and give it a sound and a
visual change when a cube is placed in it and a different one when the cube is removed.
Logic: the socket's **Interactor Events** include **Select Entered** and **Select Exited**.
The socket selects the cube after the hand has let go, so there is no hand to vibrate at
that moment.

## Headset checkpoint

Before you call this week done:

- **Grab** the five `Pulse_` cubes from left to right, and feel the change from a tap to a
  continuous vibration.
- **Grab** the four material cubes. With sound and look added, each reads as its material.
- **Hold** `FeedbackCube` for several seconds. It vibrates the whole time and stops the
  moment you let go.
- **Ask** someone else to grab the four material cubes and say which material each one is.

## Outcome
A row of cubes that shows how impulse duration changes a vibration, four cubes that read as
metal, fabric, wood and glass, and a cube that vibrates for as long as it is held. Save the
scene and commit.

## References

**XR Interaction Toolkit**
- `HapticImpulsePlayer`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticImpulsePlayer.html>
- `XRGrabInteractable`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable.html>
- `XRSocketInteractor`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor.html>

**Unity**
- `MonoBehaviour.StartCoroutine`: <https://docs.unity3d.com/6000.3/Documentation/ScriptReference/MonoBehaviour.StartCoroutine.html>
- `ScriptableObject`: <https://docs.unity3d.com/6000.3/Documentation/ScriptReference/ScriptableObject.html>

**Audio**
- Kenney, *Impact Sounds*, CC0: <https://kenney.nl/assets/impact-sounds>
