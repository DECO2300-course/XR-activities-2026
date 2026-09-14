# Activity 3: Distance-Based Haptics

> **Headset badge: headset required.** The vibration changes as your hands move, so the
> whole activity is judged on a **Meta Quest 2 / 3 / 3S**.

## Objective
Combine what you built in Activities 1 and 2 into one interaction: hold a cube in each hand,
feel both controllers vibrate more strongly as the cubes get closer, and knock one out of
your hand when they touch. Then choose something of your own to add to it.

## Prerequisites
- **[Activity 2](Activity%202%20-%20Tuning%20Feedback%20for%20Materials.md)** complete. You
  need the `Metal` and `Glass` cubes, with their sounds and materials
- **Packages you'll add this week:** none
- Hardware: a **Meta Quest 2 / 3 / 3S** with controllers, and a USB-C cable that carries data

## Instructions

### Step 1: What you are reusing

This activity adds one script to your Activity 2 scene. Almost everything it does, you have
already done:

| In `ProximityHaptics` | Where you did it before |
|---|---|
| Finds the controller holding a cube, from its Interactor | `HapticPulse`, Activity 1 |
| Sends short impulses that overlap, so the vibration is continuous | `SustainedHaptic`, Activity 2 |
| Plays the cube's **Audio Source** | Activity 1 Step 5, with the clips from Activity 2 |
| The cube changes material when it is released | `MaterialSwapper` on **Select Exited**, Activity 1 |

The new parts are measuring the distance between the cubes, turning that distance into an
amplitude, and releasing a cube from code.

### Step 2: Add the script

1. In `Assets/Scripts/`, right-click → **Create → Scripting → MonoBehaviour Script**, and
   name it `ProximityHaptics`.
2. Replace its contents with this. A reference copy is in
   **[Scripts/ProximityHaptics.cs](Scripts/ProximityHaptics.cs)**.

```csharp
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Week 8, Activity 3. While this cube and a partner cube are both held, vibrates both
/// controllers more strongly the closer the cubes get. When they touch, the partner plays its
/// sound, is released from the hand holding it, and is pushed away.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class ProximityHaptics : MonoBehaviour
{
    [Tooltip("The other cube. This is the one that flies away.")]
    public XRGrabInteractable partner;

    [Tooltip("Distance between the cubes, in metres, at which the vibration starts.")]
    public float startDistance = 0.6f;

    [Tooltip("Distance between the cube centres, in metres, that counts as touching.")]
    public float touchDistance = 0.16f;

    [Range(0f, 1f)]
    [Tooltip("Amplitude when the cubes are at Start Distance.")]
    public float minAmplitude = 0.05f;

    [Range(0f, 1f)]
    [Tooltip("Amplitude just before the cubes touch.")]
    public float maxAmplitude = 1f;

    [Tooltip("Speed the partner is pushed away at, in metres per second.")]
    public float launchSpeed = 3f;

    // The same overlapping impulses as SustainedHaptic in Activity 2.
    const float k_PulseLength = 0.1f;
    const float k_Interval = 0.08f;

    XRGrabInteractable self;
    float nextPulseTime;

    void Awake()
    {
        self = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        if (partner == null || !self.isSelected || !partner.isSelected)
            return;

        var distance = Vector3.Distance(transform.position, partner.transform.position);

        if (distance <= touchDistance)
        {
            Touch();
            return;
        }

        if (distance > startDistance || Time.time < nextPulseTime)
            return;

        // 0 at Start Distance, rising to 1 at Touch Distance.
        var closeness = Mathf.InverseLerp(startDistance, touchDistance, distance);
        var amplitude = Mathf.Lerp(minAmplitude, maxAmplitude, closeness);

        Pulse(self, amplitude);
        Pulse(partner, amplitude);
        nextPulseTime = Time.time + k_Interval;
    }

    void Touch()
    {
        var sound = partner.GetComponent<AudioSource>();
        if (sound != null)
            sound.Play();

        // Release the partner from the hand holding it, the same as the player letting go.
        partner.interactionManager.SelectExit(partner.firstInteractorSelecting, partner);

        var direction = (partner.transform.position - transform.position).normalized;
        StartCoroutine(Launch(partner.GetComponent<Rigidbody>(), direction));
    }

    IEnumerator Launch(Rigidbody body, Vector3 direction)
    {
        // Throw On Detach sets the velocity at the end of the frame the cube was released in,
        // so wait one frame before pushing it.
        yield return null;
        body.linearVelocity = (direction + Vector3.up * 0.5f).normalized * launchSpeed;
    }

    // As in HapticPulse: the HapticImpulsePlayer is on the controller holding the cube.
    static void Pulse(XRGrabInteractable cube, float amplitude)
    {
        var player = cube.firstInteractorSelecting.transform.GetComponentInParent<HapticImpulsePlayer>();
        if (player != null)
            player.SendHapticImpulse(amplitude, k_PulseLength);
    }
}
```

