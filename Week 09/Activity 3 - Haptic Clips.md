# Activity 3: Haptic Clips

> **Headset required.** A haptic clip has to be felt, and the XR Interaction
> Simulator has no controller to vibrate.
>
> **Packages you'll add: Meta XR Haptics SDK** (`com.meta.xr.sdk.haptics`), at whatever version
> Package Manager offers, which brings the
> **Meta XR Core SDK** (`com.meta.xr.sdk.core`) with it. This is the first time the course
> installs anything from Meta rather than from Unity, and it is a large dependency.

## Objective
Week 8 gave a controller two dials: amplitude and duration. This activity plays a designed
**haptic clip** instead, a vibration whose strength changes over its own length, and puts
Meta's SDK next to the OpenXR project you already have without breaking it.

## Prerequisites
- **[Week 8](../Week%2008/README.md)** complete. You need the `FeedbackCube` scene, with a
  grab that already answers with a colour change, a sound and an impulse
- **Packages you'll add this week:** the **Meta XR Haptics SDK**, and the **Meta XR Core SDK**
  that comes with it
- Hardware: a **Meta Quest 2 / 3 / 3S** with controllers, and a USB-C cable that carries data.
  The Quest 3 and 3S controllers reproduce a clip more faithfully than the Quest 2 ones, which
  vibrate at a fixed frequency
- **Commit before you start.** This activity adds a large dependency and changes your Android
  build settings. A clean commit is how you undo it in one action

## Instructions

### Step 1: Install the SDK

1. **Window → Package Manager → + → Install package by name**, and enter
   `com.meta.xr.sdk.haptics`.
2. Let it pull in `com.meta.xr.sdk.core`. Both arrive together and both appear in
   `Packages/manifest.json`.
3. Look at what else appeared in your project:
   - **`Assets/Oculus/OculusProjectConfig.asset`**, Meta's own settings file
   - **`Assets/Plugins/Android/AndroidManifest.xml`**, a **custom Android manifest**

The manifest is the one to notice. Unity generates a manifest for every Android build, and a
file in that location replaces the generated one. Yours now says which Quest models the app
supports and which Horizon OS version it needs.

> **Check that your project still builds before you add anything to it.** Build the Week 8
> scene to the headset now.

### Step 2: If the build runs out of memory

The Core SDK brings hundreds of megabytes of assets with it. On a large project, the Android
build step that compresses them can run out of memory, and Gradle reports
`java.lang.OutOfMemoryError` from a task such as `compressReleaseAssets`. If your build
succeeded in Step 1, skip this step.

1. **Edit → Project Settings → Player → Android → Publishing Settings**, and tick
   **Custom Gradle Properties Template**. Unity writes
   `Assets/Plugins/Android/gradleTemplate.properties`.
2. Open that file and set the heap on the first line:

```
org.gradle.jvmargs=-Xmx4096M -XX:MaxMetaspaceSize=1024M
```

3. Build again. It succeeds, and takes about as long as it did before.

### Step 3: Get some clips

A `.haptic` file is a designed vibration: an envelope of strength, and on capable controllers
frequency too, over a fixed length. Meta ships a library of them.

1. **Window → Package Manager → Meta XR Haptics SDK → Samples**.
2. Import **Meta XR Haptics Sample Packs**.
3. Look through `Assets/Samples/Meta XR Haptics SDK/.../Haptics`. The clips are grouped by
   what they are for: **Impacts**, **Objects**, **Weapons**, **Footsteps**, **Nature**,
   **Application_UX** and more.
4. Click one. Unity imports a `.haptic` file as a **Haptic Clip** asset, which is what the
   components and the API take.

### Step 4: Play a clip when the cube is grabbed

The SDK ships a component, so the first version needs no code.

1. Open your Week 8 scene and select `FeedbackCube`.
2. **Add Component → Haptic Source**.
3. Set **Clip** to an impact clip from the sample packs.
4. Leave **Controller** on **Both** for now.
5. On the **XR Grab Interactable**, under **Select Entered**, press `+`, drag in
   `FeedbackCube`, and choose **HapticSource → Play ()** under **Static Parameters**.
6. Remove the Week 8 **HapticPulse → Pulse** entry, so you feel one thing at a time.
7. Build and deploy, then grab the cube.

The other fields on the component are the clip's dials:

| Field | What it does |
|---|---|
| **Amplitude Scaling** | Multiplies the clip's strength, from `0` to `5` |
| **Frequency Shift** | Shifts its frequency, from `-1` to `1`, on controllers that can |
| **Loop** | Repeats the clip until something stops it |
| **Priority** | Which clip wins when two play at once, `0` highest |

### Step 5: Play it on the hand that grabbed

**Both** is a placeholder. The SDK addresses a controller by its side, so the script has to
work out which side grabbed, the same way `HapticPulse` did in Week 8.

1. In `Assets/Scripts/`, create a script named `HapticClipOnGrab`, and replace its contents
   with this. A reference copy is in
   **[Scripts/HapticClipOnGrab.cs](Scripts/HapticClipOnGrab.cs)**.

