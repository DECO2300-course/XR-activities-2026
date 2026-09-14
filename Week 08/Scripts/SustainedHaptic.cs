using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

/// <summary>
/// Week 8, Activity 2. Keeps the selecting controller vibrating for as long as this object is
/// held. Wire StartBuzz to Select Entered and StopBuzz to Select Exited.
/// </summary>
public class SustainedHaptic : MonoBehaviour
{
    [Range(0f, 1f)]
    [Tooltip("How strongly the motor vibrates, from 0 to 1.")]
    public float amplitude = 0.3f;

    [Tooltip("How long each impulse lasts, in seconds. Keep it longer than Interval.")]
    public float pulseLength = 0.1f;

    [Tooltip("Time from the start of one impulse to the start of the next, in seconds.")]
    public float interval = 0.08f;

    HapticImpulsePlayer player;
    Coroutine buzzRoutine;

    public void StartBuzz(SelectEnterEventArgs args)
    {
        if (buzzRoutine != null)
            return;

        player = args.interactorObject.transform.GetComponentInParent<HapticImpulsePlayer>();
        if (player == null)
            return;

        buzzRoutine = StartCoroutine(Buzz());
    }

    public void StopBuzz()
    {
        if (buzzRoutine == null)
            return;

        StopCoroutine(buzzRoutine);
        buzzRoutine = null;
    }

    // Disabling a component does not stop its coroutines, so stop this one here.
    void OnDisable()
    {
        StopBuzz();
    }

    IEnumerator Buzz()
    {
        // Each impulse starts before the previous one ends, so there is no gap between them.
        while (true)
        {
            player.SendHapticImpulse(amplitude, pulseLength);
            yield return new WaitForSeconds(interval);
        }
    }
}
