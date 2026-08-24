# XR Interaction Toolkit — Core Concepts (Unity 6.3 LTS, `6000.3.x`)

This document explains the **XR Interaction Toolkit** — **XRI** from here on — which is the
interaction layer this course is built on. It is a *concept* reference, not a walkthrough. The
activities in Weeks 5, 6 and 7 do the clicking; they point back here whenever a term needs
explaining.

Read it before you start the Week 6 activities, and keep it open beside you while you work.

> **Why we teach this rather than a vendor SDK.** A drag-and-drop prefab from a headset
> manufacturer's own SDK gets you a working grab in about ninety seconds, and teaches you nothing
> about how the grab works. XRI asks you to assemble the pieces yourself, so **every component in
> your scene is one you chose and can explain**. That is the point of the exercise, and it is also
> why the vocabulary below matters — you will be reading Inspector fields, not following pictures.
> The full argument is in [Software and Frameworks](Software_and_Frameworks.md).

---

## Versions this document describes

| Package | Version | Where it comes from |
|---|---|---|
| Unity Editor | Unity 6.3 LTS (`6000.3.x`) | Unity Hub |
| XR Interaction Toolkit (`com.unity.xr.interaction.toolkit`) | **3.6.0** | Upgraded in Week 5 Activity 1 |
| XR Core Utilities (`com.unity.xr.core-utils`) | 2.6.x | Editor |
| XR Plugin Management (`com.unity.xr.management`) | 4.5.x | VR template |
| Unity OpenXR Plugin (`com.unity.xr.openxr`) | 1.16.x | VR template |
| XR Hands (`com.unity.xr.hands`) | 1.7.x | VR template |
| AR Foundation (`com.unity.xr.arfoundation`) | 6.4.x | VR template |
| Unity OpenXR: Meta (`com.unity.xr.meta-openxr`) | 2.5.x | VR template |
| XR Composition Layers (`com.unity.xr.compositionlayers`) | 2.5.x | Editor |

> **Three XRI version numbers, and why the course picks the third.** Package Manager marks
> **3.3.2** as *Recommended*, because that is the version bundled with Unity 6.3. The VR template
> installs **3.4.1**. This course uses **3.6.0**. Every component, Inspector label, enum value
> and method signature in this document is identical across all three — the difference is a run
> of XR Interaction Simulator fixes, and the simulator is where you will spend most of the
> course. Week 5 Activity 1 does the upgrade.

> **Sample folders are named after the version.** After the upgrade, paths read
> `.../XR Interaction Toolkit/3.6.0/...`, and the old `3.4.1` folder must be deleted — two copies
> of Starter Assets declare the same assembly name and the project will not compile.

Everything here assumes **XRI 3.x**. XRI 2.x material is still the majority of what a search
engine will show you, and a surprising amount of it no longer compiles — see
[the namespace note](#the-xri-3x-namespace-trap) below.

---

## Key Concept: Interactors, Interactables, Transformers

This is the whole model in three words. Learn these three and most of XRI stops being mysterious.

* **Interactor** → lives on the hand or controller. It is the thing that *reaches out*: a ray
  pointer, a poking fingertip, a grabbing hand, a socket waiting to be filled.
* **Interactable** → lives on the object in the world. It is the thing that *can be reached*: a
  cube you can pick up, a pressable button, a lever.
* **Grab Transformer** → decides *how the object behaves while it is held* — does it follow the
  hand exactly, rotate about its own centre, scale when a second hand joins in.

An interaction is always a pair: one Interactor selecting or hovering one Interactable. Nothing
happens because an object *could* be picked up; it happens because an Interactor selected it. When
something in your scene refuses to work, your first question is always *which half is missing* —
the Interactor, the Interactable, or the thing that registers them to each other, which is next.

---

## The XR Interaction Manager

Every Interactor and every Interactable in the scene registers itself with an
**`XRInteractionManager`**. It is the switchboard: each frame it works out which Interactors are
hovering or selecting which Interactables, and it raises the events both sides listen for.

Three things worth knowing:

* **You do not have to add one.** By default XRI creates one at runtime the first time an
  Interactor or Interactable goes looking for it, and marks it `DontDestroyOnLoad`. That is why
  it appears in the Hierarchy only once you press Play, under **DontDestroyOnLoad** rather than
  in your scene. A scene with *two* managers can behave strangely: an Interactor registered with
  manager A can never select an Interactable registered with manager B.
* **It is not the rig.** The manager is a plain GameObject with one component. The rig is
  separate — see below.
* **It owns the matching rules**, including interaction layers, which is why layers are configured
  on the Interactor and the Interactable rather than on the manager itself.

> **Checkpoint.** If grabbing does nothing at all and there are no console errors, enter Play
> mode and check the manager count before you check anything else.

---

## The rig

The player's head and hands are an **XR Origin**: a transform holding the tracked Camera (the
headset) and the controller transforms that your Interactors hang off. It converts tracked device
positions into world positions, including the floor offset, so a seated and a standing player are
both the right height.

