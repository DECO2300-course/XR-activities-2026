# Week 9: Adding Capabilities with New Packages

Every week up to now has run on the same set of packages. **Week 9 is about adding one to a
working project**, then proving the project still does what it did before. Activity 1 adds AR
Foundation and Unity OpenXR: Meta, so your content can find the floor, the walls and your desk.
Activity 2 adds XR Hands and turns a hand shape into an event. Activity 3 adds Meta's own SDKs
alongside the OpenXR project you already have, for a capability the OpenXR stack does not offer.
Meta's SDKs and Unity's OpenXR packages are separate routes to the same hardware, and Activity 3
is where they have to share a project.

> **Activity 3 is still being tested against the course stack**, because it installs Meta's
> own SDKs alongside the OpenXR project.

## Before You Start

- **Weeks 1 to 8 finished.** A project that builds and runs on a headset, with grabbing,
  locomotion and feedback working
- **Unity 6.3 LTS (`6000.3.x`)**, **VR template**, configured for Android and Meta Quest 2 /
  3 / 3S. See
  [OpenXR in Unity: Setup and Workflow Guide](../Guides/OpenXR_Unity_Setup_Guide.md)
- **A Meta Quest 2 / 3 / 3S with controllers, and a USB-C cable that carries data.** Every
  activity this week is *headset required*
- Commit before you add a package, and again once the project builds with it. Every package
  this week changes your `Packages/manifest.json`

> **On versions.** Take whatever Package Manager offers. This week was written against
> AR Foundation **6.5.0**, Unity OpenXR: Meta **2.5.0**, Unity OpenXR Plugin **1.15.1** and
> XR Hands **1.7.x**, and those numbers are recorded so you can match them if you need to, not
> because anything depends on them. Small differences are not worth chasing. If something here
> behaves oddly, updating is the first thing to try. The authority on the course stack is
> [Software and Frameworks](../Guides/Software_and_Frameworks.md).

## Activities

- **[Activity 1](Activity%201%20-%20Augmenting%20Reality.md)**: Augmenting Reality ·
  *headset required* · *Quest 3 / 3S, with a Quest 2 route*
  - **AR Foundation** and **Unity OpenXR: Meta**, and what they add to your project
  - The headset's Scene Model, and why the planes come from Space Setup rather than a scan
  - The scene permission, without which the managers return nothing
  - `ARPlaneManager` and `ARRaycastManager`, and placing an object on your real desk
  - Passthrough as a composition layer, and camera images as a separate opt-in path

- **[Activity 2a](Activity%202a%20-%20Hand%20Gestures%2C%20Configured.md)**: Hand Gestures,
  Configured · *headset required*
  - **XR Hands**, and the Gestures sample it ships
  - Describing a gesture as an `XRHandShape`, and adding orientation with an `XRHandPose`
  - Detecting it with `StaticHandGesture`, which raises ordinary events
  - Tuning tolerances until it fires when you mean it

- **[Activity 2b](Activity%202b%20-%20Hand%20Gestures%20from%20Joint%20Data.md)**: Hand
  Gestures from Joint Data · *headset required* · **optional, and follows on from 2a**
  - Subscribing to the hand subsystem and reading joint poses each frame
  - Which space those poses are in
  - Writing the test for a gesture in C#, with your own thresholds
  - The gestures a static shape cannot describe: motion, timing, and both hands

- **[Activity 3](Activity%203%20-%20Haptic%20Clips.md)**: Haptic Clips · *headset required*
  - Installing the **Meta XR Haptics SDK**, which brings the **Meta XR Core SDK** with it,
    and checking that nothing already built stops working
  - The clip library the SDK ships, and what a `.haptic` clip is
  - `HapticSource` with no code, then playing a clip on the hand that grabbed
  - Comparing a designed clip with the two dials from Week 8

## C# Scripts

- **[ScenePermission.cs](Scripts/ScenePermission.cs)**: requests the scene permission and
  enables the plane and raycast managers once it is granted (Activity 1)
- **[PlaneReport.cs](Scripts/PlaneReport.cs)**: logs how many planes the runtime knows about,
  so you get a number rather than an impression (Activity 1)
- **[PlaceOnPlane.cs](Scripts/PlaceOnPlane.cs)**: casts at the detected planes and places a
  prefab where the ray lands, with Step 7's classification filter included commented out
  (Activity 1)
- **[ThumbsUpDetector.cs](Scripts/ThumbsUpDetector.cs)**: detects a thumbs-up from hand joint
  data and raises events, the complete version of the script Activity 2b builds up
- **[HapticClipOnGrab.cs](Scripts/HapticClipOnGrab.cs)**: plays a haptic clip on the
  controller that grabbed an object (Activity 3)
- **[MaterialSwapper.cs](../Week%2006/Scripts/MaterialSwapper.cs)**: swaps a Renderer between two
  materials from gesture events (Activities 2a and 2b)

## When something does nothing

Most of what goes wrong this week fails quietly. No error, no warning, nothing in the log,
just a feature that produces nothing. Work down this table before you debug your own code.

| What you see | Check this first |
|---|---|
| A black void instead of your room | **Meta Quest: Camera (Passthrough)** is not ticked in the OpenXR feature group, on the **Android** tab |
| No planes, and no error | Space Setup has not been run on that headset, in that room, or the scene permission was dismissed |
| Your hands never appear | The hand-tracking settings, and that the rig has a hand branch to switch to |
| A gesture never fires | The pose asset is not assigned, or the detection component has no **Background** image |
| A component you just imported is missing | Restart Unity so it picks up the sample's scripts |
| A grab makes no sound or vibration | The listener is on the wrong event, or the rig's own feedback is masking yours |

**The habit to take from this week:** when a feature does nothing, ask whether it is
switched on, on this device, before asking what is wrong with your code.

## Outcome

A project that reads the real room and places objects on it, and one capability added from
Meta's own SDKs, with the rest of your prototype still working.

## After Week 9

Week 10 and beyond is a menu of optional modules, released separately. Take the ones your
prototype needs.
