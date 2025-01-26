using System.Collections;
using UnityEngine;

public class RegularBubbleBehaviour : Bubble
{
    [SerializeField] private float targetLevitationHeight;

    public void ThrowBubble(Vector3 targetDirection)
    {
        MoveForward(targetDirection);
    }

    protected override void ApplyEffect(BubbleInteractable interactableObject)
    {
        interactableObject.transform.SetParent(transform, false);
    }

    private IEnumerator Levitate(BubbleInteractable interactableObject)
    {
        mRigidbody.useGravity = false;

        float t = 0;
        Vector3 heightVector = new Vector3(0, targetLevitationHeight, 0);

        while (t < lifeTime)
        {
            t += Time.deltaTime;

            yield return null;

            mRigidbody.linearVelocity = heightVector.normalized * speed * Time.deltaTime;
        }

        if (interactableObject.CompareTag("Enemy"))
        {
            interactableObject.ApplyEffect(EffectType.Stun);
            StartCoroutine(TimeBeforeDisabling());
        }
    }
}
