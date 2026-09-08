# Activity 1: Locomotion and Comfort

> **Headset badge: headset required.** Every judgement in this activity is one your inner
> ear makes. The XR Interaction Simulator cannot make it for you, and neither can the Scene
> view.

## Objective
Configure teleportation, continuous movement, snap turning and smooth turning from the XR
Interaction Toolkit's own locomotion providers, add the tunnelling vignette, and compare six
configurations of them on a headset.

## Prerequisites
- **Week 5** complete: a project made from the **VR template**, upgraded to **XRI 3.6.0**,
  switched to Android, with the **Meta Quest Support** feature group ticked and
  **Project Validation** clean
- **Week 6** complete: the `XR Origin (XR Rig)` prefab, and the Interactors each hand carries
- **Read [XR Interaction Toolkit — Core Concepts](../Guides/XRInteractionToolkit.md)** first.
  This activity uses that vocabulary and does not re-explain it
- **Packages you'll add this week:** none. **XR Interaction Toolkit** 3.6.x and its
  **Starter Assets** sample came in with Week 5, and everything this activity needs is inside
  that sample
- Hardware: a **Meta Quest 2 / 3 / 3S**, charged, and a USB-C cable that carries data
- Roughly 3 m × 3 m of clear floor space, and a chair within reach

> **Stop at the first symptom.** This activity includes movement styles that make some
> people feel unwell. Take the headset off and stop for the day if you feel queasy, sweaty
> or headachy. Symptoms often arrive a minute or two after you stop, so stop early.

> **Keyboard shortcuts.** Where this activity says `Ctrl`, macOS users press `Cmd` —
> `Ctrl+S` becomes `Cmd+S`.

## Instructions

### Step 1: Build a scene worth moving through

An empty grey plane produces almost no sensation of motion, and a scene like that will tell
you nothing.

1. Open your XR scene from Week 6, or make a new one from the **VR template**.
2. Give yourself a floor about **20 m × 20 m**. A Plane at scale `(2, 1, 2)` is 20 m across.
3. Scatter **fifteen to twenty objects** across it at real-world sizes: crates around 0.5 m,
   columns 3 m tall, a low wall, a doorway you can walk through. Put some at 2 m from the
   centre and some at 15 m.
4. Place several of them **within a metre of where you will be walking**.
5. Mark out a **fixed test route**: a lap that passes close to at least three objects and
   includes two changes of direction.

Step 4 is the one that matters. What triggers motion sickness is **optical flow**, the rate
at which the image sweeps across your retina, and that is dominated by nearby objects in
your peripheral vision. A distant mountain barely moves. A pillar you pass at arm's length
sweeps across your whole field of view.

> **Checkpoint.** Save the scene and add it to **File → Build Profiles → Scene List**.
> `Ctrl+S` — macOS users, `Cmd+S`.

### Step 2: Add the Starter Assets rig

The Starter Assets sample ships a rig with the locomotion pieces already assembled and wired
to the default input actions.

1. In the Project window, open
   `Assets/Samples/XR Interaction Toolkit/<version>/Starter Assets/Prefabs`, where
   `<version>` is the XRI version number.
2. Drag **`XR Origin (XR Rig)`** into your scene at `(0, 0, 0)`.

> **One rig only.** If your scene already has an **XR Origin (VR)** from the
> `GameObject → XR` menu, delete it now. Two rigs means two tracked cameras and two audio
> listeners.

3. Expand the prefab in the Hierarchy. Alongside `Camera Offset` there is a **`Locomotion`**
   object, and one child of it per style of movement:

   ```
   Locomotion
   ├── Move           ← Dynamic Move Provider
   ├── Turn           ← snap and continuous turn providers
   ├── Grab Move      ← move by pulling the world past you
   ├── Teleportation  ← Teleportation Provider
   ├── Climb          ← Climb Provider
   └── Gravity
   ```

4. Click each child and read its component list.

