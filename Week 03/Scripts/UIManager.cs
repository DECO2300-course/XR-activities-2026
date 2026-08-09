using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI rotationDisplay;
    public TextMeshProUGUI positionDisplay;
    public TextMeshProUGUI speedDisplay;
    public TextMeshProUGUI statusDisplay;
    public DebugRotator targetRotator;

    void Update()
    {
        // Update rotation display
        rotationDisplay.text = "Rotation: " + targetRotator.currentRotation.ToString("F1");
        
        // Update position display
        positionDisplay.text = "Position: " + targetRotator.objectPosition.ToString("F2");
        
        // Update speed display
        speedDisplay.text = "Speed: " + targetRotator.rotationSpeed.ToString("F1");
        
        // Update status display
        statusDisplay.text = "Status: " + (targetRotator.isRotating ? "Rotating" : "Stopped");
    }
}
