# Activity 4: Sockets

> **Headset badge: simulator-friendly.** Build and test all of this in the **XR Interaction
> Simulator**. There is a headset checkpoint at the end.

## Objective
Build a socket that accepts an object and snaps it home, restrict a second socket so it
accepts only one thing, then turn a socketed object on the spot by driving the socket's
attach transform.

## Prerequisites
- **Activity 1** complete. You need the scene, the table, `Prop` and `Mallet`. Activities 2
  and 3 are not required, though the scene you open is the one they left behind
- **[XR Interaction Toolkit — Core Concepts](../Guides/XRInteractionToolkit.md#socket-interactor)**,
  the *Socket interactor* section
- **Packages you'll add this week:** none
- Hardware: none required to build this. A **Meta Quest 2 / 3 / 3S** for the closing
  checkpoint

> **Keyboard shortcuts.** Where this activity says `Ctrl`, macOS users press `Cmd`.

## Instructions

### Step 1: A socket to drop things into

An `XRSocketInteractor` is an Interactor that never moves and takes no input. It selects
whatever Interactable is released inside its trigger Collider.

1. Open `Week06_Interactions`.
2. **GameObject → 3D Object → Cube**, name it `Plinth`, position `(-0.3, 0.76, 0.55)`,
   scale `(0.14, 0.02, 0.14)`. A low pad resting on the table, at the front-left.
3. **GameObject → Create Empty**, name it `Socket`, position `(-0.3, 0.82, 0.55)`. It sits
   six centimetres above the plinth.
4. **Add Component → Sphere Collider**. Set **Radius** to `0.12` and tick **Is Trigger**.
   A socket finds Interactables through `OnTriggerEnter`. Without a trigger Collider it
   finds nothing.
5. **Add Component → XR Socket Interactor**.
6. Press **Play**, grab `Prop`, carry it over the plinth and let go. It snaps to the socket.
   Grab it again and it comes back out.

Carry `Mallet` over instead and the socket takes that too. A socket accepts any
`XRGrabInteractable` whose **Interaction Layer Mask** overlaps its own, and both are on
`Default`. Step 3 narrows that.

> **The socket will not take it while you are still holding it.** A socket refuses any
> Interactable another Interactor has selected. It hovers the object while you hold it over
> the plinth, and selects it the moment you release.

> **It refuses for a second after you pull something out.** That is **Recycle Delay Time**,
> which defaults to `1`. Without it, an object lifted out of a socket is instantly taken
> back by it.

### Step 2: Show what will happen before it happens

`XRSocketInteractor` draws a preview mesh of the hovered object at the place it would end
up, so a player can see where it will land before letting go.

1. On the **XR Socket Interactor**, confirm **Show Interactable Hover Meshes** is ticked.
2. Play, and carry `Prop` towards the plinth without releasing. A ghost cube appears in the
   socket, at the size and orientation the real one will take.
3. **Hover Scale** sets how big that ghost is drawn. Set it to `0.8` and the preview is
   smaller than the object will be once placed.
4. **Hover Mesh Material** and **Can't Hover Mesh Material** are the materials the ghost is
   drawn with. Leave them empty and XRI creates defaults. The second one applies in the
   next step, where a socket starts refusing things.

### Step 3: A socket that refuses

Sockets are usually specific. A keyhole takes a key, a battery bay takes a battery.
**Interaction Layer Masks** are how you say so.

Every Interactor and every Interactable carries a mask, and the two interact only when
their masks share a layer. Both halves have to be set, your hands included.

1. **Edit → Project Settings → XR Plug-in Management → XR Interaction Toolkit →
   Interaction Layer Settings**. Add a layer named `Tools` in the first empty slot.
2. **GameObject → 3D Object → Cube**, name it `ToolPlinth`, position `(0.3, 0.76, 0.55)`,
   scale `(0.14, 0.02, 0.14)`.
3. **GameObject → Create Empty**, name it `ToolSocket`, position `(0.3, 0.82, 0.55)`. Give
   it a **Sphere Collider** with **Radius** `0.12` and **Is Trigger** ticked, then an
   **XR Socket Interactor**.
4. On `ToolSocket`, set **Interaction Layer Mask** to `Tools` alone.
5. Select `Socket` and set its **Interaction Layer Mask** to `Default` alone. An Interactor
   you add yourself starts on **Everything**, so until you do this the open socket takes
   the mallet too.
6. Select `Mallet` and set the **XR Grab Interactable**'s **Interaction Layer Mask** to
   `Tools` alone.
7. Expand `XR Origin (XR Rig)` → `Camera Offset` → `Left Controller` and select
   **Near-Far Interactor**. Tick `Tools` alongside `Default` in its **Interaction Layer
   Mask**, and do the same on `Right Controller`.
8. Play. `ToolSocket` takes the mallet and refuses the prop. `Socket` takes the prop and
   refuses the mallet.

> **Your hands have a mask as well.** Skip step 7 and the mallet cannot be picked up
> anywhere in the scene. It keeps its Collider and its Rigidbody. No Interactor on the rig
> carries the layer you moved it to.

> **The two defaults differ.** An Interactor you add yourself starts on **Everything**. The
> Starter Assets Interactors arrive narrowed to `Default`. Step 5 and step 7 correct for
> that.

> **A refused object gives no error.** It does not snap, and nothing appears in the Console.
> Set the hover materials on any socket a player has to understand.

### Step 4: Sockets that resize what they hold

A display stand often wants everything on it at one size.

1. Select `Socket` and find **Socket Scale Mode**. It has three values.

| Value | What it does |
|---|---|
| **None** | The object keeps the scale it arrived with. The default |
| **Fixed** | The object is set to **Fixed Scale** while socketed, and returns to its own scale when removed |
| **Stretched To Fit Size** | The object is scaled to fill **Target Bounds Size**, computed from its own bounds |

2. Set **Socket Scale Mode** to **Fixed** and **Fixed Scale** to `(0.1, 0.1, 0.1)`.
3. Play. Grow `ScaleProp` with two hands, then drop it on the plinth. It shrinks to the
   display size. Lift it out and it returns to the size you grew it to.

### Step 5: A plinth that turns

While an object is socketed the socket is selecting it, and a grab transformer drives it to
the socket's attach transform every frame. Anything you write to the object's own
`transform` is overwritten before the frame is drawn. Move the transform it is being driven
towards instead.

1. Select `Socket`, right-click it → **Create Empty**, and name the child `Attach Point`.
   Leave it at local position `(0, 0, 0)`.
2. Drag `Attach Point` into the **XR Socket Interactor**'s **Attach Transform** field.
3. Create the script: right-click in `Assets/Scripts/` → **Create → C# Script**, name it
   **SocketTurntable**, and open it.

`XRSocketInteractor` lives in the Interactors namespace, not the root one:

```csharp
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
```

The work is one line, run each frame:

```csharp
        m_Attach.Rotate(m_Axis, m_DegreesPerSecond * Time.deltaTime, Space.Self);
```

Multiplying by `Time.deltaTime` makes the rate degrees *per second* rather than degrees per
frame. Without it the turntable spins faster on a better machine.

Read the attach transform in `Start`, not `Awake`. The socket creates an attach transform
for itself in its own `Awake` when the field is empty, and two `Awake` methods on one
GameObject run in an order you do not control.

> **The socket makes one if you do not.** Leave **Attach Transform** empty and XRI creates a
> child named `[Socket] Attach` at runtime, which is how Step 1 ran without one. Assign your
> own to see it in the Hierarchy and move it.

### Step 6: The complete script

A reference copy is in **[Scripts/SocketTurntable.cs](Scripts/SocketTurntable.cs)**. Write
yours first and compare.

```csharp
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Week 6, Activity 4. Turns a socket's attach transform, so that whatever the socket is
/// holding turns with it.
/// </summary>
[RequireComponent(typeof(XRSocketInteractor))]
public class SocketTurntable : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Degrees turned per second.")]
    float m_DegreesPerSecond = 30f;

    [SerializeField]
    [Tooltip("Axis to turn about, in the socket's own space.")]
    Vector3 m_Axis = Vector3.up;

    [SerializeField]
    [Tooltip("Turn only while the socket is holding something.")]
    bool m_OnlyWhenFilled = true;

    XRSocketInteractor m_Socket;
    Transform m_Attach;

    // Start rather than Awake: the socket creates its attach transform in its own Awake,
    // and this reads the result.
    void Start()
    {
        m_Socket = GetComponent<XRSocketInteractor>();
        m_Attach = m_Socket.attachTransform;
    }

    void Update()
    {
        if (m_OnlyWhenFilled && !m_Socket.hasSelection)
            return;

        // The socket holds what it has at this transform every frame, so turning the
        // transform turns the object.
        m_Attach.Rotate(m_Axis, m_DegreesPerSecond * Time.deltaTime, Space.Self);
    }
}
```

1. Add the script to `Socket`. The `RequireComponent` attribute means Unity refuses to add
   it to anything without an `XRSocketInteractor`.
2. Set **Degrees Per Second** to `30`, a full turn every twelve seconds. Leave **Axis** at
   `(0, 1, 0)` and **Only When Filled** ticked.
3. Play. Drop `Prop` on the plinth and it turns on the spot. Lift it off and the plinth
   stops.
4. Untick **Only When Filled** and play again. The attach transform now turns whether or not
   anything is in it, so an object dropped in joins a turn already in progress.

> **The object turns but drifts sideways.** The `Attach Point` is not at the socket's centre.
> Set its local position back to `(0, 0, 0)`.

## Understanding sockets

**A socket is an Interactor.** The `XRInteractionManager` matches it to Interactables the
way it matches your hands, and the same Interaction Layer Masks filter it. It differs in
never moving and reading no input. Nothing on `XRGrabInteractable` changed to make `Prop`
socketable. The object you built in Activity 1 works here untouched.

**The attach transform is a target.** Activity 1 used one to say *where the hand holds this
object*. The socket uses one to say *where an object placed here ends up*. Whatever
transformer is driving the object reads it every frame. In Step 5 you never moved the prop.
You moved the transform the prop was being driven towards.

**Writing to the object directly fails.** A socketed object has a transformer computing its
pose each frame, as a held one does. Setting `transform.rotation` on the object writes a
value that is overwritten before the frame is drawn. Activity 3 avoided that collision by
unticking **Add Default Grab Transformers**. Here nothing needs switching off, because the
turntable moves the transformer's target rather than its output.

## Extension Activities

### **A socket that fills itself**
Give the socket something at startup. Assign `Prop` to the **XR Socket Interactor**'s
**Starting Selected Interactable** field and the scene opens with the plinth occupied.
Useful for a tool that begins on its rack. It is the only way to fill a socket without a
player action.

### **Tell the player it landed**
Fire a response when the socket fills or empties. Logic: a socket has the same **Interactor
Events** any Interactor has, so wire **Select Entered** and **Select Exited** to
**[MaterialSwapper](Scripts/MaterialSwapper.cs)** on `Plinth` and let the pad change colour
while it is occupied. No new code.

### **Turn only what should turn**
The turntable turns anything the socket accepts, at one rate. Make the rate depend on the
object. Logic: read `m_Socket.firstInteractableSelected` in `Update`, look for a component
on it carrying a speed, and use that instead of the serialised field when one is present.

### **A rack of sockets**
Duplicate the plinth and socket four times along the back of the table, each on its own
interaction layer, with four props that each fit exactly one. This is the inventory-slot
pattern. The work is almost all layer mask settings.

### **Ease the snap**
Set **Attach Ease In Time** on `Prop`'s **XR Grab Interactable** to `0.15` and drop it into
the socket again. The object slides home instead of teleporting. That field lives on
the Interactable rather than on the socket. At exactly `0`, an object moving between a
socket and a hand can show a one-frame skip as it jumps to the new attach point.

## Headset checkpoint

Build to the headset (**File → Build Profiles → Build and Run**).

- **Sockets are easier to hit than they look on a monitor.** A radius of `0.12` that felt
  fussy with a mouse is usually generous with a tracked hand. Tune it down until placing
  something takes a little care.
- **Watch the turntable at real speed.** Thirty degrees per second looks slow on a screen
  and can look fast in the headset, where you are standing beside it.
- **Try to place something one-handed while holding something else.** A desk test never
  covers it.

## Outcome
A socket that accepts anything, a second that accepts one thing and shows you when it will
not, a display stand that resizes what it holds, and a plinth that turns its contents
without your code ever touching the object it turns.

## References
- `XRSocketInteractor`: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/api/UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor.html>
- Interaction Layers: <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.6/manual/interaction-layers.html>
