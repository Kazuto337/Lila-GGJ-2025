using System.Collections;
using UnityEngine;

public class RegularBubbleBehaviour : Bubble
{
    [SerializeField] private float levitationTime;
    [SerializeField] private float targetLevitationHeight;

    public void ThrowBubble(Vector3 targetPosition)
    {
        MoveTowards(targetPosition);
    }

    protected override void ApplyEffect(BubbleInteractable interactableObject)
    {
        interactableObject.transform.SetParent(transform, false);
    }

    private IEnumerator Levitate(BubbleInteractable interactableObject)
    {
        float t = 0;
        Vector3 heightVector = new Vector3(0, targetLevitationHeight, 0);

        while (t < levitationTime)
        {
            t += Time.deltaTime;

            yield return null;

            _rigidbody.linearVelocity = heightVector.normalized * speed * Time.deltaTime;
        }

        if (interactableObject.CompareTag("Enemy"))
        {
            interactableObject.ApplyEffect(EffectType.Stun); 
        }
    }
}
