# Week 5: Into the Headset

Week 5 is where you build a small XR scene from the **VR template** — a rig, a floor, 
objects measured in metres, and something you can pick up — and then you run it through 
both of the workflows you will use every week from here: the **XR
Interaction Simulator** at your desk, and a real build on a real headset. Two ideas carry the
week. The first is that **the camera's position is an output, not an input**: you move the XR
Origin, never the camera. The second is that the two workflows are a pair, not alternatives.
One tells you whether your work runs, and only the other tells you whether it is any good.

## Before You Start

- **Unity 6.3 LTS (`6000.3.x`)**, starting a new project from the **VR template** — see
  [Software and Frameworks](../Guides/Software_and_Frameworks.md)
- **The [Week 3 checkpoint](../Week%2003/Activity%205%20-%20Getting%20Set%20Up%20for%20XR.md)
  finished.** Your project builds and runs on a headset. **Week 5 does not revisit setup.** If a
  package is missing or a project setting is wrong, fix it against
  [the OpenXR setup guide](../Guides/OpenXR_Unity_Setup_Guide.md), which is the only authority
  on configuration in this course
- **Packages you'll add this week:** none, but Activity 1 Step 2 **upgrades the XR Interaction
  Toolkit to 3.6.0** and **imports the XR Interaction Simulator sample**. The VR template
  installs XRI 3.4.1 (some Hub versions offer a newer template with 3.5.1) plus the Starter
  Assets sample, and it does not install the simulator. Package Manager will mark **3.3.2** as
  *Recommended* — that is the version bundled with the Editor, and this course goes past it
  deliberately
- Read [XR Interaction Toolkit — Core Concepts](../Guides/XRInteractionToolkit.md) before
  Activity 1, at least *Interactors, Interactables, Transformers* and *The rig*. The activities
  use that vocabulary and deliberately do not re-explain it
- **Book a headset now.** Activity 2 needs a Meta Quest 2, 3 or 3S and a USB-C cable that
  carries data. The headsets are shared and there are fewer of them than there are of you
- Commit at the start and the end of each activity, and not in between

> **Keyboard shortcuts.** Where these activities say `Ctrl`, macOS users press `Cmd` —
> `Ctrl+S` becomes `Cmd+S`, `Ctrl+D` becomes `Cmd+D`.

> **Mac users.** Everything this week works on macOS. You cannot play to a headset from the
> Editor — that is Windows-only and is not taught here — so your device workflow is *build an
> `.apk` and deploy it*, which is what both activities describe for everyone.

> **Headset badges.** Every XR activity is marked **simulator-friendly** (finish it at your
> desk), **headset required**, or **Quest 3 / 3S only**. Check the badge before you book
> equipment.

> **Your welfare comes first.** Activity 2 puts you in a headset and asks you to judge how the
> scene feels. If you start to feel unwell, take the headset off and stop. Feeling sick is data,
> not failure, and nobody is marked down for recording it.

## Learning Progression

1. **The rig** - What an `XR Origin (XR Rig)` is made of, and which part of it you are allowed
   to move
2. **Real-world scale** - Placing objects at measured sizes, because the headset reports real
   distances whether you meant it to or not
3. **One Interactable** - A Collider, a Rigidbody and an `XRGrabInteractable`, understood
   completely rather than five of them copied
4. **The camera as an output** - Why moving the camera makes people ill, and what to move instead
5. **The two workflows** - The XR Interaction Simulator for speed, the headset for truth, and
   the gap between them written down. You test in the simulator before you leave Activity 1

## Activities

Work through these in order — Activity 2 takes the scene Activity 1 leaves you with straight to
a headset.

- **[Activity 1](Activity%201%20-%20Your%20First%20XR%20Scene.md)** - Your First XR Scene
  — **simulator-friendly**
  - Starting from the **VR template**, and what it did on your behalf
  - Upgrading **XRI** to 3.6.0 and importing the **XR Interaction Simulator** sample
  - The **`XR Origin (XR Rig)`** Starter Assets prefab, with **Tracking Origin Mode** set to
    **Floor**
  - A floor, a table and a doorway placed at deliberate real-world sizes
  - One object with a Collider, a Rigidbody and an `XRGrabInteractable`
  - **The camera's position is an output, not an input** — move the XR Origin, never the camera
  - **Testing what you built**, in the **XR Interaction Simulator**, before you call it done

- **[Activity 2](Activity%202%20-%20The%20Two%20Workflows.md)** - The Two Workflows
  — **headset required**
  - Committing to a prediction before you build, so you can be wrong about something specific
  - A device build to a Meta Quest 2 / 3 / 3S, on Windows or macOS alike
  - The four things the simulator is silent about: scale, comfort, reach and framerate
  - Fixing the worst of them and building again, because a fix is a guess until you stand
    next to it

## Outcome

By the end of Week 5 you have an XR scene of your own — correctly floored, honestly scaled, with
one object you can pick up and throw — that you have seen through both workflows in a single
session, and improved because of what the second one told you.

You also have the two habits the rest of the course assumes. **Move the rig, never the camera.**
And **iterate in the XR Interaction Simulator, but finish on the headset**, because work that
has only ever run on a monitor is not finished work.

> **Headset checkpoint for the week.** Your scene launched standalone on a Meta Quest 2 / 3 / 3S,
> the floor was under your feet, you grabbed and threw the object with a real controller, and you
> changed something in the scene because of how it felt to stand in it. If any of that is not
> true, say so this week — from Week 6 onward every activity assumes this loop works.
