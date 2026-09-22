using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;

/// <summary>
/// Week 9, Activity 2b. Detects a right-hand thumbs-up by reading joint poses off the tracked
/// hand each update, and raises UnityEvents when the gesture starts and stops.
/// Wire On Gesture Detected and On Gesture Ended in the Inspector, and drag the XR Origin
/// into the XR Origin field or the thumb direction test will only work where you started.
/// </summary>
public class ThumbsUpDetector : MonoBehaviour
{
    [Header("Scene references")]
    [Tooltip("Drag the XR Origin here. Joint poses are measured relative to the tracking " +
             "origin, so they need lifting into world space before comparing with Vector3.up.")]
    [SerializeField] Transform m_XROrigin;

    [Header("Thresholds, all multiples of hand scale")]
    [Tooltip("A fingertip closer to the wrist than this times hand scale counts as curled.")]
    [SerializeField] float m_CurlThreshold = 1.3f;

    [Tooltip("A thumb tip further from the wrist than this times hand scale counts as extended.")]
    [SerializeField] float m_ThumbExtendThreshold = 1.4f;

    [Tooltip("Dot product of the thumb direction against world up. 0.6 is about 53 degrees.")]
    [SerializeField] float m_UpAlignment = 0.6f;

    [Tooltip("How long the answer must hold before the events fire. If your cube strobes, " +
             "this is the field.")]
    [SerializeField] float m_HoldSeconds = 0.2f;

    [Header("Events")]
    public UnityEvent onGestureDetected;
    public UnityEvent onGestureEnded;

    static readonly List<XRHandSubsystem> s_Subsystems = new List<XRHandSubsystem>();

    XRHandSubsystem m_Subsystem;
    bool m_Detected;
    float m_StableTime;

    void Update()
    {
        // The subsystem is generally not running on the first frame, because OpenXR is still
        // bringing the session up, so keep asking until it is. TryFindSubsystem returns
        // immediately once we are subscribed.
        TryFindSubsystem();
    }

    void OnDisable()
    {
        // Without this, a domain reload in the Editor leaves a dead handler subscribed.
        if (m_Subsystem != null)
            m_Subsystem.updatedHands -= OnUpdatedHands;

        m_Subsystem = null;
    }

    void TryFindSubsystem()
    {
        if (m_Subsystem != null && m_Subsystem.running)
            return;

        SubsystemManager.GetSubsystems(s_Subsystems);

        foreach (var subsystem in s_Subsystems)
        {
            if (!subsystem.running)
                continue;

            m_Subsystem = subsystem;
            m_Subsystem.updatedHands += OnUpdatedHands;
            return;
        }
    }

    void OnUpdatedHands(XRHandSubsystem subsystem,
        XRHandSubsystem.UpdateSuccessFlags successFlags,
        XRHandSubsystem.UpdateType updateType)
    {
        // This event fires twice per frame, once for game logic and once before rendering.
        // A gesture is game logic, so ignore the render-time call.
        if (updateType != XRHandSubsystem.UpdateType.Dynamic)
            return;

        var hand = subsystem.rightHand;

        // Hands leave the tracking volume constantly. Handle only the "hand is there" case
        // and the gesture stays latched on the first time your hand goes out of view.
        if (!hand.isTracked)
        {
            SetDetected(false);
            return;
        }

        SetDetected(EvaluateThumbsUp(hand));
    }

    /// <summary>
    /// Find the thumb tip and the four fingertips, then test that the thumb points up while
    /// the fingers are curled. Everything else is arithmetic.
    /// </summary>
    bool EvaluateThumbsUp(XRHand hand)
    {
        // Every joint must read this frame. A joint the runtime is not confident about is a
        // reason to say "no gesture", not a reason to carry on with last frame's thumb.
        if (!TryGetJointPosition(hand, XRHandJointID.Wrist, out var wrist) ||
            !TryGetJointPosition(hand, XRHandJointID.MiddleProximal, out var middleProximal) ||
            !TryGetJointPosition(hand, XRHandJointID.ThumbMetacarpal, out var thumbMetacarpal) ||
            !TryGetJointPosition(hand, XRHandJointID.ThumbTip, out var thumbTip) ||
            !TryGetJointPosition(hand, XRHandJointID.IndexTip, out var indexTip) ||
            !TryGetJointPosition(hand, XRHandJointID.MiddleTip, out var middleTip) ||
            !TryGetJointPosition(hand, XRHandJointID.RingTip, out var ringTip) ||
            !TryGetJointPosition(hand, XRHandJointID.LittleTip, out var littleTip))
            return false;

        // Wrist to middle knuckle: a stable measure of this hand's size. Every threshold below
        // is a multiple of it, which is what makes the gesture work for somebody else's hands.
        var handScale = Vector3.Distance(wrist, middleProximal);
        if (handScale <= 0f)
            return false;

        // Test 1: all four fingers curled, so each fingertip is pulled back towards the wrist.
        var fingersCurled =
            IsCurled(indexTip, wrist, handScale) &&
            IsCurled(middleTip, wrist, handScale) &&
            IsCurled(ringTip, wrist, handScale) &&
            IsCurled(littleTip, wrist, handScale);

        if (!fingersCurled)
            return false;

        // Test 2: the thumb is extended. The same measurement, the opposite direction.
        var thumbExtended =
            Vector3.Distance(thumbTip, wrist) > m_ThumbExtendThreshold * handScale;

        if (!thumbExtended)
            return false;

        // Test 3: the thumb points up. This is what stops a fist counting, and what stops a
        // thumbs-down counting. It is also the only test that compares against something
        // outside the hand, so it is the only one that cares which space the joints are in.
        var thumbDirection = (thumbTip - thumbMetacarpal).normalized;
        return Vector3.Dot(thumbDirection, Vector3.up) > m_UpAlignment;
    }

    bool IsCurled(Vector3 fingertip, Vector3 wrist, float handScale) =>
        Vector3.Distance(fingertip, wrist) < m_CurlThreshold * handScale;

    bool TryGetJointPosition(XRHand hand, XRHandJointID jointID, out Vector3 position)
    {
        position = Vector3.zero;

        var joint = hand.GetJoint(jointID);
        if (!joint.TryGetPose(out var pose))
            return false;

        position = ToWorld(pose.position);
        return true;
    }

    Vector3 ToWorld(Vector3 position) =>
        m_XROrigin != null ? m_XROrigin.TransformPoint(position) : position;

    /// <summary>
    /// Debounce. Tracked hands are noisy, so require the answer to be stable for
    /// m_HoldSeconds before turning it into an event.
    /// </summary>
    void SetDetected(bool conditionMet)
    {
        if (conditionMet == m_Detected)
        {
            m_StableTime = 0f;
            return;
        }

        m_StableTime += Time.deltaTime;
        if (m_StableTime < m_HoldSeconds)
            return;

        m_StableTime = 0f;
        m_Detected = conditionMet;

        if (m_Detected)
            onGestureDetected.Invoke();
        else
            onGestureEnded.Invoke();
    }
}
