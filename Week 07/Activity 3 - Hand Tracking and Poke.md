# Activity 3: Hand Tracking and Poke

> **Headset badge: headset required.** Hand tracking has no simulator. There is no
> keyboard-and-mouse hand, and the XR Interaction Simulator cannot produce one.

## Objective
Turn on hand tracking, put the controllers down, and drive the panel you built in Activity 2
with your fingers: press its buttons with a fingertip and drag a slider along its track.

## Prerequisites
- **[Activity 2](Activity%202%20-%20World-Space%20UI.md)** of this week, finished. You need the
  world-space canvas with its **Event Camera**, **`TrackedDeviceGraphicRaycaster`** and
  working `XRUIInputModule`. This activity adds nothing to that setup and depends on all of it
- **[Week 6 Activity 1](../Week%2006/Activity%201%20-%20Grab%20Interactables%20and%20Interactors.md)**,
  Step 6, where you first used the poke Interactor against an `XRSimpleInteractable`
- **Packages you'll add this week: XR Hands** (`com.unity.xr.hands`) **1.6.x**, with its
  **HandVisualizer** sample, plus the **Hands Interaction Demo** sample from the XR Interaction
  Toolkit. This is the only package added in Week 7
- Hardware: a **Meta Quest 2 / 3 / 3S** and a USB-C cable that carries data. Hand tracking
  works on all three

> **Enable hand tracking in the headset too.** **Settings → Movement tracking → Hand and body
> tracking** must be on in the Quest's own settings, or your build will see nothing however
> well Unity is configured.

## Instructions

### Step 1: Install XR Hands

**Window → Package Manager → Unity Registry → XR Hands → Install.** You want **1.6.x**.

Then import the **HandVisualizer** sample from the same page. It gives you a pair of rendered
hands so you can see what the tracking is doing, which you will want in Step 4 before you
trust anything else.

Then import the **Hands Interaction Demo** sample, from **XR Interaction Toolkit → Samples**.
You need it. Step 3 explains why.

### Step 2: The hand-tracking setup — four toggles in three places

Do all of it. **Hand tracking is enabled by four separate toggles spread across three panels,
and missing any one produces "my hands don't work" with no error and nothing in the Console.**
Nothing has gone wrong; a switch is off.

1. Enable **OpenXR** in **Project Settings → XR Plug-in Management**.
2. Enable the **Hand Tracking Subsystem** under **OpenXR Feature Groups**.
3. Enable **Meta Quest Support**, ***on the Android tab only***. The Android tab and the
   desktop tab are configured separately.
4. Enable **Meta Hand Tracking Aim**.
5. Add an **Interaction Profile** — the **Oculus Touch Controller Profile** — so controllers
   still work alongside hands.

> **Checkpoint.** Go back to **Project Settings → XR Plug-in Management → OpenXR** and look at
> the **Android** tab specifically. Hand tracking ticked, **Meta Quest Support** ticked,
> **Meta Hand Tracking Aim** ticked, one interaction profile listed. Run **Project Validation**
> while you are there.

> **Why an interaction profile in a hands module.** The runtime still expects to be told what
> controllers look like, and a project with no profile can fail to initialise at all, taking
> hand tracking down with it.

### Step 3: Let the rig switch between hands and controllers

**Swap the rig.** The `XR Origin (XR Rig)` you have used since Week 6 cannot show hands. It
carries an
[**`XRInputModalityManager`**](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Inputs.XRInputModalityManager.html),
which switches between a controller branch and a hand branch of the rig, but its **Left Hand**
and **Right Hand** fields are **empty**. There is no hand branch to switch to.

1. Delete the `XR Origin (XR Rig)` from your scene.
2. From `Hands Interaction Demo/Prefabs`, drag in **`XR Origin Hands (XR Rig)`** at
   `(0, 0, 0)`. This is the same rig plus a wired hand branch. Controllers behave exactly as
   before.
3. Select it and find the `XRInputModalityManager`. Read its four fields: **Left Controller**,
   **Right Controller**, **Left Hand** and **Right Hand**. All four are populated now, and the
   manager turns one pair off as it turns the other on.
4. Note its **Tracked Hand Mode Started** and **Motion Controller Mode Started** events. Any
   part of your UI that has to change between modes hangs off those.

> **Empty hand fields are why hands never appear.** Every OpenXR toggle can be correct, the
> system hand-tracking menu can work, and your app will still show controllers and nothing
> else. Check these four fields before you check anything else.

Both branches carry the same Interactors, so the `XRPokeInteractor` and `NearFarInteractor`
you have been using since Week 6 exist on the hands too. **Nothing on your canvas changes.**

### Step 4: Build and look at your hands

1. **File → Build Profiles → Android → Build And Run**, or drag the `.apk` onto
   **Device Manager** in **MQDH**.
2. Put the headset on **holding the controllers**, then set them down on a table and lift your
   hands where the headset can see them. The rendered hands should appear within a second or
   two.
