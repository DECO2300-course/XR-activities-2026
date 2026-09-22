# Activity 1: Augmenting Reality

> **Headset required.** Written for a **Meta Quest 3 or 3S**. On a Meta Quest 2, take the
> [Quest 2 route](#quest-2-route-capability-detection-and-a-stand-in-surface) instead.
>
> **Two packages added.** AR Foundation and Unity OpenXR: Meta change your manifest, your build
> size and your Project Validation list. If your prototype will never look at the real room,
> this is a dependency you do not need.
>
> **The first ten minutes go on the room, not the code.** The planes this activity reads do not
> exist until a human has walked around the room with the headset on and drawn them.
>
> `Ctrl` is `Cmd` on macOS. Nothing here needs Meta Quest Link: every headset instruction is
> *build an `.apk` through **File → Build Profiles** and deploy it with **Meta Quest Developer
> Hub***, which is sections 9 and 10 of
> [the OpenXR setup guide](../Guides/OpenXR_Unity_Setup_Guide.md).

---

## Objective

Make your virtual content aware of the real room. You will read the floor, the walls and a
tabletop out of the headset's room description, draw them so you can see what you found, cast a
ray from your controller at them, and place a virtual object where that ray lands, on your
actual desk at your actual desk height.

Two AR Foundation components do it: **`ARPlaneManager`**, which reports the surfaces the headset
knows about, and **`ARRaycastManager`**, which lets you hit them.

## Prerequisites

- **Weeks 1–8 finished.** A project that builds and runs on a headset, an **XR Origin (VR)** rig
  you assembled yourself, and at least one working `XRGrabInteractable`
- **Unity 6.3 LTS (`6000.3.x`)**, Android build target, OpenXR with the **Meta Quest Support**
  feature group already ticked, which is sections 3 and 5 of
  [the OpenXR setup guide](../Guides/OpenXR_Unity_Setup_Guide.md). This activity adds features
  *inside* that group; it does not set the group up for you
- **XRI vocabulary:** *Interactor*, *Interactable*, *XR Origin (VR)*. Reference:
  [XR Interaction Toolkit — Core Concepts](../Guides/XRInteractionToolkit.md)
- **A Meta Quest 3 or 3S**, charged, with a **USB-C cable that carries data**
- **A real room you can walk around in.** Two square metres of clear floor and one table is
  enough. A corridor or a seat in a full lab is not

> **Commit before you start.** Adding these packages rewrites `Packages/manifest.json` and
> `Packages/packages-lock.json`.

---

## Instructions

### Step 1: Run Space Setup on the headset

Do this before you open Unity.

1. Put the headset on and press the Meta button on your right controller.
2. Go to **Settings → Environment Setup → Space Setup** and press **Set up**.
3. Scan the room, then mark up the walls, the ceiling, the floor and each significant piece of
   furniture by hand. Give each item its correct label when the headset asks what it is.
4. Mark up the **table you intend to put a virtual object on**. Step 6 aims at that table, and
   an unmarked table is invisible to your app no matter how solid it looks to you.

Headset menus move between Horizon OS releases. If that path has changed, look for the option
that has you trace your walls and furniture.

> **Plane detection on this stack is a query against the device's Scene Model**, not a scan your
> app performs. The Scene Model is the room description *the user* created by running Space
> Setup, and your app gets back whatever the human drew.
>
> **If Space Setup has never been run, no planes appear and there is no error.** No exception, no
> warning, no red line in the log. `ARPlaneManager` is empty and your placement ray hits nothing.
>
> So the room is captured once rather than continuously, and a different room is a different
> Scene Model. Work done in the lab demos against nothing at home.

> **Checkpoint.** Space Setup has completed on this headset, in this room, and your target table
> is one of the things you marked. Imperfect is fine. Do not continue until this is true.

### Step 2: Add the packages

Back in Unity, open **Window → Package Manager** and install, from the Unity Registry:

- **AR Foundation** (`com.unity.xr.arfoundation`)
- **Unity OpenXR: Meta** (`com.unity.xr.meta-openxr`)

Take whatever version Package Manager offers. Expect three new lines in `manifest.json` rather
than two, because Unity OpenXR: Meta also pulls in XR Composition Layers.

> **Versions this activity was written against:** AR Foundation **6.5.0**, Unity OpenXR: Meta
> **2.5.0**, XR Composition Layers **2.1.1**, Unity OpenXR Plugin **1.15.1**. Nearby versions
> are fine, and small differences are not worth chasing. If something behaves oddly, moving to
> these is the first thing to try.

AR Foundation is the vendor-neutral half, defining `ARPlaneManager`, `ARRaycastManager` and the
rest as an interface with no idea what hardware is underneath. Unity OpenXR: Meta is the
*provider* that fills it in on a Quest. You need both, and they should be on matching major
versions.

Then go to **Edit → Project Settings → XR Plug-in Management → Project Validation**, **Android**
tab, and work top to bottom until there are no red errors. New packages add new validation
rules, and this panel is the fastest diagnostic you have.

`Ctrl+S` to save, and commit the manifest files.

### Step 3: Enable the features

**Edit → Project Settings → XR Plug-in Management → OpenXR**, **Android** tab, under the **Meta
Quest** feature group.

**Features are enabled individually.** Ticking the feature group is not the same as ticking the
things inside it. Tick:

- **Meta Quest: Session**
- **Meta Quest: Planes**
- **Meta Quest: Raycasts**
- **Meta Quest: Anchors**
- **Meta Quest: Camera (Passthrough)**, so you see the real room behind your content rather
  than a black void

There is no feature called "Passthrough" on its own. The camera feature is the passthrough
feature.

> **Unsupported features self-disable. They do not error.** If a feature is not supported by the
> headset that ends up running your build, it switches itself off and the app carries on. There
> is no exception to catch and nothing in the log to search for. **The failure mode is silent
> absence**: the manager is there, it is running, and it never produces anything.
>
> When a feature does nothing, ask whether it is switched on, on this device, before asking
> what is wrong with your code.

### Step 4: Build the scene

Open a new scene, or a copy of your Week 5 scene, and save it as `SceneUnderstanding.unity`.
Four things go in the Hierarchy:

1. **An `AR Session`** at the root of the scene, from **GameObject → XR → AR Session**. It
   starts and stops the AR subsystems; without it the managers have nothing to talk to
2. **Your rig**, an **XR Origin (VR)**, with its Tracking Origin Mode on **Floor**
3. **The managers**, added as components on that XR Origin itself:
   **XR → AR Foundation → AR Plane Manager** and **XR → AR Foundation → AR Raycast Manager**.
   Both require an `XROrigin` on the same GameObject, so a child will not do
4. **An `AR Camera Manager`** (**XR → AR Foundation → AR Camera Manager**) on the rig's
   **Main Camera**. Then set that camera's **Background Type** to **Solid Color** and the
   colour's **alpha to 0**. Enabling or disabling this component is how you turn passthrough on
   and off at runtime

> **Build it now, before you add anything else.** You should see your room rather than a black
> void. If you see black, the cause is one of three settings: **Meta Quest: Camera
> (Passthrough)** from Step 3, the **AR Camera Manager** component, or the camera's background
> alpha.

Now give `ARPlaneManager` something to draw. Its **Plane Prefab** field is optional, and
anything you put there gets an `ARPlane` component added automatically. Fastest route:
**GameObject → XR → AR Default Plane**, saved as a prefab. Rolling your own is a flat quad with
a semi-transparent, brightly coloured, unlit material. Garish is correct here, because you are
trying to see the shape of the data rather than ship a look.

Its other field is **Detection Mode**, which decides the kinds of plane it reports.

> **The scene permission, or none of this runs.** Planes, bounding boxes, meshes and occlusion
> all need the Android permission `com.oculus.permission.USE_SCENE`, granted at runtime. Unity
> adds it to your manifest, but a manager that starts before the grant does nothing.
>
> **The prompt appears inside the headset, once, on the first run.** Dismiss it and your app
> gets zero planes with no error, which looks exactly like a room that was never set up. If you
> think you dismissed it, uninstall the app from the headset and deploy again.

So start both managers **unticked**, request the permission on the first frame, and tick them
when it is granted. Reference copy: **[Scripts/ScenePermission.cs](Scripts/ScenePermission.cs)**.

```csharp
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR.ARFoundation;

public class ScenePermission : MonoBehaviour
{
    const string k_ScenePermission = "com.oculus.permission.USE_SCENE";

    [SerializeField] ARPlaneManager planeManager;
    [SerializeField] ARRaycastManager raycastManager;

    void Start()
    {
        if (Permission.HasUserAuthorizedPermission(k_ScenePermission))
        {
            Enable();
            return;
        }

        var callbacks = new PermissionCallbacks();
        callbacks.PermissionGranted += _ => Enable();
        Permission.RequestUserPermission(k_ScenePermission, callbacks);
    }

    void Enable()
    {
        planeManager.enabled = true;
        raycastManager.enabled = true;
    }
}
```

### Step 5: See what the room gave you

Build to the headset with **File → Build Profiles → Android → Build and Run**, or drag the
`.apk` onto **Meta Quest Developer Hub**. Put it on and look around. You should see coloured
rectangles pasted over your floor, your walls and your table. That is the Scene Model, rendered.

Add a script so you get a number rather than an impression, and read it in **Meta Quest
Developer Hub**'s log view. Reference copy: **[Scripts/PlaneReport.cs](Scripts/PlaneReport.cs)**.

```csharp
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneReport : MonoBehaviour
{
    [SerializeField] ARPlaneManager planeManager;

    void Update()
    {
        // trackables is the live collection of planes the runtime currently knows about.
        Debug.Log($"Planes: {planeManager.trackables.count}");
    }
}
```

To be told when things change rather than polling every frame, subscribe instead:

```csharp
void OnEnable()  => planeManager.trackablesChanged.AddListener(OnPlanesChanged);
void OnDisable() => planeManager.trackablesChanged.RemoveListener(OnPlanesChanged);

void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
{
    Debug.Log($"+{args.added.Count} planes, {args.updated.Count} updated, " +
              $"{args.removed.Count} removed");
}
```

Either way, the concept to keep is that **planes arrive over time and can be added, updated and
removed**. Code that reads the plane list once in `Start()` finds it empty and concludes the
room is featureless.

**Planes are classified.** Each carries `classifications`, saying what kind of surface the user
said it was, which is how you write "put this on a table but never on a wall". It is a **flags**
value of type `PlaneClassifications`, so test it with a bitwise and rather than `==`; the
singular `classification` member is deprecated. On Meta the values filled in are Table, Couch,
Floor, Ceiling, WallFace, WallArt, DoorFrame, WindowFrame, InvisibleWallFace, InnerWallFace and
Other.

> **Meta does not classify planes as horizontal or vertical.** `PlaneAlignment` is worked out
> by the package from the plane's own pose, not reported by the runtime.

> **Checkpoint.** Rectangles on the floor, on at least one wall, and on your target table, and a
> plane count above zero in the log. If the count is zero, go back to Step 1, which is where the
> fault usually is.

### Step 6: Raycast at a plane and place something on it

Point a controller at your real table, press a button, and a virtual object lands on it. Make a
small prefab to place, a cube 10 cm on a side is plenty, and write this. Reference copy:
**[Scripts/PlaceOnPlane.cs](Scripts/PlaceOnPlane.cs)**, which carries Step 7 commented out.

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField] ARRaycastManager raycastManager;
    [SerializeField] Transform rayOrigin;              // a controller transform on your rig
    [SerializeField] GameObject prefabToPlace;
    [SerializeField] InputActionReference placeAction; // e.g. the Select action from XRI

    // Reused every cast so we are not allocating a new list each time.
    static readonly List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void OnEnable()
    {
        placeAction.action.Enable();
        placeAction.action.performed += OnPlace;
    }

    void OnDisable()
    {
        placeAction.action.performed -= OnPlace;
    }

    void OnPlace(InputAction.CallbackContext context)
    {
        var ray = new Ray(rayOrigin.position, rayOrigin.forward);

        if (!raycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            return;   // pointing at nothing the room knows about

        // Hits come back sorted nearest-first.
        var hitPose = s_Hits[0].pose;
        Instantiate(prefabToPlace, hitPose.position, hitPose.rotation);
    }
}
```

Assign the four fields in the Inspector: the `ARRaycastManager` from your XR Origin, a controller
transform to cast from, your cube prefab, and an input action for the press.

> **Two failure modes that look identical from inside the headset.** Nothing appears because
> the ray missed, or because the action never fired. Log a line at the top of `OnPlace` before
> you debug anything else. If it never prints, the problem is input and has nothing to do with
> AR Foundation.

Two reusable ideas in that code:

- **Raycasting against planes is not `Physics.Raycast`.** Detected planes are *trackables*, not
  colliders, and Unity's physics system knows nothing about them. Making the room collide with
  physics objects needs mesh or bounding-box data, which this activity does not add
- **`TrackableType` is a filter, and it is where intent lives.** Hits inside a plane's polygon
  rather than against its infinite extension is the difference between "on the table" and
  "floating three metres past the edge of the table"

**Try it.** Place a cube on the floor, then the table, then a wall, where it arrives lying flat
because you used the hit's rotation as well as its position. Then point at an unmarked chair and
notice nothing happens.

> **Checkpoint.** A virtual cube is sitting on your real table at your real table height, and
> you put it there by pointing at it.

> **Two limits on this provider.** The screen-point overload of `Raycast` does nothing on Meta,
> so cast a world-space `Ray`. And `PlaneWithinPolygon` is performed by AR Foundation rather
> than by Meta's runtime, so it needs an enabled `ARPlaneManager` to have anything to test
> against.

### Step 7: Filter by classification

Restrict placement so the cube only lands on a table, and refuses walls and floor. In `OnPlace`,
between the cast and the `Instantiate`:

```csharp
        // s_Hits is the list Step 6 declared, and hits come back sorted nearest-first.
        if (s_Hits[0].trackable is not ARPlane plane)
            return;

        if ((plane.classifications & PlaneClassifications.Table) == 0)
            return;
