# Week 4: Interactive Object Systems

Week 4 introduces interaction systems: proximity-based reactions, object selection, and
physics-based object manipulation. It builds on the fundamentals from the previous weeks
and combines them into more complex mechanics. You hand-build *notice me*, *hold me* and
*release me* here so that when the XR Interaction Toolkit (XRI) does all three for you in
Week 6, you already know what it is doing.

## Before You Start

- **Unity 6.3 LTS (`6000.3.x`)** with the **Universal 3D (URP)** template — see [Software and Frameworks](../Guides/Software_and_Frameworks.md)
- The [Week 2](../Week%2002/README.md) and [Week 3](../Week%2003/README.md) activities finished — you carry the skills forward, not the project. Week 4 starts a new project, created inside your course repository, and both activities build on it
- Your Unity project created **inside** your course repository — see the [Unity + GitHub Course Guide](../Guides/Unity_GitHub_Course_Guide_V1.pdf)
- Commit your work as you go. Each activity is a sensible commit.

> **Keyboard shortcuts.** Where these activities say `Ctrl`, macOS users press `Cmd` —
> `Ctrl+S` becomes `Cmd+S`, `Ctrl+D` becomes `Cmd+D`.

> **No XR yet.** Everything this week runs on a keyboard in a flat 3D scene. The headset
> work starts in Week 5.

## Learning Progression

1. **Proximity Reactions** - Measuring distance to the player, and acting on the moment they
   arrive rather than every frame they stay
2. **Selection and Physics** - Holding an object, deciding whether physics or your code owns
   its position while you hold it, and handing it back on release

## Activities

Work through these in order — Activity 2 continues in the scene Activity 1 leaves you with.

- **[Activity 1](Activity%201%20-%20Proximity%20Reactions.md)** - Proximity-Based Object Reactions
  - Proximity detection using Vector3.Distance
  - Object highlighting and visual feedback systems
  - Finding another object by tag, and when to wire a reference instead
  - Movement, sound, and visual effects extensions

- **[Activity 2](Activity%202%20-%20Object%20Selection%20and%20Throwing.md)** - Object Selection and Throwing
  - Object selection and interaction mechanics
  - Object parenting and transform management
  - Why a parented object still falls, and what `isKinematic` does about it
  - Physics-based throwing, and reading the `Interact` action to manage held state

## C# Scripts

Reference copies of the scripts written across these activities are in the `Scripts/`
directory. Write your own first — these are for checking against, not for pasting.

- **[ProximityHighlighter.cs](Scripts/ProximityHighlighter.cs)** - Basic proximity highlighting system
- **[ProximityMover.cs](Scripts/ProximityMover.cs)** - Movement-based proximity reactions
- **[ProximitySound.cs](Scripts/ProximitySound.cs)** - Audio feedback for proximity changes
- **[ProximityEffects.cs](Scripts/ProximityEffects.cs)** - Visual effects and transformations
- **[Grabbable.cs](Scripts/Grabbable.cs)** - Object interaction and physics setup
- **[PlayerGrabber.cs](Scripts/PlayerGrabber.cs)** - Basic object grabbing system
- **[PlayerGrabberRaycast.cs](Scripts/PlayerGrabberRaycast.cs)** - Raycast-based object selection
- **[PlayerGrabberVelocity.cs](Scripts/PlayerGrabberVelocity.cs)** - Velocity-based throwing mechanics
- **[SimpleWASD.cs](Scripts/SimpleWASD.cs)** - Player movement and rotation controls

## Outcome

By the end of Week 4 you will have a scene whose objects notice you coming, a player who can
pick one up and carry it, and a throw that hands the object back to the physics engine — all
driven by the same **Input System** actions you met in Week 2, and all committed to your
repository.
