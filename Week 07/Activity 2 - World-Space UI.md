# Activity 2: World-Space UI

> **Headset badge: headset required.** Whether a menu is readable, reachable and comfortable
> are three questions a monitor answers wrongly. A panel that looks right in the Game view is
> routinely unreadable or out of reach on the headset.

## Objective
Take the world-space canvas you already know how to build and make it work in XR: sized in
metres, placed inside the comfortable viewing zone, and driven by a ray from across the room.
Then compare head-locked, world-locked and body-locked placement on the headset.

## Prerequisites
- **[Week 3 Activity 2 — Unity UI Elements](../Week%2003/Activity%202%20-%20Unity%20UI.md)**.
  This activity assumes all of it: Canvas, Rect Transform, anchors, TextMeshPro, wiring a
  Button to a script, and the difference between **Screen Space — Overlay** and **World
  Space** render modes. None of that is repeated here
- **[Activity 1](Activity%201%20-%20Locomotion%20and%20Comfort.md)** of this week. You need to
  be able to move around the scene to test the panel from more than one position
- **[XR Interaction Toolkit — Core Concepts](../Guides/XRInteractionToolkit.md)**, in
  particular `NearFarInteractor`
- **Packages you'll add this week:** none. **XR Interaction Toolkit** 3.6.x and its
  **Starter Assets** sample are already installed, and TextMeshPro Essentials came in with
  Week 3
- Hardware: a **Meta Quest 2 / 3 / 3S** and a USB-C cable that carries data

> **Keep Step 5 short.** It asks you to experience head-locked UI, which some people find
> unpleasant within seconds. Take the headset off and stop if you feel unwell.

## Instructions

### Step 1: Start from what you already built

In [Week 3 Activity 2](../Week%2003/Activity%202%20-%20Unity%20UI.md) you built a canvas with
text and a button, switched its **Render Mode** to **World Space**, and pushed it out into
the scene.

1. Open the scene from **Activity 1** of this week.
2. Create a canvas the same way you did in Week 3 — `UI → Canvas` — and set its
   **Render Mode** to **World Space** straight away.
3. Add a **Panel** as a background, one **Text — TextMeshPro** heading, and **three
   Buttons**. Label the buttons `Teleport`, `Continuous` and `Vignette`. They will not do
   anything yet.
4. Delete or disable any leftover **Screen Space — Overlay** canvas in the scene.

