# Activity 3: Writing Your Own Grab Transformer

> **Headset badge: simulator-friendly.** Write and test all of this in the **XR Interaction
> Simulator**. There is a headset checkpoint at the end.

## Objective
Implement **`IXRGrabTransformer`** yourself by inheriting **`XRBaseGrabTransformer`**, so
that a held object follows your hand under your own code — and cycles its material colour
through the rainbow as you move it up and down.

## Prerequisites
- **Activity 1** and **Activity 2** complete
- **[XR Interaction Toolkit — Core Concepts](../Guides/XRInteractionToolkit.md#writing-your-own)**,
  the *Grab Transformers → Writing your own* section
- Comfortable creating and attaching a C# script — see
  [Week 2 Activity 3](../Week%2002/Activity%203%20-%20Input%20Movement.md) if not
- **Packages you'll add this week:** none
- Hardware: none required to build this. A **Meta Quest 2 / 3 / 3S** for the closing
  checkpoint

> **Keyboard shortcuts.** Where this activity says `Ctrl`, macOS users press `Cmd`.

## Instructions

### Step 1: A prop of its own, with the automatic transformers switched off

Everything has worked so far because **Unity adds a default transformer set to
`XRGrabInteractable` automatically**. To take over, you have to turn that off — otherwise
the default and your code both write to the same target pose and you spend an hour
debugging a fight you did not know was happening.

1. Open `Week06_Interactions`. Duplicate `Prop` (`Ctrl+D`), name it `RainbowProp`, and
   move it to `(0.5, 0.825, 0.9)`.
2. Give it a material of its own — **Assets → Create → Material**, name it `RainbowProp`,
   set its base colour to something clearly not white, and drag it onto the prop. Your
   script will tint this material, so the prop needs one it does not share.
3. On the **XR Grab Interactable**, untick **Add Default Grab Transformers**.
4. Play and grab `RainbowProp`. It should now be selectable but should **not follow your
   hand** — you are holding an object that nothing is moving. That is the correct starting
   state, and it is worth seeing once.

> **Why the checkbox and not just "add mine as well".** With it ticked, the Interactable
> re-adds a default `XRGeneralGrabTransformer` on any frame it finds its single-grab list
> empty. Two transformers writing `targetPose.position` are not merged; the later one
> wins, and which one is later is not something you control.

### Step 2: The shape of a transformer

Create the script: right-click in `Assets/Scripts/` → **Create → C# Script**, name it
**CustomTransformer**, and open it.

A grab transformer is a `MonoBehaviour` that inherits `XRBaseGrabTransformer`. The base
class supplies the interface plumbing; you override the parts you care about. Start with
the `using` lines, because this is where most copied XRI code dies:

```csharp
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;
```

XRI 3.x splits these across four namespaces where 2.x used one. A sample from an older
tutorial fails on its very first line with "type or namespace could not be found", pointing
at code that looks perfectly correct. See
[the namespace trap](../Guides/XRInteractionToolkit.md#the-xri-3x-namespace-trap).

The members you can override, and when each fires:

| Member | Declared as | When it runs | What you use it for |
|---|---|---|---|
| `OnLink` | `virtual` | Once, when the transformer is registered with an Interactable | Caching references — renderers, materials, child transforms |
| `OnGrab` | `virtual` | When a grab begins | Capturing "how things were" at the start of the hold |
| `OnGrabCountChanged` | `virtual` | When the number of selecting Interactors changes mid-hold | Noticing a second hand joining or leaving |
| `Process` | `abstract` | Every frame while held | Writing the target pose and scale. This is the one that does the work, and the one you must implement |
| `OnUnlink` | `virtual` | When the transformer is unregistered from the Interactable | Releasing references |
| `registrationMode` | `virtual` | Read once, at `Start` | Declaring whether you handle one-hand grabs, multi-hand grabs, or both |

`Process` is the important one. Its signature is:

```csharp
    public override void Process(XRGrabInteractable grabInteractable,
        XRInteractionUpdateOrder.UpdatePhase updatePhase,
        ref Pose targetPose, ref Vector3 localScale)
```

Read the two `ref` parameters carefully, because they *are* the contract. You are not
setting `transform.position` on the object. You are being handed the pose and scale the
Interactable intends to apply this frame, and invited to change them. The Interactable
applies the result afterwards, honouring its own **Movement Type** — which is why a
transformer written this way still respects the kinematic-versus-velocity choice you made
in Activity 1.

`updatePhase` tells you *when* in the frame you are being called. Guard on it, or your
work runs several times a frame:

```csharp
        if (updatePhase != XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            return;
```

> **One declaration each.** If you paste a method in twice — two `Process` methods, two
> `OnGrab` methods — the file will not compile, and the error names the *second* one, which
> makes it look like the second one is wrong. Nothing is wrong with it except that the
> first one already exists. Delete the duplicate.

### Step 3: Make it follow the hand

The movement rule is the same one you would write on paper: *remember where the object was
relative to the hand when the grab started, then keep it there.*

1. Declare the state you need to remember between frames:

```csharp
    Vector3 m_GrabOffset;
```

2. Capture it when the grab begins. The hand's position is the **attach transform** of the
   Interactor doing the selecting — the same attach transform idea from Activity 1, seen
   from the other side:

```csharp
    static Transform GetFirstAttachTransform(XRGrabInteractable grabInteractable)
    {
        var interactors = grabInteractable.interactorsSelecting;
        return interactors.Count > 0 ? interactors[0].GetAttachTransform(grabInteractable) : null;
    }
```

3. In `OnGrab`, subtract to get the offset. In `Process`, add it back:

```csharp
        // Movement: follow the hand, keeping the offset captured when the grab began.
        targetPose.position = attach.position + m_GrabOffset;
```

4. Add the script to `RainbowProp`. There is nothing to drag: an `XRBaseGrabTransformer`
   finds the `XRGrabInteractable` on its own GameObject at `Start` and registers itself
   according to its `registrationMode`. The **Starting Single Grab Transformers** list on
   the Interactable is for transformers that live on *other* objects.
5. Play. The prop follows your hand and keeps its offset. It does not rotate, because you
   have not written any rotation — that is the first extension below.

> **Checkpoint.** Grab and move. If nothing happens, check that **Add Default Grab
> Transformers** is unticked and the script compiled. If the object teleports into your
> hand instead of keeping its offset, `OnGrab` is not capturing the offset.

### Step 4: Rainbow hue on vertical movement

Now the interesting part. Moving the held object **up and down cycles its colour through
the spectrum**; moving it sideways does nothing to the colour.

Colour lives in HSV for this — hue is a single number from `0` to `1` that walks around the
colour wheel, so "cycle through the rainbow" is just "add to the hue and wrap".

1. In `OnLink`, cache the Renderer and take a private copy of its material:

```csharp
        // Reading .material hands back a copy that belongs to this object alone, so
        // tinting one cube does not tint every other cube sharing the same material.
        if (m_TargetRenderer != null)
            m_MaterialInstance = m_TargetRenderer.material;
```

   Reading `.material` rather than `.sharedMaterial` is what stops you tinting every prop
   in the scene — and, in the Editor, permanently editing the material asset on disk.

2. In `OnGrab`, record the colour the object already has. Take all three components, not
   just the hue: keeping the original saturation and value is what stops a muted grey-blue
   prop snapping to a fluorescent one on the first frame of the grab.

```csharp
        // Keep the saturation and value the material already has, so only the hue moves.
        if (m_MaterialInstance != null)
            Color.RGBToHSV(m_MaterialInstance.color, out m_StartHue, out m_StartSaturation, out m_StartValue);
```

3. In `Process`, map vertical travel since the grab began onto hue, and wrap with
   `Mathf.Repeat`:

```csharp
        var verticalTravel = attach.position.y - m_StartAttachHeight;
        var hue = Mathf.Repeat(m_StartHue + verticalTravel * m_HueCyclesPerMetre, 1f);
        m_MaterialInstance.color = Color.HSVToRGB(hue, m_StartSaturation, m_StartValue);
```

`Mathf.Repeat(x, 1f)` is what makes it a cycle rather than a ramp: past `1` it wraps back
to `0`, so lifting the object keeps walking around the wheel instead of stopping at
magenta. Below `0` it wraps the other way, so lowering it runs the spectrum backwards.

4. The copy you took in `OnLink` is yours, so destroy it when the component goes away:

```csharp
    // The copy taken in OnLink belongs to this component, so this component destroys it.
    protected virtual void OnDestroy()
    {
        if (m_MaterialInstance != null)
            Destroy(m_MaterialInstance);
    }
```

### Step 5: The complete script

Your finished `CustomTransformer.cs` should match this. A reference copy is in
**[Scripts/CustomTransformer.cs](Scripts/CustomTransformer.cs)** — write yours first and
compare, rather than pasting.

```csharp
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

/// <summary>
/// Week 6, Activity 3. A one-handed grab transformer that moves the held object with
/// the hand and cycles its material hue as the hand travels up and down.
/// </summary>
public class CustomTransformer : XRBaseGrabTransformer
{
    [SerializeField]
    [Tooltip("Full hue cycles per metre of vertical hand movement.")]
    float m_HueCyclesPerMetre = 2f;

    [SerializeField]
    [Tooltip("Renderer to tint. Leave empty to use the first Renderer on the Interactable.")]
    Renderer m_TargetRenderer;

    // Registers this transformer for one-handed grabs only. The Interactable keeps a
    // separate list for multi-hand grabs, which this component does not handle.
    protected override RegistrationMode registrationMode => RegistrationMode.Single;

    Material m_MaterialInstance;
    Vector3 m_GrabOffset;
    float m_StartAttachHeight;
    float m_StartHue;
    float m_StartSaturation;
    float m_StartValue;

    public override void OnLink(XRGrabInteractable grabInteractable)
    {
        base.OnLink(grabInteractable);

        if (m_TargetRenderer == null)
            m_TargetRenderer = grabInteractable.GetComponentInChildren<Renderer>();

        // Reading .material hands back a copy that belongs to this object alone, so
        // tinting one cube does not tint every other cube sharing the same material.
        if (m_TargetRenderer != null)
            m_MaterialInstance = m_TargetRenderer.material;
    }

    public override void OnGrab(XRGrabInteractable grabInteractable)
    {
        base.OnGrab(grabInteractable);

        var attach = GetFirstAttachTransform(grabInteractable);
        if (attach == null)
            return;

        m_GrabOffset = grabInteractable.transform.position - attach.position;
        m_StartAttachHeight = attach.position.y;

        // Keep the saturation and value the material already has, so only the hue moves.
        if (m_MaterialInstance != null)
            Color.RGBToHSV(m_MaterialInstance.color, out m_StartHue, out m_StartSaturation, out m_StartValue);
    }

    public override void Process(XRGrabInteractable grabInteractable,
        XRInteractionUpdateOrder.UpdatePhase updatePhase,
        ref Pose targetPose, ref Vector3 localScale)
    {
        if (updatePhase != XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            return;

        var attach = GetFirstAttachTransform(grabInteractable);
        if (attach == null)
            return;

        // Movement: follow the hand, keeping the offset captured when the grab began.
        targetPose.position = attach.position + m_GrabOffset;

        // Colour: vertical hand travel since the grab began drives the hue.
        if (m_MaterialInstance == null)
            return;

        var verticalTravel = attach.position.y - m_StartAttachHeight;
        var hue = Mathf.Repeat(m_StartHue + verticalTravel * m_HueCyclesPerMetre, 1f);
        m_MaterialInstance.color = Color.HSVToRGB(hue, m_StartSaturation, m_StartValue);
    }

    // The copy taken in OnLink belongs to this component, so this component destroys it.
    protected virtual void OnDestroy()
    {
        if (m_MaterialInstance != null)
            Destroy(m_MaterialInstance);
    }

    static Transform GetFirstAttachTransform(XRGrabInteractable grabInteractable)
    {
        var interactors = grabInteractable.interactorsSelecting;
        return interactors.Count > 0 ? interactors[0].GetAttachTransform(grabInteractable) : null;
    }
}
```

### Step 6: Test it

1. Save the script (`Ctrl+S`) and let Unity recompile.
2. Set **Hue Cycles Per Metre** on the component to `2`. Leave **Target Renderer** empty —
   `OnLink` finds it.
3. Play. Grab `RainbowProp` and lift it. The colour should walk up through the spectrum,
   and back down as you lower it. Move it sideways and the colour holds steady.
4. Let go and grab again. The cycle should resume from the colour it currently is, not
   jump back to red — that is what capturing the colour in `OnGrab` bought you.

> **Colour never changes?** Either the prop has no Renderer beneath it, or its shader has
> no main colour property for `Material.color` to write to. Check with a plain URP Lit
> material first.

> **Colour changes on every prop at once?** Something in your scene is writing to a shared
> material. Check you used `.material`, not `.sharedMaterial`.

## Understanding what you just wrote

The reason this is worth doing is not the rainbow. It is that you have now seen the seam
that XRI is built around.

**The Interactable owns selection. The transformer owns motion.** Nothing in
`CustomTransformer` knows how a grab is triggered, which button was pressed, which hand it
was, or how the object will physically be moved into place. It is handed a target pose and
asked to change it. Everything else stays exactly as you configured it in Activity 1 —
including the movement type, which is applied *after* your code runs.

**That seam is why `XRGeneralGrabTransformer` was replaceable at all.** In Activity 2 you
swapped one transformer for another on an object you had already made grabbable, and the
grabbing did not care. Frameworks that fold "how it is held" into "that it is held" cannot
do that, and you end up rewriting the grab to change the hold.

**And it is why the default set has to be switched off.** Step 1 is not a formality. The
Interactable checks its single-grab list every frame and refills it from the default if it
is empty, so leaving the checkbox ticked does not give you "yours plus theirs" — it gives
you an argument.

## Extension Activities

### **Rotation as well as position**
The prop currently keeps whatever rotation it had. Make it turn with the hand. Logic:
record the hand's rotation in `OnGrab` alongside the position, work out the delta each
frame, and apply it to `targetPose.rotation`. Key code: `attach.rotation *
Quaternion.Inverse(m_StartAttachRotation)` gives you the rotation the hand has turned
through since the grab began. Note that your offset will also need rotating if you want it
to feel right.

### **A drawer that only slides**
Constrain the object to a single axis, so it can be pulled out and pushed in but never
lifted. Logic: in `Process`, keep the object's existing X and Y from `targetPose` and only
take the hand's contribution on Z. Key code: build the position from components rather
than assigning the whole vector, then `Mathf.Clamp` the travel so the drawer stops at both
ends. This is the version of this exercise you are most likely to actually use.

### **Colour by speed, not height**
Drive the hue from how fast the hand is moving instead of how high it is. Logic: keep last
frame's attach position, and each frame divide the change by `Time.deltaTime` to get a
speed. Then map speed onto saturation or value rather than hue, so a still object goes grey
and a fast one goes vivid — `Color.HSVToRGB` takes all three.

### **A held look, using MaterialSwapper**
Instead of tinting, swap the whole material while the object is held. Add
**[MaterialSwapper](Scripts/MaterialSwapper.cs)** to the prop and wire the
`XRGrabInteractable`'s **Select Entered** and **Select Exited** events to
`SwapToMaterialB` and `SwapToMaterialA`. No new code at all — and worth doing once,
because it shows how much of XRI is reachable from the Inspector.

### **Two hands, one transformer**
Override `OnGrabCountChanged` and make the prop do something different when a second hand
joins — freeze in place, change its hue cycle rate, or go monochrome. Logic: read
`grabInteractable.interactorsSelecting.Count` inside that override and store the result;
branch on it in `Process`. You will also need to change `registrationMode` to
`RegistrationMode.SingleAndMultiple` so the transformer is registered for both.

## Headset checkpoint

Build to the headset (**File → Build Profiles → Build and Run**).

- **Tune `m_HueCyclesPerMetre` on device.** A value that looked lively on a monitor is
  usually far too fast in the headset, where a metre of vertical hand travel is a large,
  deliberate movement rather than a flick of the mouse.
- **Watch the offset.** With a real tracked hand, an object held a hand's-width away from
  where your hand actually is feels immediately wrong in a way it never does on screen.
- **Check the frame rate.** You are writing to a material every frame. It is cheap here,
  but form the habit of asking.

## Outcome
A grab transformer of your own that moves a held object and repaints it from its vertical
motion — and a working understanding of the `IXRGrabTransformer` contract, which is the
piece of XRI that most repays knowing.

## References
- `IXRGrabTransformer`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Transformers.IXRGrabTransformer.html>
- `XRBaseGrabTransformer`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Transformers.XRBaseGrabTransformer.html>
