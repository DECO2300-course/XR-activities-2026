using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Week 6, Activity 4. Turns a socket's attach transform, so that whatever the socket is
/// holding turns with it.
/// </summary>
[RequireComponent(typeof(XRSocketInteractor))]
public class SocketTurntable : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Degrees turned per second.")]
    float m_DegreesPerSecond = 30f;

    [SerializeField]
    [Tooltip("Axis to turn about, in the socket's own space.")]
    Vector3 m_Axis = Vector3.up;

    [SerializeField]
    [Tooltip("Turn only while the socket is holding something.")]
    bool m_OnlyWhenFilled = true;

    XRSocketInteractor m_Socket;
    Transform m_Attach;

    // Start rather than Awake: the socket creates its attach transform in its own Awake,
    // and this reads the result.
    void Start()
    {
        m_Socket = GetComponent<XRSocketInteractor>();
        m_Attach = m_Socket.attachTransform;
    }

    void Update()
    {
        if (m_OnlyWhenFilled && !m_Socket.hasSelection)
            return;

        // The socket holds what it has at this transform every frame, so turning the
        // transform turns the object.
        m_Attach.Rotate(m_Axis, m_DegreesPerSecond * Time.deltaTime, Space.Self);
    }
}