There are two ways to get one, and they are not equivalent:

| Name | What it is | What you get |
|---|---|---|
| **`XR Origin (XR Rig)`** | A ready-made *prefab* in the XRI Starter Assets sample | The origin, camera offset and camera, **plus** both controllers with their Interactors, an **Input Action Manager** holding `XRI Default Input Actions`, an `XRInputModalityManager`, and the locomotion providers |
| **XR Origin (VR)** | A *menu item* under **GameObject → XR** | The origin, a camera offset, and a camera. An **Input Action Manager** is added and pointed at `XRI Default Input Actions` if that asset is in the project. **No controllers and no Interactors** |

**This course uses the prefab.** The menu item is the right tool when you want to assemble a rig
from scratch and see every piece, but a rig built that way cannot grab anything until you add
controller transforms, tracked pose drivers, Interactors and input bindings yourself.

When an activity names one, it means that one. Using both leaves you with two rigs and two audio
listeners.

The prefab's hierarchy, which is worth being able to read from memory:

```
XR Origin (XR Rig)
└── Camera Offset
    ├── Main Camera
    ├── Left Controller
    │   ├── Near-Far Interactor
    │   ├── Poke Interactor
    │   └── Teleport Interactor
    └── Right Controller
        └── ... the same again
```

---

## Interactors — the reaching half

All of these live on the controller or hand side of the rig. A single controller commonly carries
several at once, each responsible for a different kind of reach.

Every Interactor that responds to a button reads it through an **input reader** rather than
polling hardware. On the Starter Assets prefabs those readers are already set to **Input Action
Reference** and pointed at actions such as `XRI Left Interaction/Select`. A component you add
yourself with **Add Component** arrives with its readers empty, and will do nothing at all until
you assign them.

### Near-far interactor

**`NearFarInteractor`** is the modern default in XRI 3.x, and the one the Starter Assets rig uses.
It merges two behaviours that used to need two separate components: when your hand is close to an
object it grabs directly, and when it is not, it casts a ray to reach further. The handover
between near and far is handled for you, so the player never thinks about which mode they are in.

* Available from XRI 3.2 onward.
* Works with UI Toolkit, so the same interactor drives world-space menus.

### Ray interactor

**`XRRayInteractor`** is still present and still supported. It does one thing — cast a ray, hover
and select whatever it hits. Reach for it when you want an explicit laser pointer and nothing
else, or when you are following older material. For a general-purpose hand, prefer
`NearFarInteractor`.

### Poke interactor

**`XRPokeInteractor`** treats a point — usually a fingertip or the controller's tip — as a physical
prod. It is the natural fit for buttons, keypads and any panel the player should press rather than
point at, and it is what makes hand tracking feel convincing. It is also UI Toolkit compatible.
The Starter Assets rig already carries one on each hand.

### Direct interactor

**`XRDirectInteractor`** selects whatever is overlapping its trigger collider — no ray, no
distance, touch only. `NearFarInteractor` covers this case for most scenes, but the direct
interactor is still the clearest thing to reason about when you are debugging a grab.

### Socket interactor

**`XRSocketInteractor`** is an Interactor that does not move and does not belong to a hand. It sits
in the world and *accepts* Interactables — a holster, a slot in a puzzle board, a stand a trophy
snaps onto. Combined with interaction layers it is how you build "this piece only fits here".

> **Known quirk, and where the setting lives.** With **Attach Ease In Time** set to `0`, an object
> transferring between a socket and a hand can show a one-frame visual skip as it teleports to the
> new attach point. A small non-zero ease-in time hides it. The field is on the
> **`XRGrabInteractable`**, not on the socket.

