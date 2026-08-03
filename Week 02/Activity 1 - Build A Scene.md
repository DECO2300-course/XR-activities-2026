# Activity 1: Build a Scene from Primitives

## Objective
Learn Unity best practices while creating a 3D scene using primitive GameObjects. This scene will be used for the upcoming activities.

## Prerequisites
- Unity 6.3 LTS installed, and a course repository set up (see the [Guides](../Guides/README.md))
- Basic familiarity with Unity Editor windows (Scene, Game, Hierarchy, Inspector, Project)
- Understanding of basic camera controls in the Scene view

> **Keyboard shortcuts.** This course supports Windows and macOS. Where you see `Ctrl`,
> macOS users press `Cmd` instead — `Ctrl+S` becomes `Cmd+S`, `Ctrl+D` becomes `Cmd+D`.

## Instructions

### Step 1: Project Setup and Organization
1. Open Unity Hub and create a new project with the **Universal 3D** template

   > **Use Universal 3D, not the other templates.** Universal 3D uses the Universal Render
   > Pipeline (URP), which is the pipeline that runs on Quest headsets. The Built-In
   > pipeline is Unity's legacy renderer, and High Definition RP will not run on a Quest at
   > all. Everything you build this semester ends up on a headset, so start on the right
   > pipeline now — switching a project's render pipeline later is genuinely painful.

2. Create the project **inside your course repository**, not beside it
3. **Save your scene immediately** (Ctrl+S) and name it "Activity1_Scene"
4. **Create an organized folder structure** in the Project window:
   - Right-click in Project window → Create → Folder
   - Name it "Materials"
   - Create additional folders: "Scripts", "Textures"

### Step 2: Create Your Ground Plane
1. **Use the menu**: GameObject → 3D Object → Plane
2. **Rename it properly**: Right click the Plane in Hierarchy, select 'Rename' and call it "Ground_Plane"
3. **Position it correctly**: Select the Plane then in the Inspector, set Position to (0, 0, 0)
4. **Scale if needed**: Use the Scale tool or Inspector to adjust size

### Step 3: Add Scene Objects
1. **Create objects systematically**:
   - GameObject → 3D Object → Cube 
   - GameObject → 3D Object → Sphere
   - GameObject → 3D Object → Cylinder

2. **Name objects clearly and consistently**:
   - "Building_01", "Building_02" (for structures)
   - "Tree_01", "Tree_02" (for natural elements)
   - "Wall_North", "Wall_South" (for directional elements)

3. **Use the Inspector for precise positioning**:
   - Select an object and use the Position fields for exact placement
   - Use the Transform tools (Move, Rotate, Scale) for visual adjustments

### Step 4: Organize Your Hierarchy
1. **Group related objects**:
   - Create empty GameObjects as containers: GameObject → Create Empty
   - Name them "Buildings", "Trees", "Walls", etc.
   - Drag related objects into these containers
   - (This allows multiple objects to act as a single object)

2. **Use parent-child relationships**:
   - Select multiple objects
   - Drag them onto a parent object to group them
   - This keeps your Hierarchy organized and manageable

### Step 5: Apply Textures from Downloaded Assets
1. **Download the texture assets**:
   - Download the jpg images (brick.jpg, grass.jpg) from the repository
   - Place them in your project's "Textures" folder

2. **Apply textures to objects**:
   - Select an object in your scene
   - Drag the texture file from the Project window onto the object
   - Unity automatically creates a material using URP's **Lit** shader, with your texture
     as its Base Map
   - Repeat for different objects (brick texture for buildings, grass for ground)

3. **Organize your materials**:
   - Materials will be created automatically when you apply textures
   - Move them to your "Materials" folder for organization
   - Rename them descriptively: "Brick_Material", "Grass_Material"

### Step 6: Scene Validation
1. **Check your scene**:
   - Ensure all objects are properly named
   - Confirm your Hierarchy is organized
   - Make sure your camera is positioned to see your scene
   - Verify textures are applied correctly

2. **Save your work** (Ctrl+S)

### Step 7: Build Your Scene
Now use all the techniques you've learned to create a complete scene:

1. **Continue building systematically**:
   - Add more objects using the same naming conventions
   - Group related objects together in the Hierarchy
   - Apply textures to give your scene visual interest
   - Use precise positioning for professional-looking results

2. **Scene theme suggestions**:
   - **City**: Buildings, roads, streetlights, cars
   - **Forest**: Trees, rocks, paths, wildlife
   - **Maze**: Walls, corridors, dead ends, center point
   - **Space**: Planets, asteroids, space stations, stars
   - **Garden**: Flowers, benches, fountains, pathways
   - **Castle**: Towers, walls, gates, courtyards

3. **Remember the best practices**:
   - Name objects clearly and consistently
   - Organize your Hierarchy with parent objects
   - Use the Inspector for precise positioning
   - Save frequently as you build 


## Extension Activities

### **Scene Variations**
- Create different themed scenes (city, forest, maze, space)
- Experiment with different primitive combinations
- Try different scales and proportions
- Add more complex object arrangements

### **Advanced Organization**
- Use layers to organize objects
- Create prefabs for reusable objects
- Experiment with different material properties
- Add basic lighting to your scene

## Outcome
A well-organized 3D scene built entirely from primitive shapes, following Unity best practices. The scene should be ready for use in upcoming activities that will add scripts and behaviors to the objects. 