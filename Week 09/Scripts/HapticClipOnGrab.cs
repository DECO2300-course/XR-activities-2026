using Oculus.Haptics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Week 9, Activity 3. Plays a haptic clip on the controller that grabbed this object.
/// Wire Play to Select Entered and Stop to Select Exited.
/// </summary>
public class HapticClipOnGrab : MonoBehaviour
{
    [Tooltip("The haptic clip to play. The Haptics SDK sample packs are full of them.")]
    public HapticClip clip;

    [Tooltip("Drag the rig's Left Controller here.")]
    public Transform leftController;

    [Tooltip("Drag the rig's Right Controller here.")]
    public Transform rightController;

    HapticClipPlayer player;

    void Awake()
    {
        if (clip != null)
            player = new HapticClipPlayer(clip);
    }

    // A player holds an unmanaged handle, so release it with the object.
    void OnDestroy()
    {
        player?.Dispose();
    }

    public void Play(SelectEnterEventArgs args)
    {
        if (player == null)
            return;

        // The SDK addresses a controller by its side, and the event says which Interactor
        // grabbed, so ask which controller that Interactor belongs to.
        var interactor = args.interactorObject.transform;

        if (rightController != null && interactor.IsChildOf(rightController))
            player.Play(Controller.Right);
        else if (leftController != null && interactor.IsChildOf(leftController))
            player.Play(Controller.Left);
    }

    public void Stop()
    {
        player?.Stop();
    }
}