### Gaze interactor

**`XRGazeInteractor`** lets the player select by looking, which matters for accessibility and for
headsets with eye tracking. On Meta Quest, eye-gaze input needs the **Eye Gaze Interaction**
OpenXR feature enabled in **XR Plug-in Management → OpenXR**, and it is not enabled by default.

---

## Interactables — the reachable half

### Grab interactable

**`XRGrabInteractable`** is the one you will use most. Put it on an object with a `Collider` and
it can be picked up, held, and thrown. It declares `[RequireComponent(typeof(Rigidbody))]`, so
Unity adds the Rigidbody for you.

What it actually does is narrower than it first appears, and worth stating precisely: each frame
it is held, `XRGrabInteractable` **applies a position, rotation and scale that were computed for
it by one or more `IXRGrabTransformer`**. The Interactable manages the selection; the transformer
decides the movement. That separation is the reason two-handed scaling can be added to an object
without touching the grab logic at all.

Fields you will meet early:

* **Movement Type** — `Velocity Tracking`, `Kinematic`, or `Instantaneous`. This is the single
  biggest lever on how "physical" a held object feels, and how badly it can shove other objects
  around.
* **Attach Transform** — the point on the object that lines up with the hand. Leave it empty and
  the object grabs about its own origin, which is why a sword picked up by the blade is usually an
  attach transform problem, not a bug.
* **Throw On Detach** — carries the hand's motion into the Rigidbody's `linearVelocity` when you
  let go. Without it, released objects drop straight down. It has no effect on a kinematic
  Rigidbody. **Throw Velocity Scale** and **Throw Smoothing Duration** beside it tune the feel.
* **Select Mode** — `Single` or `Multiple`. Two hands cannot hold the same object until this is
  `Multiple`.
* **Add Default Grab Transformers** — on by default. Untick it when you supply your own.

### Simple interactable

**`XRSimpleInteractable`** can be hovered and selected but is not moved by the interaction. It is
the right component for a button, a lever handle, or anything where you only want the *events* —
you wire up **Select Entered**, **Select Exited** and the hover equivalents yourself under
**Interactable Events**, and decide what happens.

### Snap volumes and climbing

Two specialised Interactables you will see referenced in Unity's own samples:
**`XRInteractableSnapVolume`**, which makes a ray "stick" to a nearby Interactable so small targets
are easier to hit, and **`ClimbInteractable`**, which turns a piece of geometry into something the
player can pull themselves along. The Starter Assets sample ships `Climbing Wall`, `Ladder` and
`Climb Sample` prefabs that use the second.

---

## Grab Transformers — how a held object behaves

This is where XRI does something worth understanding, and where Week 6 spends most of its time.

**`XRGeneralGrabTransformer`** is the default. It extends **`XRBaseGrabTransformer`** and implements
**`IXRGrabTransformer`**, and it handles the common cases out of the box:

* **One hand** — the object follows the hand's position and rotation.
* **Two hands** — **Two Handed Rotation Mode** offers `First Hand Only`,
  `First Hand Directed Towards Second Hand` (the default) and `Two Handed Average`.
* **Scaling** — off until you tick **Allow Two Handed Scaling**, then bounded by
  **Minimum Scale Ratio** and **Maximum Scale Ratio** while **Clamp Scaling** is on, so an object
  can never be shrunk to nothing or blown up past what your scene can cope with. One-handed
  scaling is driven by a scale value provider (a thumbstick axis, typically) rather than by hand
  separation.
* **Position constraints** — **Permitted Displacement Axes** restricts movement to any combination
  of X, Y and Z, measured according to **Constrained Axis Displacement Mode**.

Scaling is **uniform**. There is no per-axis scale constraint on this component, which is exactly
the gap Week 6's `NonUniformGrabFreeTransformer` exists to fill.

**Unity adds a default transformer set to `XRGrabInteractable` automatically.** You do not have to
add one to get a working grab. Because the default *is* an `XRGeneralGrabTransformer`, adding that
component yourself does not produce a second one — the Interactable uses the component already
there. Add a transformer of a *different* type and you have two, which is why Week 6 Activity 3
starts by unticking **Add Default Grab Transformers**.