```

The bitwise and matters: a plane can carry more than one classification, so `==` would reject
a table that is also marked as something else.

Build it, point at the floor and at a wall, and confirm the cube refuses both. Then commit
`SceneUnderstanding.unity` and `PlaceOnPlane.cs`.

---

## Understanding the Scene Model

- **The planes are a query, not a scan.** Phone tutorials describe plane detection as
  *discovery*, with flat regions emerging as the user waves the device around. Nothing like that
  happens here.

| | Discovery model (what tutorials assume) | Query model (what you have) |
|---|---|---|
| Where the data comes from | Your app, while running | Space Setup, before your app ran |
| Gets better if the user looks around | Yes | No |
| Missing surface means | Not looked at yet, so keep waiting | Never marked up, so waiting will not help |
| Correct response to "no planes" | Prompt the player to scan | Prompt the player to run Space Setup |
| Moving a real table mid-session | Eventually noticed | Not noticed |

- **Write a first-run check.** If your app finds zero planes, say so on screen, in words, and
  tell the player to run Space Setup. It is the difference between an app that appears broken
  and an app that explains itself.

- **The passthrough layer is not something you can read.** The Meta runtime submits it as a
  **composition layer**, at default order `-1`, behind everything your app renders. So you
  **cannot** run computer vision over it, sample the real world's colour from it, read a marker
  or QR code out of it, or screenshot the real world by grabbing the framebuffer.

- **Camera images are a different path, and it does exist.** Unity OpenXR: Meta 2.5 adds image
  capture through `ARCameraManager` in CPU and GPU forms, and the CPU form is meant for exactly
  that pixel work. It is opt-in and carries conditions:
  - Tick **Camera Image Support** in the gear-icon settings beside **Meta Quest: Camera
    (Passthrough)**
  - **CPU capture** needs a **Minimum API Level** of **Android 12L (API level 32)**
  - **GPU capture** needs **Vulkan**, and does not work over Meta Quest Link
  - A GPU image is valid for the current frame only, one at a time, acquired and released inside
    the render pipeline's camera callbacks
  - Enabling it adds camera permissions to your manifest

- **What you get instead is geometry**: planes, bounding volumes, meshes, depth, anchors. For
  putting a thing on a table, hiding a thing behind the couch or bouncing a ball off a real
  wall, geometry is the right answer and pixels were never needed.

> **If your prototype's idea rests on reading camera images, raise it with your tutor early.**
> The path exists, and it is a piece of work in itself.

### Which manager gives you what

| Feature | Manager | What it gives you |
|---|---|---|
| Camera (passthrough) | `ARCameraManager` | Passthrough control, and the route to camera images. Never the passthrough layer's own pixels |
| Planes | `ARPlaneManager` | Flat surfaces, classified |
| Bounding boxes | `ARBoundingBoxManager` | The *volume* of furniture rather than its flat top. Not in the Add Component menu, so type its name |
| Meshing | `ARMeshManager` | A triangle mesh of the room, for when a rectangle is too crude |
| Occlusion | `AROcclusionManager` | Depth information |
| Ray casts | `ARRaycastManager` | Hits against trackables |
| Anchors | `ARAnchorManager` | Objects that stay put in the real room across sessions rather than drifting relative to your rig |
| Colocation discovery | Meta Quest: Colocation Discovery | Shared space between headsets |

Learning this mapping tells you which manual page you need before you know what the component is
called.

---

## Quest 2 route: capability detection and a stand-in surface

A **Meta Quest 2** is a generation behind: low-resolution greyscale passthrough, no depth
sensing. Because unsupported features **self-disable rather than error**, you cannot find out
what is missing by reading the console. You have to ask the device what it supports, then build
something that copes with the answer.

### Part A: Report what the device supports

Do **Steps 1 to 4** above exactly as written. None of that is Quest 3 specific. Then write a
capability report that turns silent absence into text on a screen:

```csharp
// For each manager you added, ask three questions and print all three:
//   1. Is the component present in the scene at all?
//   2. Did its subsystem get created, or is it null?
//   3. Is that subsystem running?
// A present component with a null or stopped subsystem IS the silent self-disable.
Debug.Log($"ARPlaneManager: component={planeManager != null}, " +
          $"subsystem={planeManager?.subsystem != null}, " +
          $"running={planeManager?.subsystem?.running}");