3. Add `ProximityHaptics` to `Metal`.
4. Drag `Glass` from the Hierarchy into its **Partner** field. Leave the other values as they
   are.

This script is not wired to an event. It checks the two cubes in `Update` every frame, so
there is nothing to add under **Interactable Events**.

### Step 3: Build and try it

1. Build and deploy.
2. Pick up `Metal` in one hand and `Glass` in the other, and hold them apart.
3. Move them slowly towards each other. Both controllers start vibrating once the cubes are
   within 0.6 m, and the vibration gets stronger as they get closer.
4. Touch them together. `Glass` plays its clip, leaves your hand, and flies away.
5. Pick `Glass` up again and repeat.

### Step 4: Change how it feels

Change one value at a time, build, and compare:

| Field | Try | What changes |
|---|---|---|
| **Start Distance** | `0.3` | The vibration starts later, and rises faster |
| **Min Amplitude** | `0.3` | The vibration is already strong when it starts |
| **Launch Speed** | `1` | `Glass` drops out of your hand instead of flying |

## Understanding the script

**Held cubes do not report collisions with each other.** An `XRGrabInteractable` with the
default **Movement Type**, **Instantaneous**, makes its Rigidbody kinematic while it is held,
and two kinematic Rigidbodies do not raise `OnCollisionEnter`. The script measures the
distance between the cubes instead, and treats anything closer than **Touch Distance** as a
touch.

**Two steps turn a distance into an amplitude.** `Mathf.InverseLerp` turns the distance into
a value from `0` at **Start Distance** to `1` at **Touch Distance**. `Mathf.Lerp` then turns
that value into an amplitude between **Min Amplitude** and **Max Amplitude**. The same two
steps can turn any measurement into any setting.

**Releasing from code is the same as letting go.** `XRInteractionManager.SelectExit` ends the
selection exactly as releasing the grip does. **Select Exited** runs, so `MaterialSwapper`
changes `Glass` back to its resting material.

**The push waits one frame.** With **Throw On Detach** ticked, the XR Interaction Toolkit
sets a released cube's velocity from the hand's movement at the end of the frame it was
released in. A velocity set earlier in that frame would be replaced, so `Launch` waits one
frame before pushing.

## Extension Activities

This is a starting point. Choose one of these, or an idea of your own, and add it to the
interaction:

- **Sound that rises.** Give `Metal` a looping hum, and raise its **Pitch** as the cubes get
  closer, using the same closeness value.
- **Colour that changes.** Change the colour of both cubes as they approach, so the player can
  see what they feel.
- **A faster rhythm.** Shorten the time between impulses as the cubes get closer, instead of
  only raising the amplitude.
- **Only a hard hit counts.** Knock `Glass` away only if the cubes meet quickly. Compare their
  positions between frames to measure the speed.
- **Either cube can fly.** Push away whichever cube was moving faster, instead of always
  `Glass`.
- **It comes back.** Return `Glass` to the table after a few seconds, or into a socket from
  [Week 6 Activity 4](../Week%2006/Activity%204%20-%20Sockets.md).
- **React when not held.** Make the cubes respond when only one of them is held, or when
  neither is. Bring the held cube near the resting one and vibrate only the hand holding it,
  or throw one cube at the other and play a sound when they hit. `Update` currently stops
  unless both cubes are held, so check each cube's `isSelected` separately, and only send an
  impulse to a cube that is held. Only call `SelectExit` on a cube that is held, because a
  resting cube has no Interactor to release it from. A cube that is not held is not kinematic,
  so `OnCollisionEnter` does fire when a thrown cube hits it.
- **No controllers.** Tracked hands from Week 7 have no haptics. Decide what should replace
  the vibration for a player using hands.

## Headset checkpoint

Before you call this activity done:

- **Hold** `Metal` and `Glass` apart, then bring them together. The vibration in both
  controllers starts at a distance and grows as they get closer.
- **Touch** them. `Glass` plays its sound, leaves your hand and flies away, while `Metal`
  stays in the other hand.
- **Show** someone the idea you added from the Extension Activities.

## Outcome
Two cubes that vibrate both controllers more strongly as they approach and knock one out of
the hand when they touch, built from the pieces of Activities 1 and 2, plus one addition of
your own. Save the scene and commit.

## References

**XR Interaction Toolkit**
- `XRInteractionManager.SelectExit`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.XRInteractionManager.html>
- `XRGrabInteractable`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable.html>
- `HapticImpulsePlayer`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticImpulsePlayer.html>

**Unity**
- `Mathf.InverseLerp`: <https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Mathf.InverseLerp.html>
- `Mathf.Lerp`: <https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Mathf.Lerp.html>