### Writing your own

A custom transformer implements `IXRGrabTransformer`, usually by inheriting
`XRBaseGrabTransformer`. `Process` is `abstract` and you must implement it; `OnLink`, `OnGrab`,
`OnGrabCountChanged`, `OnUnlink` and `registrationMode` are `virtual` and you override what you
need.

A transformer sitting on the same GameObject as the `XRGrabInteractable` **registers itself** at
`Start`, using `registrationMode` to choose which list it joins: `Single`, `Multiple`,
`SingleAndMultiple` or `None`. The **Starting Single Grab Transformers** and **Starting Multiple
Grab Transformers** lists on the Interactable are for transformers that live on other objects.

The hook that matters for two-handed work is **`OnGrabCountChanged`**, called when the number of
Interactors selecting the object changes while it is held. This is where you notice "a second hand
just joined" or "one hand let go", and switch behaviour accordingly. If you are trying to detect
two-handed grabs by counting in `Update`, you are doing it the hard way.

The reason to write your own is control. A transformer that constrains a drawer to one axis, or
that scales only in Y, is a dozen lines — and it makes the Interactor / Interactable / Grab
Transformer split concrete in a way that reading about it never will.

---

## Interaction layers

Interaction layers answer the question *"is this Interactor allowed to touch this Interactable?"*
They are XRI's own filtering system and are **completely separate from Unity's physics layers** —
setting one does not affect the other, and confusing the two is a common way to lose an hour.

The concept:

* Both an Interactor and an Interactable carry a set of interaction layers.
* An interaction is only permitted if the two sets **overlap**. No overlap, no hover, no select —
  and no error message either, which is what makes this worth knowing about in advance.
* You define your own named layers for the project, then assign them per component.

Typical uses: a socket that only accepts blue keys; a teleport ray that must not pick up props; a
UI pointer that ignores the physics props entirely.

The named layers are edited in **Edit → Project Settings → XR Plug-in Management → XR Interaction
Toolkit → Interaction Layer Settings**. Each Interactor and Interactable then exposes an
**Interaction Layer Mask** field in the Inspector.

---

## The XRI 3.x namespace trap

XRI 2.x put nearly everything in one namespace. **XRI 3.x splits it up.** Any script you copy from
an older tutorial, forum post or video will therefore fail on its very first `using` line, with a
"type or namespace could not be found" error pointing at code that looks perfectly correct. It is a
baffling first error, and it is almost always this.

```csharp
using UnityEngine.XR.Interaction.Toolkit;                    // XRInteractionManager,
                                                             // XRInteractionUpdateOrder
using UnityEngine.XR.Interaction.Toolkit.Interactables;      // XRGrabInteractable,
                                                             // XRSimpleInteractable
using UnityEngine.XR.Interaction.Toolkit.Interactors;        // NearFarInteractor,
                                                             // XRPokeInteractor
using UnityEngine.XR.Interaction.Toolkit.Transformers;       // IXRGrabTransformer,
                                                             // XRBaseGrabTransformer,
                                                             // XRGeneralGrabTransformer
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;     // HapticImpulsePlayer
using UnityEngine.XR.Interaction.Toolkit.Feedback;           // SimpleHapticFeedback
```

> **If a sample will not compile, read the `using` lines first.** Do not start renaming components.
> The fastest fix is to delete the old `using`, type the class name, and let your editor offer the
> correct namespace.

A related note for Unity 6.3 LTS (`6000.3.x`) generally: the Rigidbody velocity property is now
**`linearVelocity`**, and the old scene-wide object lookups are replaced by
**`FindObjectsByType<T>(FindObjectsSortMode.None)`**. Old XR samples trip over both.

---

## Starter Assets

XRI ships a **Starter Assets** sample containing the input bindings and prefabs the activities
assume. It installs to:

```
Assets/Samples/XR Interaction Toolkit/3.6.0/Starter Assets
```

Inside, under `Prefabs/`, you get the **`XR Origin (XR Rig)`** rig prefab, a `Permissions Manager`,
and folders of `Controllers`, `Interactors`, `Affordances` and `Teleport` prefabs. Alongside them
is **`XRI Default Input Actions.inputactions`** — the action asset that maps controller buttons,
thumbsticks and triggers onto named actions.

