using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Bubble : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float lifeTime;
    [SerializeField] protected Rigidbody mRigidbody;
    [SerializeField] protected Collider effectCollider;
    [SerializeField] protected MeshRenderer meshRenderer;

    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }

    public virtual void FireBubble(Vector3 targetDirections)
    {
        StartCoroutine(MoveForward(targetDirections));
    }

    protected virtual void ApplyEffect(BubbleInteractable interactableObject)
    {
        interactableObject.ApplyEffect(EffectType.Stun);
        StartCoroutine(TimeBeforeDisabling());
    }

    protected virtual void DesactivateBubble()
    {
        Destroy(gameObject);
    }

    public virtual void ActivatePoping()
    {
        //animates poping
        meshRenderer.enabled = false;

        Debug.Log("Bubble Popped!");
    }

    protected virtual IEnumerator MoveForward(Vector3 targetDirection)
    {
        Debug.Log(targetDirection);

        float t = 0;

        while (t < lifeTime)
        {
            t += Time.deltaTime;

            yield return null;

            mRigidbody.MovePosition(transform.position + (targetDirection * Time.deltaTime * speed));
        }
    }

    protected virtual IEnumerator TimeBeforeDisabling()
    {
        ActivatePoping();

        yield return new WaitForSeconds(2f);

        DesactivateBubble();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<BubbleInteractable>(out BubbleInteractable interactable))
        {
            Debug.Log("Bubble Interactable Found");
            ApplyEffect(interactable);
            return;
        }

        StartCoroutine(TimeBeforeDisabling());
    }

    public enum EffectType
    {
        Stun = 0,
        Destroy
    }
}
