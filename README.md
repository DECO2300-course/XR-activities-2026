# Digital Prototyping — XR Activities

This repository contains the weekly activities for Digital Prototyping in XR. The course
builds your Unity and XR skills from first principles, ending with interactive experiences
running on a Meta Quest headset.

## The stack

| | |
|---|---|
| Version control | GitHub Desktop |
| Engine | Unity 6.3 LTS (`6000.3.x`) |
| Project template | Universal 3D (URP) · **VR template** from Week 3 Activity 5 onward |
| Input | Unity Input System (project-wide actions) |
| XR runtime | Unity OpenXR Plugin |
| XR interaction | Unity XR Interaction Toolkit |
| Editor testing | XR Interaction Simulator |
| Headsets | Meta Quest 2 / 3 / 3S |
| Platforms | Windows and macOS |

The authoritative version list — including which Unity modules to install — is
**[Guides/Software_and_Frameworks.md](Guides/Software_and_Frameworks.md)**. If anything in
this repository disagrees with that document, that document wins.

## Guides

Set-up and workflow reference material. Read these before the weekly activities.

- **[Guides overview](Guides/README.md)** — what each guide covers and what order to read them in
- **[Software and Frameworks](Guides/Software_and_Frameworks.md)** — everything to install, with official documentation links
- **[Unity + GitHub Course Guide](Guides/Unity_GitHub_Course_Guide_V1.pdf)** — account setup through to the daily commit workflow
- **[XR Interaction Toolkit — Core Concepts](Guides/XRInteractionToolkit.md)** — the XRI vocabulary the XR weeks assume: Interactor, Interactable, Grab Transformer
- **[Unity_Course.gitignore](Guides/Unity_Course.gitignore)** — the `.gitignore` supplied for your course repository

> Install the supplied `.gitignore` **before** you create your Unity project. Adding it
> afterwards is considerably more painful.

## Weekly content

Activities are released weekly. This page is updated as each week goes live.

### Week 1: Introduction to Game Development

No content for this week.

### Week 2: Unity Fundamentals

Unity's core concepts through hands-on activities that build progressively from basic
scene creation to interactive game mechanics — scene building, Transform manipulation,
scripting, input, and collision detection.

**[View Week 2 Materials](Week%2002/README.md)**

### Week 3: Reasoning About a Scene

Debugging with the Console and the Inspector, TextMeshPro UI wired to live script values,
vector maths and raycasting — then a first XR project built to a headset from Unity's VR
template.

**[View Week 3 Materials](Week%2003/README.md)**

### Week 4: Interactive Object Systems

Objects that notice you coming, then selection, carrying and throwing. You hand-build
*notice me*, *hold me* and *release me* here, so that when the XR Interaction Toolkit does
all three for you in Week 6, you already know what it is doing.

**[View Week 4 Materials](Week%2004/README.md)**

### Week 5: Into the Headset

The week you build a small XR scene — a rig, a floor, objects measured in metres, something you 
can pick up — test it in the XR Interaction Simulator, then take it to a real headset and find 
out what the simulator could not tell you.
Two ideas carry the week: **the camera's position is an output, not an input**, and work that
has only ever run on a monitor is not finished work.

Read **[XR Interaction Toolkit — Core Concepts](Guides/XRInteractionToolkit.md)** before you
start. Activity 2 needs a Meta Quest 2 / 3S.

**[View Week 5 Materials](Week%2005/README.md)**

### Week 6: The Interaction Toolkit

The week you stop dropping in prefabs and start assembling interactions from their parts.
You build a grab Interactable out of a Collider, a Rigidbody and an `XRGrabInteractable`,
resize an object with two hands inside bounds you set, then write the code that moves a
held object yourself.

Read **[XR Interaction Toolkit — Core Concepts](Guides/XRInteractionToolkit.md)** before you
start; the activities use its vocabulary and do not re-explain it. All three are
simulator-friendly, and each ends with a headset checkpoint.

**[View Week 6 Materials](Week%2006/README.md)**

## Getting started

1. Work through **[Software and Frameworks](Guides/Software_and_Frameworks.md)** and install the stack
2. Follow the **[Unity + GitHub Course Guide](Guides/Unity_GitHub_Course_Guide_V1.pdf)** to create your course repository
3. Create your Unity project **inside** that repository, using the **Universal 3D** template.
   Each week starts a fresh project; from Week 3 Activity 5 onward the XR weeks use the
   **VR** template instead
4. Work through each week's activities in order — each builds on the last
5. Attempt the extension challenges; they are where most of the learning happens

If you hit a setup problem none of the guides answer, that's a gap in the guides — tell
your tutor.
