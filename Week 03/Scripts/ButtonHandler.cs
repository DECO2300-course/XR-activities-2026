using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public DebugRotator targetRotator;
    public Button toggleButton;

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleRotation);
    }

    void ToggleRotation()
    {
        targetRotator.isRotating = !targetRotator.isRotating;
    }
}
