using System.Collections;
using UnityEngine;

public class RegularBubbleBehaviour : Bubble
{
    public void ThrowBubble(Vector3 targetDirection)
    {
        MoveForward(targetDirection);
    }

    protected override void ApplyEffect(BubbleInteractable interactableObject)
    {
        if (interactableObject.CompareTag("Enemy"))
        {
            interactableObject.ApplyEffect(EffectType.Stun);
            StartCoroutine(TimeBeforeDisabling());

            return;
        }

        (interactableObject as DestroyableObject).DesactivateGravity();

        StopCoroutine(MoveForward(transform.forward));
        mRigidbody.linearVelocity = Vector3.zero;
        canMove = false;

        mRigidbody.isKinematic = true;

        interactableObject.transform.SetParent(transform, false);
        interactableObject.transform.localScale = Vector3.one/2;
        interactableObject.transform.transform.localPosition = Vector3.zero;

        StartCoroutine(Levitate(interactableObject));
    }

    private IEnumerator Levitate(BubbleInteractable interactableObject)
    {
        effectCollider.enabled = false;
        yield return new WaitForSeconds(0.1f);

        float t = 0;

        while (t < lifeTime/2)
        {
            t += Time.deltaTime;

            yield return null;

            mRigidbody.MovePosition(transform.position + (interactableObject.transform.up * Time.deltaTime * speed));
        }

        yield return null;
        (interactableObject as DestroyableObject).ActivateGravity();
        interactableObject.transform.parent = null;

        (interactableObject as DestroyableObject).ResetScale();

        ActivatePoping();
    }
}
