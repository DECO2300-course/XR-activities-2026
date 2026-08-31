# Activity 2: Two-Handed Scaling

> **Headset badge: simulator-friendly.** Two-handed grabbing works in the **XR Interaction
> Simulator** — you drive one hand at a time and the grabs persist, so you can hold with
> both. It is fiddly, and it is still the fastest way to build this. There is a headset
> checkpoint at the end.

## Objective
Add **`XRGeneralGrabTransformer`** to a grab Interactable and use it to scale an object
with two hands, bounded by a minimum and maximum ratio. Find the limit of what it can
constrain, then read a transformer somebody else wrote that goes past that limit.

## Prerequisites
- **Activity 1** complete — you need the scene, the table, and a working `Prop`
- **[XR Interaction Toolkit — Core Concepts](../Guides/XRInteractionToolkit.md#grab-transformers--how-a-held-object-behaves)**,
  the *Grab Transformers* section in particular
- **Packages you'll add this week:** none
- Hardware: none required to build this. A **Meta Quest 2 / 3 / 3S** for the closing
  checkpoint

> **Keyboard shortcuts.** Where this activity says `Ctrl`, macOS users press `Cmd`.

## Instructions

### Step 1: Let the object be held by two hands

1. Open `Week06_Interactions` from Activity 1.
2. Duplicate `Prop` (`Ctrl+D`), name it `ScaleProp`, and move it to `(0, 0.825, 0.9)` — the
   back edge of the table, clear of the button and the stack, with room either side.
3. Select `ScaleProp` and set **Select Mode** on the **XR Grab Interactable** from
   **Single** to **Multiple**.
4. Press **Play**, grab `ScaleProp` with one hand, then take control of the other hand and
   grab the same object. Both hands should hold it.

> **Only one hand can hold it?** That is **Select Mode**. On **Single**, the second grab
> steals the object from the first hand rather than joining it.

### Step 2: Add the general grab transformer

Adding `XRGrabInteractable` in Activity 1 gave you a working grab with no transformer of
your own, because **Unity adds a default transformer set automatically**. You are now
going to add one explicitly so you can see and change its settings.

1. With `ScaleProp` selected, **Add Component → XR General Grab Transformer**.
2. Look at what you just added. `XRGeneralGrabTransformer` extends
   **`XRBaseGrabTransformer`** and implements **`IXRGrabTransformer`** — it is the kind of
   component you will write yourself in Activity 3, and it is the default behaviour made
   visible.
3. Play and grab with both hands. Move them apart and together. Nothing scales yet —
   scaling is off until you turn it on.

> **You have not created a second transformer.** The default set *is* an
> `XRGeneralGrabTransformer`, and when the Interactable goes looking for one it takes the
> component already on the object rather than adding another. That is only true because
> the default and the one you added are the same type. In Activity 3 they will not be, and
> that is where it matters.

### Step 3: Turn on scaling, and bound it

1. On the **XR General Grab Transformer**, tick **Allow Two Handed Scaling**.
2. Leave **Clamp Scaling** ticked. It is the switch that makes the two ratio fields below
   it apply at all.
3. Find **Minimum Scale Ratio** and **Maximum Scale Ratio**. These are *ratios*, not
   absolute sizes: a minimum of `0.25` means "never smaller than a quarter of the size
   this object was", whatever that size happened to be. They default to `0.25` and `2`.
4. Set them to `0.25` and `4`.
5. Play. Grab with both hands and pull them apart — the prop grows. Push them together —
   it shrinks. Keep pulling and it stops at four times its original size.

Bounds are not decoration. An unbounded scale lets a player shrink an object below the
resolution of their own tracking, at which point they can never grab it again to undo it;
or blow it up until it encloses the camera and the scene turns into an inside-out box. Set
both bounds on every scalable object, every time.

> **The object also scales with one hand.** **Allow One Handed Scaling** is on by default,
> and it is driven by a scale value provider — a thumbstick axis, on this rig — rather than
> by hand separation. Untick it if the one-handed behaviour is getting in the way of what
> you are trying to observe.

> **Checkpoint.** The prop grows and shrinks between two hands and refuses to go past
> either bound.

### Step 4: Two-handed rotation modes

While an object is held by two hands there is more than one sensible answer to "which way
should it point", and `XRGeneralGrabTransformer` lets you choose. Set **Two Handed
Rotation Mode** to each value in turn, playing after each:

| Value | What it does | Suits |
|---|---|---|
| **First Hand Only** | The object takes its rotation from the first hand that grabbed it. The second hand contributes scale and nothing else | A panel you are resizing but do not want to tilt |
| **First Hand Directed Towards Second Hand** | The object aims along the line between your hands, like holding a rifle. Rotating the first hand about that line twists it. This is the default | A two-handed sword, a bat, anything with a long axis |
| **Two Handed Average** | Also aims along the line between your hands, but takes its base roll from the average of both hands rather than from the first | A steering wheel, or anything where both hands should have equal say |

Pick the one that suits the object, and note that there is no "do not rotate at all" value
here. If you need that, you write it — which is Activity 3.

### Step 5: Find the limit of what it can constrain

Not everything should move in every direction. A drawer slides on one axis; a sliding door
moves in one plane.

1. Find **Permitted Displacement Axes** on the **XR General Grab Transformer**. It is a
   flags field with **X**, **Y** and **Z**. Untick **Y** and play: the prop can now be
   pulled around the table but not lifted off it.
2. **Constrained Axis Displacement Mode** decides what those axes are measured against —
   the object's own rotation, or the object's rotation with world up locked. Try both with
   the prop rotated 45°.
3. Now look for the same thing for scale. **There is no per-axis scale constraint on this
   component.** `XRGeneralGrabTransformer` scales uniformly: every axis by the same ratio,
   or not at all.

That gap is the point of the next step. Position can be constrained per axis from the
Inspector; scale cannot, so a banner that should stretch wide without getting taller is
not something this component can give you.

### Step 6: Read a transformer you didn't write

Open **[NonUniformGrabFreeTransformer.cs](Scripts/NonUniformGrabFreeTransformer.cs)** in
the `Scripts/` folder. Read it before you use it — that is the exercise. It is about a
hundred lines and it does the one thing `XRGeneralGrabTransformer` does not: it scales each
axis by its own ratio, so pulling your hands apart along the object's own X stretches it
in X alone.

Work out the answers to these from the file itself:

- Which method captures the "before" measurements, and **why that method and not
  `OnGrab`**?
- What does `GetLocalSpan` return, and why does it multiply by
  `Quaternion.Inverse(rotation)`?
- Where do `m_MinimumScaleRatio` and `m_MaximumScaleRatio` get applied, and what happens
  when an axis is switched off?

The measurement that makes the whole thing work is this one:

```csharp
    /// <summary>
    /// The vector between the two hands, measured along the object's own axes and made
    /// positive. Working in the object's axes is what makes the scaling per-axis: a
    /// stretch along the object's local X only ever grows X.
    /// </summary>
    static Vector3 GetLocalSpan(XRGrabInteractable grabInteractable, Quaternion rotation)
    {
        var interactors = grabInteractable.interactorsSelecting;
        if (interactors.Count < 2)
            return Vector3.one;

        var first = interactors[0].GetAttachTransform(grabInteractable).position;
        var second = interactors[1].GetAttachTransform(grabInteractable).position;
        var span = Quaternion.Inverse(rotation) * (second - first);

        return new Vector3(Mathf.Abs(span.x), Mathf.Abs(span.y), Mathf.Abs(span.z));
    }
```

The hands are in world space; the object's axes are not. Multiplying by the inverse of the
object's rotation moves the hand-to-hand vector into the object's own frame, so "apart
along X" means the object's X and not the world's.

Now use it:

1. Duplicate `ScaleProp`, name it `NonUniformProp`, and move it to `(-0.5, 0.825, 0.9)`.
2. Remove the **XR General Grab Transformer** from it and add
   **NonUniformGrabFreeTransformer** instead. You do not need to drag it anywhere: an
   `XRBaseGrabTransformer` registers itself with the `XRGrabInteractable` on the same
   GameObject when the scene starts, using its `registrationMode` to decide whether it
   handles one-hand grabs, multi-hand grabs, or both.
3. Leave all three axes enabled, set the bounds to `0.25` and `4`, and leave
   **Lock Rotation While Scaling** ticked.
4. Play. Grab with both hands and pull them apart *sideways*, then *vertically*. The prop
   stretches in the direction you pulled instead of growing uniformly.
5. Untick **Scale Y** and try again. It now refuses to get taller.

> **The name is inherited.** This file started life as a port of a vendor SDK's
> "grab free" transformer, and the name came with it. Its contents are XRI.

## Understanding the transformer split

An `XRGrabInteractable` does not decide how a held object moves. Each frame it is held, it
**applies a position, rotation and scale that one or more `IXRGrabTransformer` computed for
it**. That is the whole design, and it is why you were able to change scaling behaviour in
this activity without touching anything about grabbing.

Two consequences worth holding on to:

**Transformers stack and they specialise.** An Interactable keeps separate lists for the
one-hand case and the multi-hand case — **Starting Single Grab Transformers** and
**Starting Multiple Grab Transformers** on the Inspector.
`NonUniformGrabFreeTransformer` declares `RegistrationMode.Multiple`, so it registers into
the second list only, which is why the object still behaves normally in one hand even
though the file contains no one-handed code at all.

**`OnGrabCountChanged` is the two-hand hook.** It fires when the number of Interactors
selecting the object changes while it is held — the exact moment a second hand joins or
leaves. That is where "how far apart were the hands when this started" has to be recorded.
Counting selections yourself in `Update` is the hard way to do the same thing, and it is
always one frame late.

## Extension Activities

### **A resize handle instead of a whole-object grab**
Put small child cubes at two corners of a panel, each with its own `XRGrabInteractable`,
and drive the panel's scale from the distance between them. Logic: give each handle a
select-entered and select-exited listener, and while both are held, set the panel's
`localScale` from the handle separation. No transformer needed — this is the non-XRI way
to do it, and comparing the two is the point.

### **Bounds that tell you they were hit**
Extend `NonUniformGrabFreeTransformer` so that when an axis clamps at
`m_MinimumScaleRatio` or `m_MaximumScaleRatio`, something visible happens. Key code: in
`AxisScale`, compare the clamped ratio to the unclamped one, and raise a flag the rest of
the component can read. Then fire a haptic pulse through a `HapticImpulsePlayer`, or swap
the material with **[MaterialSwapper](Scripts/MaterialSwapper.cs)**.

### **Snap to sensible sizes**
Round the ratio to the nearest quarter before applying it, so an object can only be `0.5x`,
`0.75x`, `1x` and so on. Key code: `Mathf.Round(ratio * 4f) / 4f` inside `AxisScale`,
before the `Mathf.Clamp`. Then argue with yourself about whether snapping helps or hurts —
it depends entirely on whether the player is building something or playing with something.

### **Plane-only scaling**
Restrict scaling to the two axes most parallel to the line between the hands, so the third
axis is always held at its original size. Logic: in `Process`, find the largest two
components of the local span and treat the smallest as disabled. That is roughly a dozen
lines on top of what is already there.

## Headset checkpoint

Build to the headset (**File → Build Profiles → Build and Run**) and scale something with
your real hands.

- **Two hands is harder than it looks.** Reaching a small object with both hands, at the
  same time, without one hand stealing it, is a real design problem the XR Interaction
  Simulator hides completely.
- **Check your bounds against a body.** A maximum of `4` sounded reasonable on a monitor.
  Standing next to the result, it may be absurd — or not nearly enough.
- **Watch for the object leaving your hands.** If it drifts away from where your hands
  actually are, that is the transformer and the movement type disagreeing.

## Outcome
An object that two hands can resize, bounded so it cannot be destroyed by the player, plus
the knowledge of where the Inspector runs out — per-axis position, yes; per-axis scale,
no — and the ability to read an `IXRGrabTransformer` somebody else wrote and say what it
does.

## References
- `XRGeneralGrabTransformer`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Transformers.XRGeneralGrabTransformer.html>
- `IXRGrabTransformer`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Transformers.IXRGrabTransformer.html>