Its action maps are worth knowing by name, because Inspector fields ask for them:

| Map | Contains |
|---|---|
| `XRI Left` / `XRI Right` | Tracking: `Position`, `Rotation`, `Is Tracked`, `Haptic Device`, `Poke Position`, `Grip Position` |
| `XRI Left Interaction` / `XRI Right Interaction` | `Select`, `Select Value`, `Activate`, `UI Press` |
| `XRI Left Locomotion` / `XRI Right Locomotion` | `Move`, `Turn`, `Snap Turn`, `Teleport Mode`, `Grab Move`, `Jump` |
| `XRI Head` | Head and eye-gaze pose |
| `XRI UI` | `Navigate`, `Submit`, `Cancel`, `Click` |

**The grab action is called `Select`.** You read a named action rather than polling a specific
button, which is the same idea you met in
[Week 2 Activity 3](../Week%2002/Activity%203%20-%20Input%20Movement.md), and it is why the same
script works across controller types and across headsets.

> **Starter Assets requires Shader Graph.** If the sample imports with pink materials or console
> errors about missing shaders, that is why. **Project Validation** detects it and offers a fix.

> **Samples do not upgrade with their package.** After changing the XRI version, reimport the
> samples so the prefabs match. The folder is named after the version, so an old one left behind
> is visible in the Project window.

---

## Testing without a headset: the XR Interaction Simulator

The **XR Interaction Simulator** drives a virtual headset and virtual controllers from your
keyboard and mouse, so you can enter Play mode and test interactions at your desk. It works
identically on Windows and macOS, which matters on this course because Mac users have no
Editor-to-headset play mode at all.

Import it as a sample from the Package Manager, under XR Interaction Toolkit → Samples → **XR
Interaction Simulator**. The VR template does not import it for you.

Then switch it on in **Edit → Project Settings → XR Plug-in Management → XR Interaction Toolkit**:

| Setting | What it does |
|---|---|
| **Use XR Interaction Simulator in scenes** | Creates the simulator on entering Play mode, so nothing has to go into your scene |
| **Instantiate In Editor Only** | On by default. Keeps the simulator out of a device build |
| **Use Classic XR Device Simulator** | Switches to the pre-3.1 implementation. You do not want it |

You can drag `XR Interaction Simulator.prefab` into a scene by hand instead, in which case
disabling it before a device build is yours to remember.

XRI 3.1 replaced the older simulator with this one, and it is what the course uses everywhere.
Material written against the classic simulator will not match the activities.

> **The XR Interaction Simulator is fast, not truthful.** It will not show you a comfort problem,
> a frame-rate problem, or a reach problem. Build to the headset before you decide something works.

---

## Where the rest of it lives

XRI does not cover everything, and knowing which package owns what saves a lot of searching.

| You want | It comes from |
|---|---|
| Grabbing, poking, rays, sockets, UI interaction | XRI |
| Teleport areas and anchors, smooth movement, turning | XRI locomotion — `TeleportationProvider` with `Teleport Area` / `Teleport Anchor`, `DynamicMoveProvider`, `SnapTurnProvider`, `ContinuousTurnProvider`, coordinated by a `LocomotionMediator` and an `XRBodyTransformer` |
| Comfort vignette during smooth movement | XRI — `TunnelingVignetteController`, with a `TunnelingVignette` prefab in Starter Assets |
| Climbing and jumping | XRI — `ClimbProvider`, `ClimbInteractable`, `JumpProvider` |
| Hand tracking joints and gestures | XR Hands — `XRHandShape`, `XRHandPose`, `StaticHandGesture` |
| Switching between hands and controllers automatically | XRI — an `XRInputModalityManager` on the rig |
| Controller vibration | XRI — `HapticImpulsePlayer`, with `SimpleHapticFeedback` holding a reference to it and firing on hover/select |
| Passthrough, planes and anchors | AR Foundation + Unity OpenXR: Meta |
| Sharp world-locked text and video quads | XR Composition Layers |
| Voice commands | Nothing in this stack — not covered on this course |

---

## Example: what a working grab is actually made of

It is worth spelling out once, because a scene where grabbing "just works" hides all of it.

### A) On the rig — the Interactor side

* An **XR Origin** with a tracked Camera and two controller transforms.
* An **Input Action Manager** holding `XRI Default Input Actions`, so the actions are enabled at
  runtime. Without it everything tracks and no button works.
