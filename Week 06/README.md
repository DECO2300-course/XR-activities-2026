# Week 6: The Interaction Toolkit

Week 5 got a rig on your head and something grabbable in front of you. Week 6 is about
**interaction vocabulary** — the components that make a grab happen, what each one is
responsible for, and how to read a component graph you did not write. You will assemble a
grab Interactable from its parts instead of dropping in a prefab, resize an object with two
hands, read somebody else's Grab Transformer and say what it does, and then write one of
your own.

## Before You Start

- **Unity 6.3 LTS (`6000.3.x`)**, and a project made from the **VR template** in Week 5 and
  upgraded there to **XRI 3.6.0** — see
  [Software and Frameworks](../Guides/Software_and_Frameworks.md) and the
  [OpenXR setup guide](../Guides/OpenXR_Unity_Setup_Guide.md)
- **Switched to Android**, with the **Meta Quest Support** feature group ticked and
  **Project Validation** clean on the Android tab
- **Read [XRInteractionToolkit.md](../Guides/XRInteractionToolkit.md) first.** It is this week's
  vocabulary — Interactor, Interactable, Grab Transformer, the XR Interaction Manager, the
  XRI 3.x namespaces. The activities use those terms and deliberately do not re-explain
  them. Keep it open beside you
- **Packages you'll add this week:** none. Everything runs on **XR Interaction Toolkit**
  3.6.x with its **Starter Assets** and **XR Interaction Simulator** samples, all of which
  arrived in Week 5. If the samples are missing, import them from **Window → Package
  Manager → XR Interaction Toolkit → Samples**
- Commit at the start and the end of each activity, and not in between.
  `Packages/manifest.json` and `ProjectSettings/` belong in the repository with your scene

> **Keyboard shortcuts.** Where these activities say `Ctrl`, macOS users press `Cmd` —
> `Ctrl+S` becomes `Cmd+S`, `Ctrl+D` becomes `Cmd+D`.

> **macOS has no Quest Link.** Mac users cannot play to the headset from the Editor. That
> is why this week is built around the **XR Interaction Simulator** — but the headset
> checkpoints still mean building an `.apk` and deploying it.

## Learning Progression

1. **Interactables from parts** — Collider, Rigidbody, `XRGrabInteractable`, and the three
   fields that decide how a held object feels
2. **Interactors compared** — near-far and ray reaching versus poking, and why one hand
   carries several
3. **Grab Transformers used** — two-handed scaling, bounded and constrained, from the
   Inspector, and where the Inspector runs out
4. **Grab Transformers read** — working out what a transformer somebody else wrote does
5. **Grab Transformers written** — implementing `IXRGrabTransformer` yourself

## Activities

Work through these in order — each one builds on the scene from the last.

- **[Activity 1](Activity%201%20-%20Grab%20Interactables%20and%20Interactors.md)** - Grab Interactables and Interactors *(simulator-friendly)*
  - Building a grab Interactable from Collider, Rigidbody and `XRGrabInteractable`
  - Attach transforms — where the hand actually holds the object, and why local `-0.5` is
    the bottom face whatever you scale the object to
  - Movement types — velocity tracking, kinematic, instantaneous, and what each does to
    the rest of your scene
  - Throw on detach, and the `linearVelocity` a throw actually is
  - Comparing Interactors: `NearFarInteractor` and `XRRayInteractor` against
    `XRPokeInteractor`, and what an Interactor with no input assigned does

- **[Activity 2](Activity%202%20-%20Two-Handed%20Scaling.md)** - Two-Handed Scaling *(simulator-friendly)*
  - `XRGeneralGrabTransformer` and where the default grab behaviour actually lives
  - Two-handed scaling bounded by **Minimum Scale Ratio** and **Maximum Scale Ratio**
  - The three two-handed rotation modes, and per-axis *position* constraints
  - Finding the limit: this component scales uniformly and cannot scale per axis
  - Reading `NonUniformGrabFreeTransformer.cs` and saying what it does differently
  - `OnGrabCountChanged` as the two-hand hook

- **[Activity 3](Activity%203%20-%20Writing%20Your%20Own%20Grab%20Transformer.md)** - Writing Your Own Grab Transformer *(simulator-friendly)*
  - Switching off the automatic default transformer set, and why it has to be off
  - Inheriting `XRBaseGrabTransformer` and implementing `IXRGrabTransformer`
  - `Process`, and what the `ref Pose` and `ref Vector3` parameters mean
  - Following the hand from the Interactor's attach transform
  - Rainbow hue cycling driven by vertical movement, in HSV

## C# Scripts

Reference copies of the scripts used across these activities are in the `Scripts/`
directory. Write your own first — these are for checking against, not for pasting.

- **[CustomTransformer.cs](Scripts/CustomTransformer.cs)** - One-handed grab transformer
  that follows the hand and cycles material hue on vertical movement (Activity 3)
- **[NonUniformGrabFreeTransformer.cs](Scripts/NonUniformGrabFreeTransformer.cs)** -
  Two-handed grab transformer with per-axis scaling and bounded ratios (Activity 2)
- **[MaterialSwapper.cs](Scripts/MaterialSwapper.cs)** - Swaps between two materials from
  Interactable events; used for the poke button in Activity 1 and as an extension in
  Activity 3

> **All three are XRI 3.x.** If your editor cannot resolve a type, read the `using` lines
> before you rename anything — see
> [the namespace trap](../Guides/XRInteractionToolkit.md#the-xri-3x-namespace-trap).

## Outcome

By the end of Week 6 you will have a scene in which you built every interaction from its
components and can explain what each one does: an object you can pick up by a chosen grip
point, throw with a velocity you tuned, press without pointing at, resize with two hands
within bounds you set — and one you wrote the movement code for yourself. More usefully,
when an XR scene you did not build refuses to work, you will know the six things to check
and the order to check them in.
