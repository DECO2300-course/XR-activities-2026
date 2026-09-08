# Week 7: Locomotion, Comfort and World-Space UI

Week 7 is about the part of XR that a monitor cannot show you. Motion sickness is invisible
on a screen: a project can look flawless in the Editor and be unusable ten seconds after
somebody puts the headset on. This week you configure the standard ways of moving a player
through a world and the standard ways of putting a menu in front of them, then go and find
out what each one feels like. All three activities are on the headset.

## Before You Start

- **Unity 6.3 LTS (`6000.3.x`)**, **VR template**, configured for Android and Meta Quest 2 /
  3 / 3S — see
  [OpenXR in Unity — Setup and Workflow Guide](../Guides/OpenXR_Unity_Setup_Guide.md)
- **XR Interaction Toolkit 3.6.x** with the **Starter Assets** sample imported, from
  [Week 5](../Week%2005/README.md)
- The XRI vocabulary from
  [XR Interaction Toolkit — Core Concepts](../Guides/XRInteractionToolkit.md). Interactor,
  Interactable, XR Origin and Starter Assets are all assumed here
- Unity UI, canvases and TextMeshPro from
  [Week 3 Activity 2](../Week%2003/Activity%202%20-%20Unity%20UI.md). Activity 2 builds on it
  rather than repeating it
- **A Meta Quest 2 / 3 / 3S and a USB-C cable that carries data.** All three activities are
  *headset required*

**Packages you'll add this week:** **XR Hands** (`com.unity.xr.hands`) **1.6.x** in Activity 3,
plus two samples: **HandVisualizer** from XR Hands, and **Hands Interaction Demo** from the XR
Interaction Toolkit. Activities 1 and 2 add nothing: the tunnelling vignette is already inside
the **Starter Assets** sample you imported in Week 5.

> **Keyboard shortcuts.** Where these activities say `Ctrl`, macOS users press `Cmd` —
> `Ctrl+S` becomes `Cmd+S`.

> **macOS users, and anyone without Quest Link.** Every headset instruction this week is
> written as *build an `.apk` and deploy it*. Nothing this week depends on Editor-to-headset
> play mode.

> **Stop at the first symptom.** This week deliberately puts you in front of movement styles
> that make some people feel unwell. Take the headset off and stop for the day if you feel
> queasy, sweaty or headachy. Nobody is expected to push through nausea.

## Learning Progression

1. **Locomotion styles** — teleportation, continuous movement, snap turning and smooth
   turning, configured from XRI's own locomotion providers rather than written by hand
2. **Comfort mitigation** — the tunnelling vignette, and what it does to your field of view
3. **Comparing configurations** — trying each one on the headset and noticing what changes
4. **World-space UI** — canvas placement, distance and physical size in metres
5. **Attachment** — why head-locked interfaces are uncomfortable, and what body-locked and
   world-locked do instead
6. **Hands** — hand tracking, and driving that same panel with a fingertip instead of a ray

## Activities

Work through these in order. Activity 2 assumes you can move around the scene from Activity 1,
and Activity 3 drives the panel from Activity 2.

- **[Activity 1](Activity%201%20-%20Locomotion%20and%20Comfort.md)** — Locomotion and Comfort
  · *headset required*
  - Teleportation areas and anchors, and the interaction layer that makes them work
  - Continuous movement, snap turning and smooth turning from XRI's locomotion providers
  - The tunnelling vignette, and when it earns its cost
  - Six configurations compared on the headset

- **[Activity 2](Activity%202%20-%20World-Space%20UI.md)** — World-Space UI
  · *headset required*
  - World-space canvases sized in metres, not pixels
  - Placement: distance, height, angle, and the comfort zone
  - Ray interaction with `NearFarInteractor`, `TrackedDeviceGraphicRaycaster` and
    `XRUIInputModule`
  - Head-locked, body-locked and world-locked placement, felt rather than argued

- **[Activity 3](Activity%203%20-%20Hand%20Tracking%20and%20Poke.md)** — Hand Tracking and Poke
  · *headset required*
  - The XR Hands package, and the four toggles that switch hand tracking on
  - `XRInputModalityManager`, and swapping between controllers and hands
  - Poking the Activity 2 panel with a fingertip, and a slider you drag with it
  - `XRPokeFilter` on a physical button, and why a uGUI Button cannot have one
  - What a tracked hand costs you, and what it stops costing the player

## C# Scripts

**None in the main activities, and that is deliberate.** Locomotion is the one system in this
course you are told not to write yourself. XRI ships providers that are already correct about
frame-rate independence, tracking origin handling and provider arbitration. The week's work is
configuration and judgement. The Extension Activities do include small scripts, and those are
yours to write.

## Outcome

A scene you can move through in four different ways, a world-space menu you can point at from
across the room, and the same menu driven by your fingers with the controllers on the table.
More usefully, a habit: **you no longer trust a movement or UI decision that has only ever
been seen on a monitor.**