3. Look at the tracking before you use it. Turn your hands over. Cross them. Move one behind
   the other. Hold one at the edge of your vision.

**What you should notice.** Tracking is a camera solution, so the hands are estimated rather
than measured. They jitter slightly when still, they lag your real hand by a few milliseconds,
and they degrade or vanish when a hand is occluded, out of the camera's view, or edge-on.
Everything you build on hands has to survive that.

### Step 5: Poke the panel

Your canvas from Activity 2 already has everything a poke needs. The
[**`XRPokeInteractor`**](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor.html)
on each hand has an **Enable UI Interaction** tick that registers it with the same
`XRUIInputModule` the ray uses, so a fingertip generates the same press events a trigger pull
did.

1. Move the panel to **0.5 m** from the player's start position and drop it to a height of
   around **1.1 m**, so it sits where a hand rests rather than where an eye reads.
2. Make the buttons **physically large**. Set each to **50 × 50** UI units, which at a canvas
   scale of `0.001` is **5 cm square**. Four centimetres is about the smallest target that
   works; six is comfortable.
3. Space them at least **2 cm apart** in world terms.
4. Put a **thin box behind the canvas** so your finger meets something rather than passing
   through a floating rectangle.
5. Build, and press each button with a fingertip.

**That panel is now in the wrong place for reading.** Activity 2 put it at 1.5–2 m because that
is where text is legible; poking needs it inside arm's reach. Reading and poking pull in
opposite directions, and you resolve it by choosing per panel: a panel you mostly read gets a
ray, a panel you mostly press comes to your hand. Panels that try to do both at 1 m are usually
bad at both.

### Step 6: Constrain the press, on a physical button

The canvas buttons you just poked have no sense of direction. A finger sliding sideways across
one, on its way somewhere else, presses it, and so does a finger arriving from behind the
panel.

Fixing that means leaving uGUI. **`XRPokeFilter`** constrains an *Interactable*, not a uGUI
element: its **Poke Interactable** field takes an `XRBaseInteractable`, so it has nothing to
attach to on a Canvas. You already built the object it does want, in
[Week 6 Activity 1](../Week%2006/Activity%201%20-%20Grab%20Interactables%20and%20Interactors.md)
Step 6.

1. Make a cube beside the panel, scale `(0.06, 0.02, 0.06)`, and name it `Hard Button`.
2. Give it an **`XRSimpleInteractable`**, exactly as in Week 6.
3. Add **`XRPokeFilter`**. Point its **Poke Interactable** at the `XRSimpleInteractable` and
   its **Poke Collider** at the cube's collider.
4. Open **Poke Configuration**. This is where the press lives: the axis a press must travel
   along, how deep it must go before it counts, and how far off-axis a finger may drift.
5. Wire **Select Entered** and **Select Exited** to the
   [`MaterialSwapper`](../Week%2006/Scripts/MaterialSwapper.cs) from Week 6 so you can see the
   press.
6. Build. Press the cube from the front and it fires. Swipe your finger sideways across it, or
   push it from behind, and it does not.

> **Direction is what makes a virtual button feel like a button.** A real button can only be
> pressed one way. That constraint costs you a uGUI Button and buys you a press worth having.

**So you have two routes to a pokeable control**, and they trade off against each other.

| | uGUI on a Canvas | 3D Interactable |
|---|---|---|
| **Setup** | None beyond Activity 2 | Collider, Interactable, filter, events |
| **Layout** | Rect Transform, anchors, text | By hand, in world space |
| **Press direction and depth** | Not available | `XRPokeFilter` |
| **Use it for** | Menus, settings, anything with text | Controls that should feel physical |

### Step 7: Add a slider and drag it

A button is a single event. A slider is a continuous one, and it is where poke stops feeling
like a novelty.

1. Add a **UI → Slider** to the canvas, below the buttons.
2. Size it **400 × 60** UI units, so it is **40 cm long and 6 cm tall** in the world.
3. Select the slider's **Handle** and enlarge it to fill the height of the track. A handle you
   cannot see the edges of is a handle you cannot find with a fingertip.
4. Set **Min Value** `0` and **Max Value** `1`.
5. Build, put a fingertip on the handle, and drag it along the track.

A uGUI Slider needs nothing added. The poke Interactor reports a press and a drag position to
the input module, and the Slider reads those the same way it reads a mouse.

Try the same slider with the ray from 3 m away. Both work, and they do not feel remotely alike:
the ray is precise but abstract, and the poke is imprecise but physical. A 40 cm slider is easy
to drag with a finger and fiddly to hit with a ray; a 4 cm one is the other way round.

### Step 8: Build and evaluate

Deploy, and work down this list in the headset with the controllers on the table.

- [ ] Do your hands appear within a couple of seconds of putting the controllers down?
- [ ] Can you press each button without accidentally pressing a neighbour?
- [ ] Does a sideways swipe across `Hard Button` fire nothing, where the same swipe across a
      canvas button does?
