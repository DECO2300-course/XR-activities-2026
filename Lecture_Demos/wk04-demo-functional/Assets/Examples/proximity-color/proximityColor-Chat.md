# Proximity Color Change — Chat Transcript

## Me

I need a script for Unity 6.3 LTS that makes a cube blue when another cube gets close to it.

## Copilot

Here's a simple Unity C# script that changes a cube's color to blue when another cube comes within a specified distance.

### Script: `ProximityColorChange.cs`

```csharp
using UnityEngine;

public class ProximityColorChange : MonoBehaviour
{
    [Header("Target Cube")]
    public Transform otherCube;

    [Header("Settings")]
    public float triggerDistance = 3f;

    private Renderer cubeRenderer;
    private Color originalColor;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color;
    }

    void Update()
    {
        if (otherCube == null)
            return;

        float distance = Vector3.Distance(transform.position, otherCube.position);

        if (distance <= triggerDistance)
        {
            cubeRenderer.material.color = Color.blue;
        }
        else
        {
            cubeRenderer.material.color = originalColor;
        }
    }
}
```

### How to Use

1. Create two cubes in your scene.
2. Attach this script to the cube that should change color.
3. In the Inspector, drag the other cube into the **Other Cube** field.
4. Set **Trigger Distance** to the desired detection range (for example, `3`).
5. Play the scene.

### Alternative: Use a Trigger Collider

If you want better performance with many objects, you can use trigger colliders instead:

```csharp
using UnityEngine;

public class TriggerColorChange : MonoBehaviour
{
    private Renderer cubeRenderer;
    private Color originalColor;

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        originalColor = cubeRenderer.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cubeRenderer.material.color = Color.blue;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cubeRenderer.material.color = originalColor;
        }
    }
}
```

For the trigger version:

1. Add a **Box Collider** to the cube.
2. Check **Is Trigger**.
3. Tag the moving cube as `Player` (or change the tag check in the script).

This works in Unity 6.3 LTS just as it does in recent Unity versions.

---

## Me

Can you explain this to me at a high level?

## Copilot

Absolutely. At a high level, this script is doing three things every frame:

### 1. Remembering which cube to watch

```csharp
public Transform otherCube;
```

This creates a slot in the Inspector where you drag the second cube.

Think of it as:

> "This is the cube I want to monitor."

### 2. Measuring the distance between the cubes

```csharp
float distance = Vector3.Distance(transform.position, otherCube.position);
```

Every frame, Unity checks:

- Where is my cube?
- Where is the other cube?

Then it calculates the straight-line distance between them.

For example:

```text
My Cube       Other Cube
    ■ ----------- ■
        2.5m
```

If the cubes are 2.5 units apart, `distance` becomes `2.5`.

### 3. Deciding whether to change color

```csharp
if (distance <= triggerDistance)
{
    cubeRenderer.material.color = Color.blue;
}
else
{
    cubeRenderer.material.color = originalColor;
}
```

This is the decision-making part. The script asks:

> "Is the other cube close enough?"

If yes:

```text
distance = 2
triggerDistance = 3
```

Since 2 is less than 3: ✅ Turn blue.

If not:

```text
distance = 5
triggerDistance = 3
```

Since 5 is greater than 3: ✅ Return to the original color.

### What happens over time?

Unity runs `Update()` about once per frame:

```text
Frame 1 -> Check distance
Frame 2 -> Check distance
Frame 3 -> Check distance
...
```

So as you move the second cube around:

```text
Far Away
■                ■
Color: Normal

Gets Closer
■         ■
Color: Normal

Inside Range
■    ■
Color: Blue

Moves Away Again
■                ■
Color: Normal
```

### The Unity concepts involved

| Concept | What it does |
| --- | --- |
| **GameObject** | The cube in the scene. |
| **Transform** | Stores position, rotation, and scale. |
| **Renderer** | Draws the cube on screen. |
| **Material** | Controls what the cube looks like. |
| **`Update()`** | Runs every frame. |
| **`Vector3.Distance()`** | Calculates how far apart two objects are. |

So the overall logic is simply:

```text
Every frame:
    Measure distance to other cube

    If close enough:
        Turn blue
    Else:
        Use original color
```

This is a very common pattern in Unity, and it's a great beginner example because it introduces object references, positions, distances, and conditionals all in one script.