> **Screen Space — Overlay does not work in XR.** There is no screen to overlay onto. There
> are two eye images rendered from slightly different positions, and an overlay canvas is
> drawn at a fixed place in both, so your eyes cannot converge on it. The
> [Canvas render modes](https://docs.unity3d.com/Packages/com.unity.ugui@2.6/manual/UICanvas.html)
> are documented with the uGUI package.

### Step 2: Size the canvas in metres

A canvas in world space is measured in **Unity units, which are metres**. An 800 × 600 Rect
Transform at scale 1 is a panel **800 metres wide**.

1. Select the Canvas. Set the **Rect Transform** **Width** to `800` and **Height** to `600`.
   Work in these familiar UI units for layout, exactly as you did in Week 3.
2. Set the Canvas **Scale** to `(0.001, 0.001, 0.001)`.
3. Do the arithmetic: 800 × 0.001 = **0.8 m wide**, 600 × 0.001 = **0.6 m tall**. That is
   about the size of a large monitor.
4. Set your heading text to a font size of around `36` UI units. At a scale of `0.001` that
   is a **3.6 cm** cap height, which is readable at 2 m.
5. Find **Dynamic Pixels Per Unit** on the **Canvas Scaler** and raise it to `3` if your text
   looks soft. It renders the text at a higher internal resolution for the same physical size.

**Decide the physical size first, then work backwards to the scale.** "How many pixels should
this be?" is a meaningless question in XR. "How many centimetres wide is this panel?" is the
question, and the scale factor is the arithmetic that gets you there.

> **Checkpoint.** Drop a 1 m cube next to the canvas in the Scene view. If your panel is not
> obviously in the same size class as the cube, your scale is wrong. `Ctrl+S` — macOS users,
> `Cmd+S`.

### Step 3: Place it where a person can look at it

Position, distance and angle matter as much as size, and the constraints come from human
anatomy rather than from Unity.

| Property | Value | Why |
|---|---|---|
| **Distance** | 1.5 m to 2 m | Closer than 0.5 m and your eyes have to cross to converge. Further than 5 m and text has to be enormous. Fixed-focus headsets are sharpest around here |
| **Height** | centre near 1.4 m | People look slightly down at rest. A panel at or above eye height holds the neck extended |
| **Tilt** | up by about 15° | So its face points at the eyes rather than past them |
| **Bearing** | within 30° of straight ahead | Anything needing a body turn needs a reason |

For a player starting at the origin and facing +Z, that is position `(0, 1.4, 1.8)` and
rotation `(-15, 180, 0)`, so the panel faces back towards the player.

### Step 4: Make it work with a ray

Pointing at a menu from across the room is the far-interaction case. XRI handles it, but it
needs three pieces present, and missing any one produces a canvas that looks fine and ignores
you.

1. **On the Canvas:** set **Event Camera** to the tracked **Main Camera** inside your XR
   Origin. A world-space canvas with no event camera cannot convert a ray hit into a UI
   position.
2. **On the Canvas:** add
   [**`TrackedDeviceGraphicRaycaster`**](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.UI.TrackedDeviceGraphicRaycaster.html).
   Unity's standard `GraphicRaycaster` understands a mouse; this one casts against the canvas
   from a tracked device.
3. **In the scene:** your **EventSystem** needs XRI's
   [**`XRUIInputModule`**](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.UI.XRUIInputModule.html)
   rather than the default standalone one. Interactors register with it to reach uGUI at all.
   The Starter Assets rig brings a configured EventSystem with it, so check before you add a
   second one. Two EventSystems in a scene fight.
4. **On the rig:** nothing to add. The
   [**`NearFarInteractor`**](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactors.NearFarInteractor.html)
   on each hand already drives canvases with the same ray it uses for far grabbing.

Unity's own walkthrough of this setup is
[UI setup](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/ui-setup.html), and the package ships a **World Space UI** sample with
a worked demo scene for both uGUI and UI Toolkit.

Point at a button and it should highlight. Pull the select trigger and it should click.

> **When a canvas ignores your ray, check those three in order.** Event camera, raycaster,
> input module. It is almost never the interactor.

### Step 5: Compare head-locked, world-locked and body-locked

Keep this step short.

1. **Head-locked.** Drag your Canvas so it is a **child of the Main Camera** inside the XR
   Origin, at local position `(0, 0, 1.8)`. Build, put the headset on, look around, and walk
   your route from Activity 1. **Thirty seconds is enough.**
2. **World-locked.** Un-parent the canvas back into the scene at the position from Step 3.
   Build and walk the same route.
3. **Body-locked.** Parent the canvas under the **XR Origin** root, so it follows you as you
   teleport and walk but does not follow your head rotation. Build and walk the route again.

Leave your panel in whichever placement won.

**Head-locked UI is uncomfortable for three reasons stacking on top of each other:**

- **You cannot look away from it.** Your eyes make small movements constantly, and normally
  the world stays still while they do. A head-locked panel moves with every one of them, so
  your eyes never settle. It behaves like a smudge on your glasses.
- **It sits at a fixed distance forever**, so the muscles that focus and converge your eyes
  never relax. This is what produces the headache.
- **It is the one thing in the scene that does not obey the world.** When you turn,
  everything sweeps past except the panel.

**Body-locked is the useful compromise.** The panel stays available wherever you go, so you
never have to walk back to a menu, but it holds still while you look around. A good
body-locked menu also lags: it follows you after a short delay and stops when it is roughly
in front of you, rather than tracking you rigidly.

### Step 6: Build and evaluate

Build the `.apk` and deploy — **File → Build Profiles → Android → Build And Run**, or drag
the `.apk` onto **Device Manager** in **MQDH** if Unity cannot see the headset. Then work down
this list in the headset.

- [ ] Can you read the heading from where the panel is placed, without leaning in?
- [ ] Is the panel the physical size you intended? Compare it against a 1 m cube.
- [ ] Does the ray highlight buttons from across the room, and click reliably?
- [ ] Does the panel stay comfortable after you have teleported somewhere else?
- [ ] Is anything on it too close to the edge of your field of view to notice?

Anything that fails is a placement or sizing problem, and both are Inspector values. Fix them,
rebuild, re-check. Save the scene when the list passes.

## Understanding world-space UI in XR

**A canvas in XR is a physical object.** That reframing fixes most UI problems in XR. It has a
size in centimetres, a position in the room, and a distance from the person reading it. Every
question you would ask about a poster on a wall — can I read it from here, can I reach it, is
it at a sensible height, is it in my way — is the right question to ask about your menu.

**Three ways to attach UI:**

| Attachment | Follows | Use it for |
|---|---|---|
| **World-locked** | Nothing. It stays in the world | Diegetic UI, signage, control panels on machines, anything belonging to a place |
| **Body-locked** | Your position, not your head rotation | Menus, inventories, tools that must be available anywhere |
| **Head-locked** | Position and rotation | Almost nothing. Brief fades, loading screens, the vignette from Activity 1 |

The vignette is a legitimate head-locked element because it is not something you look *at*.
Head-locking is defensible for effects applied to your vision and indefensible for content you
need to read.

**Text is the hard part.** World-space text is rendered at a fixed resolution and then sampled
by a headset whose per-eye resolution is far below your monitor's, so text that is crisp in the
Game view is often mush on device. The levers are physical size, distance, contrast, and
Dynamic Pixels Per Unit, in that order of effectiveness.

**A ray amplifies hand tremor over its length.** At 5 m a 1 cm target is genuinely hard to hit
and a 6 cm one is easy. That is the argument for the other way of driving a panel, which is
[Activity 3](Activity%203%20-%20Hand%20Tracking%20and%20Poke.md), where the same panel is
driven by a fingertip.

## Extension Activities

### **A lazy body-locked menu**
Make the panel follow the player properly: it stays put while they look around, then smoothly
catches up when they walk away or turn far enough that it leaves view.

Logic: each frame, compute the position you want — a fixed distance in front of the XR Origin,
at a fixed height, using only the camera's Y rotation so the panel never tilts. Compare it
against where the panel is. If the angle between them is under a dead zone of roughly 35°, do
nothing. Above that, ease towards the target.

```csharp
using UnityEngine;

public class LazyFollowPanel : MonoBehaviour
{
    [SerializeField] Transform head;        // the tracked Main Camera — drag it in
    [SerializeField] float distance = 1.8f;
    [SerializeField] float deadZoneDegrees = 35f;
    [SerializeField] float smoothTime = 0.4f;

    Vector3 velocity;
    // Build a flattened forward from head.forward (zero the Y, normalise), place the
    // target at head.position + flatForward * distance, and only SmoothDamp towards it
    // when Vector3.Angle between the current and target directions exceeds the dead zone.
}
```

The dead zone is the trick. Without it the panel chases your head continuously and you have
rebuilt head-locked UI with extra steps.

### **The comfort settings panel**
Make the three buttons do what they say, and finish the extension you started in Activity 1.
`Teleport` and `Continuous` enable one movement provider and disable the other; `Vignette`
toggles the vignette.

Logic: serialised references to the provider components, and `component.enabled = true` or
`false` in each button handler. Wire the buttons exactly as you did in
[Week 3 Activity 2](../Week%2003/Activity%202%20-%20Unity%20UI.md). `onClick.AddListener` has
not changed.

### **Distance and legibility test**
Duplicate your panel at 0.5 m, 1 m, 2 m, 5 m and 10 m, scaling each so it occupies the same
angular size: double the distance, double the scale. Build once and stand at the origin. Note
the closest one you can comfortably converge on and the furthest one you can still read. Those
two numbers are your project's usable UI range, and they are properties of the headset rather
than of your scene.

### **Curve the panel**
A flat panel wider than about 0.8 m has edges noticeably further from your eyes than its
centre. Break it into three sub-panels angled inwards by about 10° each and compare
readability at the edges.

### **Diegetic instead of floating**
Delete the floating panel and put the same three controls on a physical object: a console, a
wrist-mounted display, or a clipboard you can pick up with the `XRGrabInteractable` from
Week 6. Note what it costs you in discoverability and what it buys you in presence.

## Headset checkpoint

Before you move on to Activity 3:

- **Read** your heading from the panel's placed position without leaning in.
- **Click** a button with the ray from at least 3 m away.
- **Teleport** somewhere else and confirm the panel is still usable from there.
- **Confirm** your panel ended up in the placement you chose in Step 5, and that you tried all
  three before choosing.

## Outcome
A world-space menu sized in metres rather than pixels, placed inside the comfortable viewing
zone, driven by a ray from across the room, in the attachment mode you chose after feeling all
three. You now size and place UI by asking how big it is in centimetres and how far away it is
in metres, which is the only way of asking that survives contact with a headset.

## References

**Components**
- `TrackedDeviceGraphicRaycaster`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.UI.TrackedDeviceGraphicRaycaster.html>
- `XRUIInputModule`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.UI.XRUIInputModule.html>
- `NearFarInteractor`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactors.NearFarInteractor.html>

**Manual**
- UI setup in XRI: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/ui-setup.html>
- XRI samples, including **World Space UI**: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/samples.html>
- Canvas and its render modes: <https://docs.unity3d.com/Packages/com.unity.ugui@2.6/manual/UICanvas.html>
