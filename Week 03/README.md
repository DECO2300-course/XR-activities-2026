# Week 3: Reasoning About a Scene

Week 3 builds upon the fundamentals learned in Week 2, introducing more advanced Unity concepts including debugging, UI development, mathematical operations, raycasting, and XR preparation.

## Before You Start

- **Unity 6.3 LTS (`6000.3.x`)** with the **Universal 3D (URP)** template — see [Software and Frameworks](../Guides/Software_and_Frameworks.md)
- Your Unity project created **inside** your course repository — see the [Unity + GitHub Course Guide](../Guides/Unity_GitHub_Course_Guide_V1.pdf)
- The [Week 2](../Week%2002/README.md) activities finished — you carry the skills forward, not the project. Week 3 starts a new project, created inside your course repository, and Activities 1 to 4 all build on it
- Commit your work as you go. Each activity is a sensible commit.

> **Keyboard shortcuts.** Where these activities say `Ctrl`, macOS users press `Cmd` —
> `Ctrl+S` becomes `Cmd+S`.

## Learning Progression

1. **Editor Debugging** - Learn Unity's debugging tools and console output
2. **UI Development** - Create TextMeshPro UI elements and connect them to scripts
3. **Mathematical Operations** - Master vector operations, distance calculations, and angle measurements
4. **Raycasting** - Implement line-of-sight detection and interactive targeting systems
5. **XR Preparation** - Get a VR-template project running on a Meta Quest 2 / 3 / 3S, and change it

## Activities

All activities are located in the `Week 03/` directory:

- **[Activity 1](Activity%201%20-%20Editor%20Debugging.md)** - Introduction to Editor Debugging
  - Unity Console window and Debug.Log statements
  - Public variables and Inspector monitoring
  - Real-time script value modification


- **[Activity 2](Activity%202%20-%20Unity%20UI.md)** - Unity UI Elements
  - UI components and Canvas setup
  - Screen Space vs World Space UI canvases
  - UI Manager scripts and reference connections
  - Real-time UI updates from script values

- **[Activity 3](Activity%203%20-%20Unity%20Math.md)** - Unity Math and Vector Operations
  - Vector3.Distance calculations and angle measurements
  - LookAt function and direction vectors
  - Practical math applications with multiple objects
  - Real-time mathematical operations and UI display

- **[Activity 4](Activity%204%20-%20Raycasting.md)** - Raycasting in Unity
  - Physics.Raycast fundamentals and hit detection
  - Line-of-sight systems with visual debugging
  - Gizmos for raycast visualisation
  - Interactive object detection and targeting

- **[Activity 5](Activity%205%20-%20Getting%20Set%20Up%20for%20XR.md)** - Getting Set Up for XR
  - A separate project, made from Unity's **VR template**
  - Building it to a Meta Quest 2 / 3 / 3S and looking around inside it
  - Changing the template's world and building again
  - Follows the [OpenXR Unity Setup Guide](../Guides/OpenXR_Unity_Setup_Guide.md) throughout

## C# Scripts

The following Unity-compatible C# scripts are included in the `Scripts/` directory:

- **[DebugRotator.cs](Scripts/DebugRotator.cs)** - Debugging utilities for object rotation
- **[UIManager.cs](Scripts/UIManager.cs)** - User interface interaction and management
- **[ButtonHandler.cs](Scripts/ButtonHandler.cs)** - Button event handling and UI interactions
- **[UsefulMath.cs](Scripts/UsefulMath.cs)** - Mathematical operations and calculations
- **[SimpleRaycast.cs](Scripts/SimpleRaycast.cs)** - Line-of-sight and interaction systems

## Getting Started

1. **Prerequisites**: Completion of the Week 2 activities, in the same Unity project — see **Before You Start** above
2. **Setup**: Ensure your Unity project is properly configured for advanced development
3. **Activities**: Follow the activities in order for best learning progression
4. **Extensions**: Each activity includes optional extension challenges for advanced learners
