using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

/// <summary>
/// Week 8, Activities 1 and 2. Sends one haptic impulse to the controller that selected this
/// object. Wire Pulse to Select Entered.
/// </summary>
public class HapticPulse : MonoBehaviour
{
    [Range(0f, 1f)]
    [Tooltip("How strongly the motor vibrates, from 0 to 1.")]
    public float amplitude = 0.5f;

    [Tooltip("How long the impulse lasts, in seconds.")]
    public float duration = 0.1f;

    public void Pulse(SelectEnterEventArgs args)
    {
        // The HapticImpulsePlayer is on the controller, which is a parent of the Interactor.
        var player = args.interactorObject.transform.GetComponentInParent<HapticImpulsePlayer>();

        // A tracked hand has no controller, so there is no HapticImpulsePlayer to find.
        if (player == null)
            return;

        player.SendHapticImpulse(amplitude, duration);
    }
}
