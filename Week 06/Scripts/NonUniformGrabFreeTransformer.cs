using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

/// <summary>
/// Week 6, Activity 2 extension. A two-handed grab transformer that scales the held
/// object per axis rather than uniformly, so pulling your hands apart along the object's
/// own X stretches it in X only.
///
/// XRGeneralGrabTransformer scales uniformly and bounds the result with
/// minimumScaleRatio and maximumScaleRatio. This one keeps the same idea of a bounded
/// ratio but applies a separate ratio to each axis you allow.
/// </summary>
public class NonUniformGrabFreeTransformer : XRBaseGrabTransformer
{
    [Header("Axes allowed to scale")]
    [SerializeField]
    bool m_ScaleX = true;

    [SerializeField]
    bool m_ScaleY = true;

    [SerializeField]
    bool m_ScaleZ = true;

    [Header("Bounds, as a ratio of the scale the object had when the second hand joined")]
    [SerializeField]
    float m_MinimumScaleRatio = 0.25f;

    [SerializeField]
    float m_MaximumScaleRatio = 4f;

    [Header("Two-handed movement")]
    [SerializeField]
    [Tooltip("Keep the object centred between the two hands.")]
    bool m_FollowHandMidpoint = true;

    [SerializeField]
    [Tooltip("Freeze rotation while two hands are holding the object, so it only scales.")]
    bool m_LockRotationWhileScaling = true;

    // Hands closer together than this along an axis contribute no scale on that axis.
    const float k_MinimumSpan = 0.001f;

    // Registers this transformer for multi-hand grabs only, so one-handed grabs keep
    // whatever the Interactable's single-grab transformer already does.
    protected override RegistrationMode registrationMode => RegistrationMode.Multiple;

    Vector3 m_StartScale = Vector3.one;
    Vector3 m_StartSpan = Vector3.one;
    Vector3 m_StartMidpointOffset = Vector3.zero;
    Quaternion m_StartRotation = Quaternion.identity;
    bool m_TwoHanded;

    public override void OnGrabCountChanged(XRGrabInteractable grabInteractable,
        Pose targetPose, Vector3 localScale)
    {
        base.OnGrabCountChanged(grabInteractable, targetPose, localScale);

        // This is the two-hand hook: it fires the moment a second hand joins or leaves,
        // which is where the "before" measurements have to be taken.
        m_TwoHanded = grabInteractable.interactorsSelecting.Count >= 2;
        if (!m_TwoHanded)
            return;

        m_StartScale = localScale;
        m_StartRotation = targetPose.rotation;
        m_StartSpan = GetLocalSpan(grabInteractable, m_StartRotation);
        m_StartMidpointOffset = targetPose.position - GetMidpoint(grabInteractable);
    }

    public override void Process(XRGrabInteractable grabInteractable,
        XRInteractionUpdateOrder.UpdatePhase updatePhase,
        ref Pose targetPose, ref Vector3 localScale)
    {
        if (updatePhase != XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            return;

        if (!m_TwoHanded || grabInteractable.interactorsSelecting.Count < 2)
            return;

        var rotation = m_LockRotationWhileScaling ? m_StartRotation : targetPose.rotation;
        var span = GetLocalSpan(grabInteractable, rotation);

        localScale = new Vector3(
            AxisScale(m_ScaleX, m_StartScale.x, span.x, m_StartSpan.x),
            AxisScale(m_ScaleY, m_StartScale.y, span.y, m_StartSpan.y),
            AxisScale(m_ScaleZ, m_StartScale.z, span.z, m_StartSpan.z));

        if (m_LockRotationWhileScaling)
            targetPose.rotation = m_StartRotation;

        if (m_FollowHandMidpoint)
            targetPose.position = GetMidpoint(grabInteractable) + m_StartMidpointOffset;
    }

    float AxisScale(bool axisEnabled, float startScale, float currentSpan, float startSpan)
    {
        if (!axisEnabled || startSpan < k_MinimumSpan)
            return startScale;

        var ratio = Mathf.Clamp(currentSpan / startSpan, m_MinimumScaleRatio, m_MaximumScaleRatio);
        return startScale * ratio;
    }

    static Vector3 GetMidpoint(XRGrabInteractable grabInteractable)
    {
        var interactors = grabInteractable.interactorsSelecting;
        var sum = Vector3.zero;
        for (var i = 0; i < interactors.Count; ++i)
            sum += interactors[i].GetAttachTransform(grabInteractable).position;

        return interactors.Count > 0 ? sum / interactors.Count : sum;
    }

    /// <summary>
    /// The vector between the two hands, measured along the object's own axes and made
    /// positive. Working in the object's axes is what makes the scaling per-axis: a
    /// stretch along the object's local X only ever grows X.
    /// </summary>
    static Vector3 GetLocalSpan(XRGrabInteractable grabInteractable, Quaternion rotation)
    {
        var interactors = grabInteractable.interactorsSelecting;
        if (interactors.Count < 2)
            return Vector3.one;

        var first = interactors[0].GetAttachTransform(grabInteractable).position;
        var second = interactors[1].GetAttachTransform(grabInteractable).position;
        var span = Quaternion.Inverse(rotation) * (second - first);

        return new Vector3(Mathf.Abs(span.x), Mathf.Abs(span.y), Mathf.Abs(span.z));
    }
}
