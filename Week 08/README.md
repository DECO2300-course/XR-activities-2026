# Week 8: Visual, Audio and Haptic Feedback

Week 8 is about how an object responds when a player interacts with it. You make a cube
change colour, play a sound and vibrate the controller when it is grabbed, all from one
interaction event. Then you tune those responses so that four cubes read as four different
materials. Finally you combine both in a pair of cubes that vibrate more strongly as they
get closer. The visual and audio feedback can be built and tested in the XR Interaction
Simulator. Vibration has to be felt, so every activity finishes on the headset.

## Before You Start

- **Unity 6.3 LTS (`6000.3.x`)**, **VR template**, configured for Android and Meta Quest 2 /
  3 / 3S. See
  [OpenXR in Unity: Setup and Workflow Guide](../Guides/OpenXR_Unity_Setup_Guide.md)
- **Your Week 7 project.** It already has **XR Interaction Toolkit 3.6.x**, the
  **Starter Assets** sample from [Week 5](../Week%2005/README.md), and the Android build
  settings. Keep working in it rather than starting a new project
- **`MaterialSwapper.cs` from Week 6** in `Assets/Scripts/`. If it is not there, copy it in
  from [Week 6 Scripts](../Week%2006/Scripts/MaterialSwapper.cs)
- **A Meta Quest 2 / 3 / 3S with controllers, and a USB-C cable that carries data.** All
  three activities are *headset required*
- Commit at the start and the end of each activity

**Packages you'll add this week:** none. Audio Sources are part of Unity, and
`HapticImpulsePlayer` is part of the XR Interaction Toolkit.

> **Keyboard shortcuts.** Where these activities say `Ctrl`, macOS users press `Cmd`.
> `Ctrl+S` becomes `Cmd+S`, and `Ctrl+D` becomes `Cmd+D`.

> **macOS users, and anyone without Quest Link.** Every headset instruction this week is
> written as *build an `.apk` and deploy it*. Nothing this week depends on Editor-to-headset
> play mode.

## Learning Progression

1. **Interactable events**: where an XRI Interactable reports hover and select, and how to
   attach responses to them in the Inspector
2. **Three kinds of feedback**: a material change, a spatial sound and a controller
   vibration, all from the same event
3. **The right controller**: using the event's argument to vibrate the hand that grabbed
4. **Duration and amplitude**: what each one changes about how a vibration feels
5. **Materials**: adding vibration, sound and look one at a time so an object reads as metal,
   fabric, wood or glass
6. **Sustained vibration**: repeating impulses for as long as an object is held
7. **Distance-based haptics**: turning a measurement into an amplitude, and releasing a held
   object from code

## Activities

Work through these in order. Each activity builds on the scene from the one before.

- **[Activity 1](Activity%201%20-%20Reacting%20to%20Interaction%20Events.md)**: Reacting to
  Interaction Events · *headset required*
  - The **Interactable Events** foldout on `XRGrabInteractable`
  - A material change wired to **Select Entered** with `MaterialSwapper` from Week 6
  - A 3D **Audio Source**, and what **Spatial Blend** does
  - The `SimpleHapticFeedback` already on the Starter Assets rig, and switching it off
  - `HapticImpulsePlayer`, and a script that vibrates the controller that grabbed

- **[Activity 2](Activity%202%20-%20Tuning%20Feedback%20for%20Materials.md)**: Tuning
  Feedback for Materials · *headset required*
  - Comparing impulse durations, and trying different amplitudes, on the headset
  - Four material cubes, first with vibration only, then with sound and look added
  - Vibrating for as long as an object is held, and stopping on release

- **[Activity 3](Activity%203%20-%20Distance-Based%20Haptics.md)**: Distance-Based Haptics ·
  *headset required*
  - A script that reuses the haptics from Activities 1 and 2
  - Turning the distance between two held cubes into an amplitude
  - Releasing a held cube from code and pushing it away
  - Adding an idea of your own to the interaction

## C# Scripts

Reference copies of the scripts used in these activities are in the `Scripts/` directory.
Write your own first, and use these to check against.

- **[HapticPulse.cs](Scripts/HapticPulse.cs)**: sends one haptic impulse to the controller
  that grabbed the object (Activities 1 and 2)
- **[SustainedHaptic.cs](Scripts/SustainedHaptic.cs)**: repeats impulses on that controller
  for as long as the object is held (Activity 2)
- **[ProximityHaptics.cs](Scripts/ProximityHaptics.cs)**: vibrates both controllers more
  strongly as two held cubes get closer, and knocks one away when they touch (Activity 3)

Activity 1 also uses **[MaterialSwapper.cs](../Week%2006/Scripts/MaterialSwapper.cs)** from
Week 6.

## Audio

The four material clips for Activity 2 are in the **[Audio](Audio/)** directory. They are
from Kenney's [Impact Sounds](https://kenney.nl/assets/impact-sounds) pack, released under a
CC0 licence, which is included beside them.

## Outcome

A cube that responds to a grab with a colour change, a sound and a vibration on the
controller that grabbed it, four cubes that read as four materials, and a pair of cubes whose
vibration grows as they get closer. You will have felt on
the headset where a short impulse stops feeling like a tap, and how much each kind of
feedback adds to a material.
