# Activity 1: Reacting to Interaction Events

> **Headset badge: headset required.** The visual and audio feedback work in the **XR
> Interaction Simulator**. The haptic feedback does not, because the simulated controllers
> have no motor, so the last step is on a **Meta Quest 2 / 3 / 3S**.

## Objective
Make a grabbable cube respond when it is picked up: it changes colour, plays a sound, and
vibrates the controller that grabbed it. All three responses are triggered by the same
**Select Entered** event on its `XRGrabInteractable`.

## Prerequisites
- **Your Week 7 project.** It already has the **XR Interaction Toolkit** 3.6.x, its
  **Starter Assets** sample, and the Android build settings. Keep working in it rather than
  starting a new project
- **`MaterialSwapper.cs` from Week 6** in `Assets/Scripts/`. If it is not there, copy it in
  from [Week 6 Scripts](../Week%2006/Scripts/MaterialSwapper.cs)
- **[XR Interaction Toolkit Core Concepts](../Guides/XRInteractionToolkit.md)**, in particular
  Interactors and Interactables
- **Packages you'll add this week:** none
- Hardware: a **Meta Quest 2 / 3 / 3S** with controllers, and a USB-C cable that carries data

> **Keyboard shortcuts.** Where this activity says `Ctrl`, macOS users press `Cmd`.
> `Ctrl+S` becomes `Cmd+S`, and `Ctrl`-click becomes `Cmd`-click.

## Instructions

### Step 1: Set up the scene

This is the layout from Week 6 Activity 1, without the props.

1. Create a new scene and save it as `Week08_Feedback` in `Assets/Scenes/` (`Ctrl+S`).
2. Delete the **Main Camera** the new scene came with. The rig brings its own.
3. Drag the **`XR Origin (XR Rig)`** prefab from
   `Assets/Samples/XR Interaction Toolkit/3.6.0/Starter Assets/Prefabs/` into the scene at
   `(0, 0, 0)`.
4. Add a floor: **GameObject → 3D Object → Plane** at `(0, 0, 0)`.
5. Add a table: **GameObject → 3D Object → Cube**, name it `Table`, position
   `(0, 0.725, 0.75)`, scale `(1.2, 0.05, 0.6)`. Its top surface is at `y = 0.75`.
6. **File → Build Profiles → Scene List → Add Open Scenes**, and make sure
   `Week08_Feedback` is ticked.

### Step 2: Make a grabbable cube

1. **GameObject → 3D Object → Cube**, name it `FeedbackCube`, position `(0, 0.825, 0.75)`,
   scale `(0.15, 0.15, 0.15)`. It rests on the table.
2. **Add Component → Rigidbody**.
3. **Add Component → XR Grab Interactable**.
4. Press **Play** and grab the cube with a simulated hand.

