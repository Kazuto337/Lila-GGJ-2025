using System.Collections;
using UnityEngine;

public class ExplosiveBubble : Bubble
{
    [SerializeField] private GameObject explosion;
    public void ThrowBubble(Vector3 targetPosition)
    {
        MoveForward(targetPosition);
    }
    protected override void ApplyEffect(BubbleInteractable interactableObject)
    {
        interactableObject.ApplyEffect(EffectType.Stun);
        ActivateExplosion();
    }
    public override void ActivatePoping()
    {
        gameObject.SetActive(false);
    }

    private void ActivateExplosion()
    {
        StartCoroutine(TimeBeforeDisabling());
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, transform.rotation);
        StartCoroutine(TimeBeforeDisabling());

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<BubbleInteractable>().ApplyEffect(EffectType.Stun);
        }

        if (collision.gameObject.CompareTag("Destroyable"))
        {
            collision.gameObject.GetComponent<BubbleInteractable>().ApplyEffect(EffectType.Destroy);
        }
    }
}