**Those component lists are your reference for the rest of this activity.** Find components
by what they do as well as by name: **Add Component** and typing `teleport`, `move`, `turn`
or `locomotion` will surface them.

### Step 3: Read the shape of the locomotion system

XRI splits locomotion into two kinds of thing.

| Piece | What it is | Examples |
|---|---|---|
| **Locomotion provider** | One component per style of movement. Asks to move the rig when its input action fires | [`TeleportationProvider`](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider.html), [`ContinuousMoveProvider`](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement.ContinuousMoveProvider.html), [`SnapTurnProvider`](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning.SnapTurnProvider.html), [`ContinuousTurnProvider`](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning.ContinuousTurnProvider.html) |
| **Mediator** | One component on the rig. Gives providers access to the XR Body Transformer, and decides which one may move the rig this frame | [`LocomotionMediator`](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.LocomotionMediator.html) |

The mediator is why a teleport landing and a thumbstick push in the same frame do not fight
over the same transform. The full set of providers is listed in
[Locomotion overview](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/locomotion.html).

Every provider moves the **XR Origin**, never the camera. Writing to the camera transform
moves the world under a head that did not move.

> **You are not writing these yourself.** A move provider is thirty lines, and five of them
> are subtle: frame-rate independence, moving relative to the tracking origin, and
> cooperating with whatever else wants to move the rig.

### Step 4: Teleportation

Teleportation is the safest locomotion that exists, because there is no continuous motion for
your inner ear to disagree with.

1. Check the `Locomotion/Teleportation` object for **`TeleportationProvider`**, and add it if
   it is missing.
2. Select your floor Plane and add
   [**`TeleportationArea`**](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationArea.html).
   The whole surface is now somewhere the player may land.
3. Create a small platform elsewhere in the scene and give it
   [**`TeleportationAnchor`**](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor.html)
   instead. Its **Teleport Anchor Transform** is the pose the player arrives in: they aim
   anywhere on the anchor and always land at the same place, facing the same direction.
4. **Set the Interaction Layer Mask on both to `Teleport`.** Read the callout below before you
   skip this.
5. Push the **right** thumbstick forward on the headset. That hand switches to teleport aiming
   and shows an arc, and releasing commits the teleport.

> **Teleport fails silently on the wrong interaction layer.** The rig's Teleport Interactor
> only sees interactables on the **Teleport** layer, and a component you add yourself starts on
> **Default**. The arc appears, the reticle does not, nothing happens, and nothing is logged.

The `Teleport` layer is created for you when the Starter Assets sample is imported. Check it
exists under **Edit → Project Settings → XR Plug-in Management → XR Interaction Toolkit →
Interaction Layer Settings**, which is the same list you used for `Tools` in
[Week 6 Activity 4](../Week%2006/Activity%204%20-%20Sockets.md).

> **Ready-made teleport prefabs live in a different sample.** The `Teleport` folder in Starter
> Assets holds reticles, not interactables. `Teleport Anchor` and `Teleport Floor Area` prefabs,
> already on the right layer, ship with the **Hands Interaction Demo** sample. Dropping one in
> and reading what is on it is a good way to check your own against a known-correct one.

Area versus anchor is a design decision. **Areas** give freedom and suit open ground.
**Anchors** give control and suit anywhere that arriving in the wrong spot breaks something:
the seat of a vehicle, one side of a workbench, a viewing position you composed.

### Step 5: Continuous movement

Continuous movement is what everybody wants and what makes everybody sick.

1. Select `Locomotion/Move`. The Starter Assets rig carries a **Dynamic Move Provider** here,
   which derives from
   [`ContinuousMoveProvider`](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement.ContinuousMoveProvider.html)
   and adds a per-hand choice of forward direction.
2. Set **Move Speed** to around **2 m/s**, roughly a brisk walk. Higher speeds are more
   optical flow, not more fun.
3. Find the field that decides **what "forward" means** and set it to the **head** for your
   first test. The alternative is the hand, so that you walk where you point rather than
   where you look.