> **Checkpoint.** The cube follows your hand and drops when you let go. If it does not,
> work through
> [what a working grab is made of](../Guides/XRInteractionToolkit.md#example-what-a-working-grab-is-actually-made-of)
> before you go further.

### Step 3: Find the Interactable Events

1. Select `FeedbackCube` and scroll to the bottom of the **XR Grab Interactable** component.
2. Expand the **Interactable Events** foldout.
3. Find the **Select** heading. Under it are **Select Entered** and **Select Exited**.

**Select Entered** runs when an Interactor starts selecting the cube. For a grab, that is
the moment a hand picks it up. **Select Exited** runs when the hand lets go.

Each event is a list of methods to call, the same kind of list as **On Click ()** on the
Week 3 Button. You press `+`, drag in an object, and choose one of its methods. When the
event fires, every method in the list runs, in order.

The foldout has more headings than **Select**. Three of them are useful this week:

| Heading | Events | Runs when |
|---|---|---|
| **Hover** | **Hover Entered**, **Hover Exited** | An Interactor starts or stops hovering over the object, for example a ray pointing at it |
| **Select** | **Select Entered**, **Select Exited** | An Interactor starts or stops selecting the object. For an `XRGrabInteractable`, that is a grab and a release |
| **Activate** | **Activated**, **Deactivated** | The player presses or releases the trigger while holding the object |

The **First/Last** headings run only for the first Interactor to arrive and the last to
leave, which matters when two hands hold one object.

### Step 4: Visual feedback

1. In `Assets/Materials/`, creating the folder if it does not exist, create two materials
   named `CubeIdle` and `CubeHeld`, in colours you can tell apart.
2. Add the **MaterialSwapper** script from Week 6 to `FeedbackCube`. Set **Target Object** to
   `FeedbackCube`, **Material A** to `CubeIdle` and **Material B** to `CubeHeld`.
3. Under **Select Entered**, press `+`. Drag `FeedbackCube` into the object slot, then choose
   **MaterialSwapper → SwapToMaterialB ()** from the function dropdown.
4. Under **Select Exited**, press `+`, drag in `FeedbackCube`, and choose
   **MaterialSwapper → SwapToMaterialA ()**.
5. Play. Grab the cube and it changes to `CubeHeld`. Let go and it changes back.

### Step 5: Audio feedback

1. Select `FeedbackCube` and **Add Component → Audio Source**.
2. Drag a short clip into the **Audio Generator** field. The VR template includes one at
   `Assets/VRTemplateAssets/Audio/Button_22_click.wav`.
3. Untick **Play On Awake**. Leave **Loop** unticked.
4. Drag **Spatial Blend** all the way to **3D**, a value of `1`.
5. Expand **3D Sound Settings**. Set **Min Distance** to `0.3` and **Max Distance** to `10`.
6. On the **XR Grab Interactable**, under **Select Entered**, press `+`, drag in
   `FeedbackCube`, and choose **AudioSource → Play ()**.
7. Play and grab the cube. The clip plays each time you pick it up.

**Select Entered** now has two entries.

> **Spatial Blend decides where a sound comes from.** At `0` the clip plays at the same
> volume in both ears wherever the cube is. At `1` its volume and direction depend on where
> the cube is relative to the rig's **Main Camera**, under `Camera Offset`, which carries
> the scene's **Audio Listener**.

Inside **Min Distance** the clip plays at full volume. Further away it gets quieter, and it
stops getting quieter at **Max Distance**.

> **Leave Spatialize unticked.** It passes the sound to a spatialiser plugin, and this
> project does not have one installed.

### Step 6: Switch off the rig's own haptics

The Starter Assets rig already vibrates the controllers. Each Interactor on it has a
**`SimpleHapticFeedback`** component, and on the **Near-Far Interactor** it is set to send a
pulse whenever that hand starts hovering over or selecting anything. You have felt it in
every headset build since Week 5.

Left on, it fires at the same moment as the pulse you are about to add, and you cannot feel
the two separately.

1. Expand `XR Origin (XR Rig)` → `Camera Offset` → `Left Controller` and select
   **Near-Far Interactor**.
2. `Ctrl`-click the **Near-Far Interactor** under `Right Controller`, so both are selected.
3. Find the **Simple Haptic Feedback** component. Under **Select**, untick
   **Play Select Entered**. Under **Hover**, untick **Play Hover Entered**.

The component that drives the motor is the **`HapticImpulsePlayer`** on `Left Controller`
and on `Right Controller`. `SimpleHapticFeedback` sends its pulses through it, and so will
your script.

### Step 7: Haptic feedback

The vibration has to happen in the controller that grabbed the cube. The event tells your
script which controller that was.

Every Interactable event passes an argument describing the interaction. **Select Entered**
passes a `SelectEnterEventArgs`, and its `interactorObject` is the Interactor that did the
grabbing. The `HapticImpulsePlayer` is on that Interactor's parent controller.

1. In `Assets/Scripts/`, right-click → **Create → Scripting → MonoBehaviour Script**, and
   name it `HapticPulse`.
2. Replace its contents with this. A reference copy is in
   **[Scripts/HapticPulse.cs](Scripts/HapticPulse.cs)**.

```csharp
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

/// <summary>
/// Week 8, Activities 1 and 2. Sends one haptic impulse to the controller that selected this
/// object. Wire Pulse to Select Entered.
/// </summary>
public class HapticPulse : MonoBehaviour
{
    [Range(0f, 1f)]
    [Tooltip("How strongly the motor vibrates, from 0 to 1.")]
    public float amplitude = 0.5f;

    [Tooltip("How long the impulse lasts, in seconds.")]
    public float duration = 0.1f;

    public void Pulse(SelectEnterEventArgs args)
    {
        // The HapticImpulsePlayer is on the controller, which is a parent of the Interactor.
        var player = args.interactorObject.transform.GetComponentInParent<HapticImpulsePlayer>();

        // A tracked hand has no controller, so there is no HapticImpulsePlayer to find.
        if (player == null)
            return;

        player.SendHapticImpulse(amplitude, duration);
    }
}
```

`SendHapticImpulse` takes an amplitude from `0` to `1` and a duration in seconds.
`SelectEnterEventArgs` is in the `UnityEngine.XR.Interaction.Toolkit` namespace, and
`HapticImpulsePlayer` is in `UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics`.

3. Add `HapticPulse` to `FeedbackCube`. Leave **Amplitude** at `0.5` and **Duration** at
   `0.1`.
4. On the **XR Grab Interactable**, under **Select Entered**, press `+` and drag in
   `FeedbackCube`.
5. In the function dropdown, go to **HapticPulse**. The menu has two sections. Choose
   **Pulse** from the **Dynamic SelectEnterEventArgs** section.

**Select Entered** now has three entries: the material, the sound and the vibration.

> **Dynamic methods receive the event's argument.** A method under **Static Parameters**
> is called with a value you set in the Inspector, or with nothing. A method under
> **Dynamic** is called with whatever the event passes, which here is the details of the grab.

### Step 8: Test in the XR Interaction Simulator, then on the headset

**In the XR Interaction Simulator:**

1. Play and grab the cube. It changes colour and plays the clip.
2. You feel nothing, because the simulated controllers have no motor. Adjust the materials
   and the audio settings here, where each change takes seconds to test.

**On the headset:**

1. **File → Build Profiles → Android → Build And Run**. If Unity cannot see the headset,
   build the `.apk` and drag it onto **Device Manager** in **MQDH**.
2. Grab the cube with your right hand, then with your left. The controller you grab with
   vibrates, and the other one does not.
3. Point a ray at the cube without grabbing it. Nothing vibrates, because you unticked
   **Play Hover Entered** in Step 6.

Save the scene.

## Understanding interaction events

**An event does not know what is listening to it.** `XRGrabInteractable`
raises **Select Entered** without knowing what is listening. `MaterialSwapper`, `AudioSource`
and `HapticPulse` do not know about each other. Remove any one of them and the other two
still work.

**Every listener runs in the same frame.** They are called one after another when the event
fires, so the colour change, the sound and the vibration start together.

**The argument type depends on the event.** **Select Entered** passes a
`SelectEnterEventArgs`, **Select Exited** passes a `SelectExitEventArgs`, and
**Hover Entered** passes a `HoverEnterEventArgs`. A method only appears under **Dynamic** for
an event whose argument type it accepts, so `Pulse` is offered for **Select Entered** and not
for **Hover Entered**.

**Feedback can go on either side of an interaction.** Your listeners are on the
Interactable, so they belong to this cube. `SimpleHapticFeedback` is on the Interactor, so it
responds to everything that hand touches. XRI also has a **`SimpleAudioFeedback`**
component, which does the same for sound.

| | On the Interactable | On the Interactor |
|---|---|---|
| **Set up with** | The object's **Interactable Events** | `SimpleHapticFeedback`, `SimpleAudioFeedback` |
| **Applies to** | This object only | Everything that hand hovers over or selects |
| **Use it for** | Feedback about the object, such as what it is made of | Feedback that the hand has touched something |

**Other interaction components have the same events.** `XRSimpleInteractable`, the button in
Week 6 Activity 1, has the same **Interactable Events** foldout. `XRSocketInteractor` is an
Interactor, and its **Interactor Events** include **Select Entered** and **Select Exited**.

## Extension Activities

### **Feedback on hover**
Give the cube a lighter response when a hand hovers over it without grabbing: a quieter
sound and a shorter, softer pulse.

Logic: wire **Hover Entered** in the same foldout. For the sound, choose
**AudioSource → PlayOneShot (AudioClip)** and drag the VR template's
`Assets/VRTemplateAssets/Audio/Button_14_hover.wav` into the entry's parameter slot. `Pulse`
is not offered for **Hover Entered**, so add a second method to `HapticPulse` that takes a
`HoverEnterEventArgs`, with its own amplitude and duration fields for a weaker, shorter
impulse.

### **Wire the events from code**
An Interactable you add from a script with `AddComponent` has no Inspector entries, so its
listeners have to be added in code.

```csharp
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
```

Logic: get the `XRGrabInteractable` in `Awake`. In `OnEnable`, call
`grabInteractable.selectEntered.AddListener(OnSelectEntered)`. In `OnDisable`, call
`RemoveListener` with the same method. `OnSelectEntered` takes a `SelectEnterEventArgs`.
A listener you add and never remove keeps being called after its component is disabled.

### **Use XRI's feedback components instead**
Duplicate the cube and remove all three listeners from the copy. On both Near-Far
Interactors, tick **Play Select Entered** again, and add a **Simple Audio Feedback** component
with a **Select Entered Clip**. Grab each cube. Note what changes when the feedback belongs
to the hand rather than to the object, including where the sound comes from. Untick
**Play Select Entered** on both Near-Far Interactors again before Activity 2.

## Headset checkpoint

Before you move on to Activity 2:

- **Grab** the cube with each hand. The colour, the sound and the vibration start together,
  and only the hand that grabbed vibrates.
- **Hover** over the cube with a ray without grabbing, and confirm nothing vibrates.
- **Move back** about 2 m, walking or with the left thumbstick, and grab the cube with the
  ray. The clip is quieter and comes from the direction of the cube.

## Outcome
A cube that changes colour, plays a sound and vibrates the controller that grabbed it, all
wired to its **Select Entered** event. Activity 2 uses this scene to tune those responses.
Save the scene and commit.

## References

**XR Interaction Toolkit**
- `XRGrabInteractable`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable.html>
- `SelectEnterEventArgs`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs.html>
- `HapticImpulsePlayer`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticImpulsePlayer.html>
- `SimpleHapticFeedback`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Feedback.SimpleHapticFeedback.html>
- `SimpleAudioFeedback`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Feedback.SimpleAudioFeedback.html>

**Unity**
- Audio Source: <https://docs.unity3d.com/6000.3/Documentation/Manual/class-AudioSource.html>
