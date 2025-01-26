using UnityEngine;

public class BubbleGumBehaviour : Bubble
{
    [SerializeField] private GumSpring spring;

    public void ThrowBubble(Vector3 targetPosition)
    {
        MoveForward(targetPosition);
    }
    protected override void ApplyEffect(BubbleInteractable interactableObject)
    {
        interactableObject.ApplyEffect(EffectType.Stun);
        ActivateSpring();
    }

    private void ActivateSpring()
    {
        effectCollider.enabled = false;
        spring.gameObject.SetActive(true);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            effectCollider.enabled = false;
            meshRenderer.enabled = false;
            ActivateSpring();
        }
    }
}