4. Leave **Enable Strafe** ticked so sideways movement works.

**Movement is on the left stick, and teleport on the right.** That split is not set on the
providers. The rig ships with **Smooth Motion Enabled** ticked on the `Left Controller` and
unticked on the `Right Controller`, which enables the `Move` action on one hand and the
teleport actions on the other. Step 6 covers the component that does this.

Head-relative and hand-relative feel different, and people split on which is worse.
Head-relative means you cannot look sideways while moving without changing course.
Hand-relative means you can, which is more capable and, for many people, worse: the image
sweeps sideways across your eyes while your body believes it is going straight.

### Step 6: Snap turning and smooth turning

Turning is usually worse than moving. Rotation produces optical flow across your entire field
of view at once, with nothing standing still to anchor you.

1. Select `Locomotion/Turn`. Both **`SnapTurnProvider`** and **`ContinuousTurnProvider`** are
   already there, and both are already enabled. Leave them that way.
2. Set **Turn Amount** on `SnapTurnProvider` to **45** degrees, and note **Turn Speed** on
   `ContinuousTurnProvider` in degrees per second.
3. Now choose which one you get, which is **not** done on either provider. Select
   `Right Controller` on the rig, find **`ControllerInputActionManager`**, and look at
   **Smooth Turn Enabled**. Off gives you snap turning; on gives you smooth. Leave it **off**
   for now and turn it on in Step 9.

> **Turn style is chosen on the controller, not on the provider.** The two providers listen to
> two different input actions, `Turn` and `Snap Turn`, and `ControllerInputActionManager`
> enables exactly one of them. Disabling a provider component looks like it should work and
> changes nothing you can feel.

The same component decides move versus teleport, through **Smooth Motion Enabled**. Turning is
switched off entirely on a hand using smooth motion, which is why the rig ships like this:

| Controller | Smooth Motion Enabled | That stick does |
|---|---|---|
| **Left** | ticked | Continuous movement |
| **Right** | unticked | Teleport aiming, and snap or smooth turning |

> **Enable Strafe wins over turning on the same hand.** If you move that stick's hand to smooth
> motion with strafe on, sideways input strafes rather than turns.

A snap turn replaces the rotation with a jump. For a fraction of a second there is no image,
then you are facing 45° further round. Your eyes never see the sweep. It looks crude on a
monitor and it is the correct default in almost every shipped title.

### Step 7: The tunnelling vignette

The vignette is the standard mitigation for continuous movement. While the player moves, the
edges of their view are masked, narrowing what they see to a tunnel through the middle. When
they stop, it opens back up.

1. In the Project window, open the **`TunnelingVignette`** folder inside `Starter Assets`. It
   sits beside `Prefabs`, not inside it. Nothing to install: this came with the sample in
   Week 5.
2. Drag **`TunnelingVignette.prefab`** into the Hierarchy as a child of the **Main Camera**
   inside your rig, at local position `(0, 0, 0)`.
3. Select it and find
   [**`TunnelingVignetteController`**](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort.TunnelingVignetteController.html).
   Its **Locomotion Vignette Providers** list is what tells it when you are moving.
4. Add two entries to that list. Each entry is a
   [`LocomotionVignetteProvider`](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort.LocomotionVignetteProvider.html):
   a **Locomotion Provider** reference, an **enabled** tick, and an optional parameter
   override. Point one at your move provider and one at `ContinuousTurnProvider`.
5. Leave `TeleportationProvider` out, or set its entry to disabled. A teleport has no image
   sweep to mask.
6. Leave the vignette parameters at their defaults.

> **Each provider gets its own entry, with its own override.** That is why the list is a list
> of wrappers rather than a list of providers: it lets a fast turn tunnel harder than a walk.

> **Parenting under the camera is allowed.** The camera drives its child. That is not the
> same as writing to the camera's transform, which is still forbidden.

