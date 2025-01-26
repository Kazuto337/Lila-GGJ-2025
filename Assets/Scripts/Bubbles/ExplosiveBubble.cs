using System.Collections;
using UnityEngine;

public class ExplosiveBubble : Bubble
{
    [SerializeField] private Collider explosionCollider;
    public void ThrowBubble(Vector3 targetPosition)
    {
        MoveForward(targetPosition);
    }
    protected override void ApplyEffect(BubbleInteractable interactableObject)
    {
        interactableObject.ApplyEffect(EffectType.Stun);
        ActivateExplosion();
    }

    private void ActivateExplosion()
    {
        effectCollider.enabled = false;
        explosionCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<BubbleInteractable>().ApplyEffect(EffectType.Stun);
        }

        if (other.CompareTag("Destroyable"))
        {
            other.GetComponent<BubbleInteractable>().ApplyEffect(EffectType.Destroy);
        }
    }
}
