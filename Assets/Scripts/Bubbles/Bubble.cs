using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Bubble : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float lifeTime;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected Collider effectCollider;

    [SerializeField] protected UnityEvent<Bubble> onBubbleDisabled;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public virtual void FireBubble(Vector3 targetPosition)
    {
        StartCoroutine(MoveTowards(targetPosition));
    }

    protected virtual void ApplyEffect(BubbleInteractable interactableObject)
    {
        interactableObject.ApplyEffect(EffectType.Stun);
        StartCoroutine(TimeBeforeDisabling());
    }

    protected virtual void DesactivateBubble()
    {
        onBubbleDisabled.Invoke(this);
        gameObject.SetActive(false);
    }

    protected virtual void ActivatePoping()
    {
        //animates poping
        Debug.Log("Bubble Popped!");
    }

    protected virtual IEnumerator MoveTowards(Vector3 targetPosition)
    {
        float t = 0;

        while (t < lifeTime)
        {
            t += Time.deltaTime;

            yield return null;

            _rigidbody.linearVelocity = targetPosition.normalized * speed * Time.deltaTime;
        }

        StartCoroutine(TimeBeforeDisabling());
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