- [ ] Can you drag the slider from one end to the other in a single movement?
- [ ] Does the panel stay within reach after you teleport somewhere else?
- [ ] Pick a controller back up. Does the rig switch back cleanly?

Fix what fails, rebuild, re-check, and save the scene.

## Understanding hands versus controllers

**Hand tracking removes the button.** A controller has a trigger, a grip, two face buttons and
a thumbstick, and every one of them is an unambiguous, noise-free signal. A tracked hand has
none. What you get instead is a **pose**: twenty-six joints per hand, estimated from cameras,
arriving every frame with no guarantee that they are correct or that they arrive at all.

Everything follows from that.

| | Controllers | Tracked hands |
|---|---|---|
| **Select** | A trigger pull. Unambiguous | A pinch or a poke, inferred from joint positions |
| **Reliability** | Near total | Degrades with occlusion, lighting, and hands at the edge of view |
| **Haptics** | Yes | None. There is nothing to vibrate |
| **Precision** | High | Roughly a centimetre, and jittery |
| **Barrier to use** | Pick them up, learn the buttons | None |

**Poke is the interaction hands are best at**, because it needs no inference. A fingertip
either has or has not crossed a surface, and the answer does not depend on recognising a
gesture. That is why pressable panels are the standard hand-tracked UI and why almost every
hand-tracked system menu is a flat panel you prod.

**Design for tracking loss, not for perfect tracking.** A hand that vanishes mid-drag is normal,
not an edge case. Anything a hand can do should be recoverable when the hand comes back, and
nothing irreversible should happen on a single unconfirmed frame.

**No haptics is the real cost.** Week 8 gives an object three ways of answering when you touch
it, and one of the three is not available here. A poke button with no vibration has to do all
its work visually and audibly, which is why the press depth and the visible travel of the
button matter so much more than they do with a controller.

## Extension Activities

### **Wire the slider to something**
Point the slider at the vignette from Activity 1: `0` is fully open, `1` is your tightest
comfortable aperture. Logic: a serialised reference to the `TunnelingVignetteController`, and
one method taking a `float` wired to the slider's **On Value Changed**.

Then do the same for **move speed** on `ContinuousMoveProvider`, and you have the comfort
settings panel that Activity 1 and Activity 2 both left as an extension.

### **A poke button that moves**
A real button travels when you press it. Make `Hard Button` sink under the fingertip and spring
back. Logic: `XRPokeFilter` publishes its poke state, and the Starter Assets Affordances folder
carries the follow behaviour that reads it. Compare the moving button against the flat canvas
ones next to it.

### **Pinch to select at distance**
The `NearFarInteractor` on a tracked hand casts a ray like the controller did, but the select
comes from a pinch rather than a trigger. Test the ray from 3 m with hands and with
controllers, and note where your accuracy falls off.

### **Survive tracking loss**
Start a slider drag, then move your hand behind your other arm so the tracking drops. Note what
the slider does. Then make it safe: cache the value at drag start and only commit it when the
drag ends cleanly.

### **Reach test**
Duplicate the panel at 0.3 m, 0.5 m and 0.7 m, and find the distance at which poking stops
being comfortable for a seated player and for a standing one. Those numbers, and the 1.5–2 m
reading distance from Activity 2, are the two ends of your project's UI placement range.

## Headset checkpoint

Before you call this week done:

- **Put the controllers down** and confirm your hands appear.
- **Press** all three canvas buttons with a fingertip, and fail to press `Hard Button` with a
  sideways swipe.
- **Drag** the slider end to end with one finger.
- **Point** at the same panel with the ray from across the room, and notice which of the two
  you would rather use for each control.

## Outcome
A hand-tracked build in which the world-space panel from Activity 2 is driven by your fingers:
buttons that only accept a press from the front, and a slider you can drag. More usefully, a
first-hand sense of what an input has to survive when it is estimated from cameras rather than
read from a switch, which is the assumption behind any gesture recognition built on top of it.

## References

**XR Hands**
- Package overview and samples: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.6/manual/index.html>
- Hand Tracking OpenXR feature: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.6/manual/openxr-features/handtracking.html>
- Meta Aim Hand OpenXR feature: <https://docs.unity3d.com/Packages/com.unity.xr.hands@1.6/manual/openxr-features/metahandtrackingaim.html>

**Poke**
- `XRPokeInteractor`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor.html>
- XR Poke Interactor manual: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/xr-poke-interactor.html>
- `XRPokeFilter`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Filtering.XRPokeFilter.html>
- `XRSimpleInteractable`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable.html>

**Rig and UI**
- `XRInputModalityManager`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Inputs.XRInputModalityManager.html>
- UI setup in XRI: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/ui-setup.html>
- XRI samples, including **Hands Interaction Demo**: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/samples.html>