```

A manager also exposes a `descriptor`, the supported way to ask what a provider can do. On Meta,
`raycastManager.descriptor.supportsViewportBasedRaycast` is false while
`supportsWorldBasedRaycast` is true, which is Step 6's limit stated as data rather than prose.

Put the results on a world-space canvas as well as in the log. Build it, run it, and **write
down which lines came back false**. If you can, run the same build on a Quest 3 or 3S and
compare the two reports.

### Part B: A stand-in surface, and the same placement code

Put a **stand-in surface** in the scene: a plain quad or thin box, roughly the size of your real
table, that the player positions by hand at the start of the session. Grab it with your Week 6
`XRGrabInteractable`, line it up by eye, and lock it in place. It is a manual calibration doing
the job Space Setup would have done for you.

Then run **Step 6's placement** against it.

> **Keep the placement code identical, and change only where the surface came from.** Route both
> paths, real planes and the stand-in, through the same "here is a surface, here is a pose,
> place the object" function.

Because your stand-in is ordinary scene geometry with a `Collider`, this path uses
`Physics.Raycast` rather than `ARRaycastManager`, which is Step 6's distinction seen from the
other side.

Build it, confirm the cube lands on the stand-in, then commit the scene and your scripts.

---

## Extension Activities

- **Write the "please run Space Setup" screen.** Detect zero planes after a grace period and
  show a world-space message in plain language, without the words *Scene Model*. Better, take
  them there: the session subsystem can request scene capture from inside your app. Then test it
  on somebody who has not read this activity.
- **Classify everything and colour it.** Tint each plane by its classification, unknown in a
  loud warning colour, then walk your room. The unknown ones tell you how much of your room the
  markup captured.
- **Snap to the nearest edge.** Place a cube flush against the nearest *edge* of the plane it
  hit. `plane.boundary` is a `NativeArray<Vector2>` in the plane's own space; turn a boundary
  point into a world position with
  `plane.transform.TransformPoint(new Vector3(p.x, 0f, p.y))`.
- **Anchor it so it survives.** Place a cube, anchor it, quit and relaunch. Two provider-specific
  things first: anchors attached to a plane are **not supported** on Meta, so create the anchor
  at a pose with `ARAnchorManager.TryAddAnchorAsync`; and saving one returns an identifier the
  provider will not list later, so store it yourself and pass it to `TryLoadAnchorAsync`. A
  cheaper route: a plane's `trackableId` is stable across sessions in the same space setup.
- **Measure your own room.** Print the floor area, ceiling height and each table's footprint,
  and compare against a tape measure. Where it is wrong, it is wrong because of how somebody
  drew the room.

---

## Headset checkpoint

Before you close this activity:

- **Space Setup** is done on the headset you tested with, in the room you tested in.
- **Your build reports a non-zero plane count** in that room.
- **You placed an object on a real surface** by pointing at it.
- **Your scene and scripts are committed.**
- **Run it in a second room**, one you have run Space Setup in yourself.

---

## Outcome

A build that reads the real room out of the headset's Scene Model, draws what it found, and
places virtual objects onto real surfaces with a raycast that respects what each surface is. In
your repository: `SceneUnderstanding.unity` and a placement script you wrote.

Three facts to keep, which hold after the component names have changed:

1. **The planes come from Space Setup, not from your app.** No planes, no error, so check the
   room before you check the code
2. **Passthrough is a composition layer, not a camera.** Camera images are a separate, opt-in
   path with its own conditions
3. **Unsupported features go quiet rather than loud.** Absence is the failure mode, so detecting
   capability is part of the work

---

## References

**Components**
- `ARPlaneManager`: <https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.5/api/UnityEngine.XR.ARFoundation.ARPlaneManager.html>
- `ARRaycastManager`: <https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.5/api/UnityEngine.XR.ARFoundation.ARRaycastManager.html>
- `ARAnchorManager`: <https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.5/api/UnityEngine.XR.ARFoundation.ARAnchorManager.html>

**Manual**
- Plane detection in AR Foundation: <https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.5/manual/features/plane-detection.html>
- Planes on Meta: <https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.5/manual/features/planes.html>
- Camera and passthrough on Meta: <https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.5/manual/features/camera.html>
- AR Foundation manual: <https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.5>
- Unity OpenXR: Meta manual: <https://docs.unity3d.com/Packages/com.unity.xr.meta-openxr@2.5>
- Unity OpenXR Plugin manual: <https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.16>
- Device comparison, to check what your headset supports:
  <https://developers.meta.com/horizon/resources/compare-devices/>
- Course stack reference: [Software and Frameworks](../Guides/Software_and_Frameworks.md)
- Setup and build workflow: [OpenXR Unity Setup Guide](../Guides/OpenXR_Unity_Setup_Guide.md)