Peripheral vision is where motion is detected most strongly, and where the conflict between
"my eyes say I am moving" and "my inner ear says I am sitting still" is sharpest. Hiding the
periphery during motion removes most of the conflicting signal. The cost is that the player
sees less of your world at exactly the moment they are travelling through it, which is why
this is a value you tune and ideally one you let the player turn off.

### Step 8: Build to the headset

You cannot do the rest of this activity in the Editor.

1. **File → Build Profiles → Android**, confirm your headset appears under **Run Device**,
   then **Build And Run**. Put the output folder **outside** your repository.
2. If Unity cannot see the device, build the `.apk` and drag it onto the **Device Manager**
   in **MQDH**. That path works identically on Windows and macOS.
3. Find the app in the headset under **Library → Unknown Sources**, listed by your Product
   Name.

> **Check the stationary case first.** Stand still in the scene for thirty seconds and look
> around. If the world already feels wrong when you are not moving, check **Tracking Origin
> Mode** is **Floor** on the XR Origin before you go any further.

### Step 9: Compare the six configurations

Walk your fixed route from Step 1 in each of these. Switching between them is three controls:
**Smooth Motion Enabled** and **Smooth Turn Enabled** on the two `ControllerInputActionManager`
components, and the fields on the Dynamic Move Provider. Group the rebuilds: 2, 3 and 6 differ
only in Dynamic Move Provider fields.

| # | Configuration |
|---|---|
| 1 | Teleport only, snap turn |
| 2 | Continuous move, **head-forward**, snap turn, vignette **off** |
| 3 | Continuous move, **hand-forward**, snap turn, vignette **off** |
| 4 | Continuous move, head-forward, **smooth turn**, vignette **off** |
| 5 | Continuous move, head-forward, snap turn, **vignette on** |
| 6 | Continuous move at **4 m/s**, head-forward, snap turn, vignette off |

Spend a maximum of two minutes in each one, and take the headset off for a full minute
between them. Symptoms lag behind the cause, and running the tests back to back smears one
result into the next.

Notice **when** a sensation starts and **what you were doing** at the time. "It started when
I turned while still moving forward, and only near a pillar within about a metre" tells you
which line in the Inspector to change. "It felt bad" does not.

Then decide three things and leave the scene set up accordingly: the configuration you would
ship as your default, the one you would offer as an option, and the one you would not ship
at all.

> **Try somebody else's build.** People differ enormously. A configuration that is fine for
> you and unbearable for the person next to you is the reason shipped XR software offers
> comfort settings rather than picking one.

Save the scene.

## Understanding motion sickness in XR

**The cause is sensory conflict.** Your balance comes from two sources that normally agree:
your eyes, and the vestibular system in your inner ear. Move a virtual world past a
stationary player and the eyes report travel while the inner ear reports stillness. Your
brain has no category for "my senses disagree", but it has one for "I have been poisoned and
my perception is unreliable". The response to that is nausea.

**Vection is the trigger.** Vection is the illusion of self-motion produced by a moving
image, and it is strongest when that image fills your peripheral vision. Four rules follow.

| Rule | Consequence |
|---|---|
| Nearby objects cost more than distant ones | The same speed past a wall is worse than across open ground |
| Rotation costs more than translation | And the two together cost more than either alone |
| Acceleration costs more than constant speed | Smoothing thumbstick input can make things worse, not better |
| Anything that moves the horizon is expensive | View tilt, camera shake and head bob all read as your body being thrown around |

**Why teleport wins.** There is no image sweep, so no vection, so no conflict. The cost is
presence: teleporting feels like a menu action rather than like travelling, and players lose
track of where they are relative to where they were. Anchors and short teleport ranges help.

**Why snap turning wins.** The turn is discontinuous, so your eyes never see the sweep. A
brief fade during the snap improves it further. The 45° default is a compromise: smaller
angles need more presses, larger ones lose your bearings.

**Why the vignette works.** It removes the part of the visual field that generates the most
vection, and only while the conflict exists. It will not save a scene whose real problem is
acceleration, camera shake, or 4 m/s past close walls.