* Input read through **action references**, so *Select* is a named action rather than a specific
  button.
* A **`NearFarInteractor`** on each controller, giving close-range grabbing and a ray for distance
  from the one component.
* Optionally an **`XRPokeInteractor`** for pressable UI, which the Starter Assets rig already has.
* Optionally a **`HapticImpulsePlayer`**, referenced by a **`SimpleHapticFeedback`** component so
  the controller gives a small pulse on hover and select.

### B) On the object — the Interactable side

* A **`Collider`**, so it can be hit or overlapped at all.
* A **`Rigidbody`**, so it can be moved and thrown by physics.
* An **`XRGrabInteractable`**, with an Attach Transform if the default grab point is wrong.
* A **Grab Transformer** — `XRGeneralGrabTransformer` by default, added for you unless you turn
  that off; or your own `IXRGrabTransformer` when you want to constrain or extend the behaviour.

### And in between

An **`XRInteractionManager`** that both halves registered with, and **interaction layers** that
agree. Take away any one of these six things and the grab fails — usually quietly. Knowing the
list *is* the debugging procedure.

---

## FAQ

**Q: Do I have to use the Starter Assets, or can I build the rig myself?**
A: You can build it yourself, and doing so once shows you every piece. The activities assume the
Starter Assets rig and action asset exist, though, because otherwise every activity would begin
with the same twenty minutes of binding buttons.

**Q: I used GameObject → XR → XR Origin (VR) and I have no hands. Is it broken?**
A: No. That menu item creates the origin, the camera offset and the camera, and stops there.
Controllers and Interactors are yours to add. Use the `XR Origin (XR Rig)` prefab instead.

**Q: I found a tutorial that starts by opening a headset vendor's tool window and dragging in a
prefab. Can I follow it?**
A: No — it is a different stack, not a worse one. Its prefab names, its component names and its
project settings do not exist in ours, and installing both stacks side by side makes them fight
over the same settings. See [Software and Frameworks](Software_and_Frameworks.md).

**Q: `NearFarInteractor` or `XRRayInteractor`?**
A: `NearFarInteractor` for a general-purpose hand — it covers both near grabbing and far pointing.
`XRRayInteractor` when you specifically want a laser pointer and nothing else.

**Q: I added an Interactor with Add Component and it does nothing.**
A: Its input readers are empty. Set **Select Input → Input Source Mode** to **Input Action
Reference** and assign `XRI Left Interaction/Select` or its right-hand equivalent.

**Q: Hands or controllers?**
A: Support both where you can. Controllers give you buttons and haptics; hand tracking feels more
natural and needs no hardware in the player's grip. Design so the player chooses.

**Q: My script will not compile and the error is on a `using` line.**
A: XRI 3.x namespaces. Read [the namespace note](#the-xri-3x-namespace-trap) above.

**Q: Nothing happens when I try to grab, and there are no errors.**
A: Work down the six items in the example above, in order. Most often it is a missing
`Collider`, or interaction layers that do not overlap.

**Q: Do I need a headset to develop?**
A: Not for most of the work — the XR Interaction Simulator covers iteration. You do need the
headset to judge whether anything is actually any good, and to catch comfort and performance
problems the XR Interaction Simulator cannot show you.

**Q: OpenXR, or a vendor-specific plugin?**
A: **Unity OpenXR Plugin**, with the Meta Quest Support feature group for Android builds. That is
the supported path on Meta Quest 2 / 3 / 3S and the one every course activity assumes.

---

## References

* XR Interaction Toolkit manual: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6>
* XRI Starter Assets: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/samples-starter-assets.html>
* XR Interaction Simulator: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/xr-interaction-simulator-overview.html>
* XR Core Utilities (`XROrigin`): <https://docs.unity3d.com/Packages/com.unity.xr.core-utils@2.6>
* XR Hands manual: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.7>
* Unity OpenXR Plugin manual: <https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.16>
* Unity OpenXR: Meta manual: <https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.5>
* AR Foundation manual: <https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.4>
* Unity Manual: <https://docs.unity3d.com/6000.3/Documentation/Manual/index.html>
* Course stack reference: [Software and Frameworks](Software_and_Frameworks.md)
