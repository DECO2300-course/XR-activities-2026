# Week 2: Unity Fundamentals

Week 2 introduces students to Unity's core concepts through hands-on activities that build progressively from basic scene creation to interactive game mechanics.

## Before You Start

- **Unity 6.3 LTS** with the **Universal 3D (URP)** template — see [Software and Frameworks](../Guides/Software_and_Frameworks.md)
- Your Unity project created **inside** your course repository — see the [Unity + GitHub Course Guide](../Guides/Unity_GitHub_Course_Guide_V1.pdf)
- Commit your work as you go. Each activity is a sensible commit.

> **Keyboard shortcuts.** Where these activities say `Ctrl`, macOS users press `Cmd` —
> `Ctrl+S` becomes `Cmd+S`, `Ctrl+D` becomes `Cmd+D`.

## Learning Progression

1. **Scene Building** - Learn Unity's interface and create environments
2. **Object Movement** - Understand Transform components and basic scripting
3. **Input Systems** - Create player-controlled movement
4. **Collision Detection** - Build interactive game systems

## Activities

Work through these in order — each one builds on the scene and scripts from the last.

- **[Activity 1](Activity%201%20-%20Build%20A%20Scene.md)** - Build A Scene
  - Unity interface and best practices
  - Creating scenes from primitive objects
  - Asset organization and hierarchy management

- **[Activity 2a](Activity%202a%20-%20Rotate%20an%20Object.md)** - Rotate an Object
  - Basic C# scripting in Unity
  - Transform component manipulation
  - Understanding Update() and Time.deltaTime

- **[Activity 2b](Activity%202b%20-%20Move%20an%20Object.md)** - Move an Object
  - Object translation and movement
  - Vector3 and direction concepts
  - Combining rotation and movement

- **[Activity 3](Activity%203%20-%20Input%20Movement.md)** - Input Movement
  - Unity's Input System and action-based input
  - Player-controlled movement
  - WASD, arrow key, and gamepad controls from a single action

- **[Activity 4](Activity%204%20-%20Object%20Interactions.md)** - Object Interactions
  - Collision detection systems
  - Trigger colliders and event handling
  - Interactive game mechanics

## C# Scripts

Reference copies of the scripts written across these activities are in the `Scripts/`
directory. Write your own first — these are for checking against, not for pasting.

- **[Rotator.cs](Scripts/Rotator.cs)** - Continuous object rotation (Activity 2a)
- **[Mover.cs](Scripts/Mover.cs)** - Movement in a set direction (Activity 2b)
- **[PlayerMovement.cs](Scripts/PlayerMovement.cs)** - Input-driven player movement (Activity 3)
- **[CollisionHandler.cs](Scripts/CollisionHandler.cs)** - Trigger detection and collecting (Activity 4)

## A Note on Input

Unity 6 uses the **Input System** package, and every new project ships with an actions
asset already set up. You will read named actions like `Move` rather than checking
individual keys.

Older tutorials use `Input.GetAxis()` and `Input.GetKey()`. Those belong to Unity's legacy
Input Manager, which is switched off in new projects and will throw an error if you call
it. Activity 3 covers what to use instead — and the same action-based approach is what
drives VR controllers later in the semester.

## Outcome

By the end of Week 2 you will have a scene you built yourself, populated with objects that
move under their own power, a player you control, and objects that react when you reach
them — all in a project that is committed to your repository.
