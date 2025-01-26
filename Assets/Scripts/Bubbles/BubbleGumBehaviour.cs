using UnityEngine;

public class BubbleGumBehaviour : Bubble
{
    [SerializeField] private GameObject springPrefab;

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
        Instantiate(springPrefab , transform.position , transform.rotation);
        StartCoroutine(TimeBeforeDisabling());
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            effectCollider.enabled = false;
            meshRenderer.enabled = false;
            ActivateSpring();
        }
    }
}