**Frame rate is a separate problem with the same symptoms.** If your app is not holding a
steady frame rate on device you will feel unwell in it whatever your locomotion settings say.
If even teleport feels bad, check performance before you change providers.

## Extension Activities

### **Speed-linked vignette**
Make the vignette respond to how fast the player is going rather than being on or off: wide
open at a stroll, tight at a sprint.

Logic: read the current movement magnitude from your move provider each frame, map it onto an
aperture between fully open and your tightest comfortable setting, and write that to the
controller. Smooth the result over about 0.2 s so the aperture does not flicker.

```csharp
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;
```

Key ideas: serialised references to the provider and the vignette controller, dragged in
rather than found at runtime; `Mathf.InverseLerp` to turn speed into a 0–1 value; and
`Mathf.SmoothDamp` to ease the aperture.

### **A comfort settings menu**
Everything you compared in Step 9 should be a player choice. Build a runtime toggle set:
teleport or continuous, snap or smooth turn, vignette strength, movement speed.

Logic: each option enables or disables a provider component and writes one Inspector value.
`provider.enabled = false` is the whole implementation of "turn off smooth turning".

> **Toggle the actions, not the providers.** A runtime menu that sets `provider.enabled` on a
> turn provider does nothing you can feel, because `ControllerInputActionManager` decides which
> turn action is live. Set **`smoothTurnEnabled`** on that component instead, and
> **`smoothMotionEnabled`** for move versus teleport.

You need somewhere to put it, and that is
**[Activity 2](Activity%202%20-%20World-Space%20UI.md)**.

### **Anchors that arrive facing the right way**
Place three `TeleportationAnchor` objects around a workbench so the player always arrives
facing the bench, whichever they choose. Then try the same with a `TeleportationArea` and
note how much harder scene composition becomes when you cannot control arrival orientation.

### **Fade the snap**
Add a brief black fade over the snap turn: out over roughly 0.05 s, in over roughly 0.1 s.
Logic: subscribe to the turn provider's locomotion started and ended events, and drive the
alpha of a full-screen quad parented under the camera. Compare it against the unfaded snap.

### **Climb something**
[`ClimbInteractable`](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Climbing.ClimbInteractable.html)
is locomotion dressed as an Interactable: grab it, and pulling your hand down moves the rig
up. The rig's `Locomotion/Climb` object already has the provider. Add a ladder to the scene
and compare how climbing feels against teleporting to the top.

## Headset checkpoint

Before you call this activity done:

- **Teleport** across the scene and land on an anchor facing the direction you intended.
- **Walk** your route with continuous movement and snap turning.
- **Switch the vignette on** and walk the same route again. You should be able to feel the
  difference, not just see it in the Inspector.
- **Confirm the scene is saved** in the configuration you chose in Step 9.

## Outcome
A scene you can move through by teleporting, walking and turning, built from XRI's own
locomotion providers, with a tunnelling vignette you can switch on and off — and six
configurations you have felt the difference between. You no longer trust a movement decision
that has only ever been seen on a monitor.

## References

**Locomotion providers**
- `TeleportationProvider`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider.html>
- `TeleportationArea`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationArea.html>
- `TeleportationAnchor`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationAnchor.html>
- `ContinuousMoveProvider`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement.ContinuousMoveProvider.html>
- `SnapTurnProvider`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning.SnapTurnProvider.html>
- `ContinuousTurnProvider`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning.ContinuousTurnProvider.html>
- `LocomotionMediator`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.LocomotionMediator.html>
- `ClimbInteractable`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Climbing.ClimbInteractable.html>

**Comfort**
- `TunnelingVignetteController`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort.TunnelingVignetteController.html>
- `LocomotionVignetteProvider`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort.LocomotionVignetteProvider.html>

**Manual**
- Locomotion overview: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/locomotion.html>
- Teleportation: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/teleportation.html>
- Starter Assets sample: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/samples-starter-assets.html>