```csharp
using Oculus.Haptics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Week 9, Activity 3. Plays a haptic clip on the controller that grabbed this object.
/// Wire Play to Select Entered and Stop to Select Exited.
/// </summary>
public class HapticClipOnGrab : MonoBehaviour
{
    [Tooltip("The haptic clip to play. The Haptics SDK sample packs are full of them.")]
    public HapticClip clip;

    [Tooltip("Drag the rig's Left Controller here.")]
    public Transform leftController;

    [Tooltip("Drag the rig's Right Controller here.")]
    public Transform rightController;

    HapticClipPlayer player;

    void Awake()
    {
        if (clip != null)
            player = new HapticClipPlayer(clip);
    }

    // A player holds an unmanaged handle, so release it with the object.
    void OnDestroy()
    {
        player?.Dispose();
    }

    public void Play(SelectEnterEventArgs args)
    {
        if (player == null)
            return;

        // The SDK addresses a controller by its side, and the event says which Interactor
        // grabbed, so ask which controller that Interactor belongs to.
        var interactor = args.interactorObject.transform;

        if (rightController != null && interactor.IsChildOf(rightController))
            player.Play(Controller.Right);
        else if (leftController != null && interactor.IsChildOf(leftController))
            player.Play(Controller.Left);
    }

    public void Stop()
    {
        player?.Stop();
    }
}
```

2. Disable the **Haptic Source** component, and add `HapticClipOnGrab` to `FeedbackCube`.
3. Set **Clip** to the same clip, and drag the rig's `Left Controller` and `Right Controller`
   into their fields.
4. Under **Select Entered**, replace the Haptic Source entry with
   **HapticClipOnGrab → Play**, from the **Dynamic SelectEnterEventArgs** section.
5. Build and deploy. Grab with each hand in turn. Only the hand holding the cube plays the
   clip.

### Step 6: Compare a clip with an impulse

1. Put a second cube beside the first, and give it the Week 8 `HapticPulse` at amplitude `0.8`
   and duration `0.1`.
2. Build once, then grab them alternately.
3. Ask somebody else to grab both without telling them which is which, and say what the
   difference is in their own words.

An impulse is a single push with one strength. A clip is a shape: it can start hard and decay,
knock twice, or rumble. That is the whole difference, and it is worth feeling before deciding
whether a dependency this size is justified.

## Understanding haptic clips

- **A clip is authored, not computed.** Somebody designed the envelope in advance and your app
  plays it back. The two Week 8 dials are still there underneath, changing many times a second
  rather than once.
- **The two systems coexist.** `HapticImpulsePlayer` from XRI and `HapticClipPlayer` from Meta
  both drive the same motors, so send an impulse for a quick tap and play a clip for a texture.
  Priority decides which wins when two clips overlap.
- **Clip fidelity is a hardware question.** Quest 3 and 3S controllers vary frequency as well as
  strength; Quest 2 controllers vibrate at one frequency, so a clip designed around frequency
  changes arrives flattened. Test on the oldest headset you expect somebody to use.
- **A player holds a native handle.** `HapticClipPlayer` is not a plain C# object. Dispose of it
  when the object goes away, as the script does, or you leak handles across scene loads.
- **Dependencies have a footprint.** This one added hundreds of megabytes, a custom Android
  manifest and a settings asset. That is the kind of thing to know about a package before you
  add it to a project somebody else will have to build.

## Extension Activities

- **Design your own clip.** Install **Meta Haptics Studio**, import a short sound and let it
  generate a haptic clip from the audio. Adjust the envelope, export a `.haptic` file, and
  compare your clip with the shipped one for the same kind of impact.
- **A clip that loops while you hold it.** Replace Week 8's `SustainedHaptic` with a looping
  clip: **Loop** on, **Select Entered** to play, **Select Exited** to stop. Decide which of the
  two feels more like a running tool, and why.
- **Scale a clip by how hard you hit.** Use the speed calculation from Week 8's impact extension
  to set `amplitude` on the player before playing the clip, so the same clip serves a light tap
  and a heavy one.
- **Make four materials again.** Give the four Week 8 material cubes clips from the sample packs
  instead of impulses. Ask somebody to name the materials, as you did in Week 8, and compare how
  far the two approaches got you.

## Headset checkpoint

Before you call this activity done:

- **Your project still builds** with the SDK installed, and you know which setting you changed
  to keep it building.
- **Grabbing the cube plays a clip**, and only on the hand that grabbed.
- **You have felt a clip and an impulse back to back** and can say what the difference is.
- **Commit** the scene, the script and the `manifest.json` change together.

## Outcome
A cube that answers a grab with a designed vibration rather than a single push, playing on the
correct hand, in a project that still builds and still does everything it did in Week 8. You
also have a sense of what a vendor SDK costs to take on, which is the more transferable half.

## References

**Meta**
- Haptics SDK for Unity: <https://developers.meta.com/horizon/documentation/unity/unity-haptics-sdk/>
- Meta Haptics Studio: <https://developers.meta.com/horizon/documentation/unity/haptics-studio/>
- Haptics Studio download: <https://developers.meta.com/horizon/resources/haptics-studio/>

**Course**
- Week 8, where the two dials come from: [Week 8](../Week%2008/README.md)
- Setup and build workflow: [OpenXR Unity Setup Guide](../Guides/OpenXR_Unity_Setup_Guide.md)
