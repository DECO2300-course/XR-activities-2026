using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

/// <summary>
/// Week 6, Activity 3. A one-handed grab transformer that moves the held object with
/// the hand and cycles its material hue as the hand travels up and down.
/// </summary>
public class CustomTransformer : XRBaseGrabTransformer
{
    [SerializeField]
    [Tooltip("Full hue cycles per metre of vertical hand movement.")]
    float m_HueCyclesPerMetre = 2f;

    [SerializeField]
    [Tooltip("Renderer to tint. Leave empty to use the first Renderer on the Interactable.")]
    Renderer m_TargetRenderer;

    // Registers this transformer for one-handed grabs only. The Interactable keeps a
    // separate list for multi-hand grabs, which this component does not handle.
    protected override RegistrationMode registrationMode => RegistrationMode.Single;

    Material m_MaterialInstance;
    Vector3 m_GrabOffset;
    float m_StartAttachHeight;
    float m_StartHue;
    float m_StartSaturation;
    float m_StartValue;

    public override void OnLink(XRGrabInteractable grabInteractable)
    {
        base.OnLink(grabInteractable);

        if (m_TargetRenderer == null)
            m_TargetRenderer = grabInteractable.GetComponentInChildren<Renderer>();

        // Reading .material hands back a copy that belongs to this object alone, so
        // tinting one cube does not tint every other cube sharing the same material.
        if (m_TargetRenderer != null)
            m_MaterialInstance = m_TargetRenderer.material;
    }

    public override void OnGrab(XRGrabInteractable grabInteractable)
    {
        base.OnGrab(grabInteractable);

        var attach = GetFirstAttachTransform(grabInteractable);
        if (attach == null)
            return;

        m_GrabOffset = grabInteractable.transform.position - attach.position;
        m_StartAttachHeight = attach.position.y;

        // Keep the saturation and value the material already has, so only the hue moves.
        if (m_MaterialInstance != null)
            Color.RGBToHSV(m_MaterialInstance.color, out m_StartHue, out m_StartSaturation, out m_StartValue);
    }

    public override void Process(XRGrabInteractable grabInteractable,
        XRInteractionUpdateOrder.UpdatePhase updatePhase,
        ref Pose targetPose, ref Vector3 localScale)
    {
        if (updatePhase != XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            return;

        var attach = GetFirstAttachTransform(grabInteractable);
        if (attach == null)
            return;

        // Movement: follow the hand, keeping the offset captured when the grab began.
        targetPose.position = attach.position + m_GrabOffset;

        // Colour: vertical hand travel since the grab began drives the hue.
        if (m_MaterialInstance == null)
            return;

        var verticalTravel = attach.position.y - m_StartAttachHeight;
        var hue = Mathf.Repeat(m_StartHue + verticalTravel * m_HueCyclesPerMetre, 1f);
        m_MaterialInstance.color = Color.HSVToRGB(hue, m_StartSaturation, m_StartValue);
    }

    // The copy taken in OnLink belongs to this component, so this component destroys it.
    protected virtual void OnDestroy()
    {
        if (m_MaterialInstance != null)
            Destroy(m_MaterialInstance);
    }

    static Transform GetFirstAttachTransform(XRGrabInteractable grabInteractable)
    {
        var interactors = grabInteractable.interactorsSelecting;
        return interactors.Count > 0 ? interactors[0].GetAttachTransform(grabInteractable) : null;
    }
}
