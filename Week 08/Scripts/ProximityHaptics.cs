using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Week 8, Activity 3. While this cube and a partner cube are both held, vibrates both
/// controllers more strongly the closer the cubes get. When they touch, the partner plays its
/// sound, is released from the hand holding it, and is pushed away.
/// </summary>
[RequireComponent(typeof(XRGrabInteractable))]
public class ProximityHaptics : MonoBehaviour
{
    [Tooltip("The other cube. This is the one that flies away.")]
    public XRGrabInteractable partner;

    [Tooltip("Distance between the cubes, in metres, at which the vibration starts.")]
    public float startDistance = 0.6f;

    [Tooltip("Distance between the cube centres, in metres, that counts as touching.")]
    public float touchDistance = 0.16f;

    [Range(0f, 1f)]
    [Tooltip("Amplitude when the cubes are at Start Distance.")]
    public float minAmplitude = 0.05f;

    [Range(0f, 1f)]
    [Tooltip("Amplitude just before the cubes touch.")]
    public float maxAmplitude = 1f;

    [Tooltip("Speed the partner is pushed away at, in metres per second.")]
    public float launchSpeed = 3f;

    // The same overlapping impulses as SustainedHaptic in Activity 2.
    const float k_PulseLength = 0.1f;
    const float k_Interval = 0.08f;

    XRGrabInteractable self;
    float nextPulseTime;

    void Awake()
    {
        self = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        if (partner == null || !self.isSelected || !partner.isSelected)
            return;

        var distance = Vector3.Distance(transform.position, partner.transform.position);

        if (distance <= touchDistance)
        {
            Touch();
            return;
        }

        if (distance > startDistance || Time.time < nextPulseTime)
            return;

        // 0 at Start Distance, rising to 1 at Touch Distance.
        var closeness = Mathf.InverseLerp(startDistance, touchDistance, distance);
        var amplitude = Mathf.Lerp(minAmplitude, maxAmplitude, closeness);

        Pulse(self, amplitude);
        Pulse(partner, amplitude);
        nextPulseTime = Time.time + k_Interval;
    }

    void Touch()
    {
        var sound = partner.GetComponent<AudioSource>();
        if (sound != null)
            sound.Play();

        // Release the partner from the hand holding it, the same as the player letting go.
        partner.interactionManager.SelectExit(partner.firstInteractorSelecting, partner);

        var direction = (partner.transform.position - transform.position).normalized;
        StartCoroutine(Launch(partner.GetComponent<Rigidbody>(), direction));
    }

    IEnumerator Launch(Rigidbody body, Vector3 direction)
    {
        // Throw On Detach sets the velocity at the end of the frame the cube was released in,
        // so wait one frame before pushing it.
        yield return null;
        body.linearVelocity = (direction + Vector3.up * 0.5f).normalized * launchSpeed;
    }

    // As in HapticPulse: the HapticImpulsePlayer is on the controller holding the cube.
    static void Pulse(XRGrabInteractable cube, float amplitude)
    {
        var player = cube.firstInteractorSelecting.transform.GetComponentInParent<HapticImpulsePlayer>();
        if (player != null)
            player.SendHapticImpulse(amplitude, k_PulseLength);
    }
}
