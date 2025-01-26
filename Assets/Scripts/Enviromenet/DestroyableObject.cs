using UnityEngine;
using static Bubble;

public class DestroyableObject : BubbleInteractable
{
    Rigidbody body;
    public Vector3 initialLocalScale;

    private void Start()
    {
        initialLocalScale = transform.localScale;
        body = GetComponent<Rigidbody>();
    }

    public void ResetScale()
    {
        transform.localScale = initialLocalScale;
    }

    public override void ApplyEffect(EffectType effect)
    {
        if (effect != EffectType.Destroy)
        {
            return;
        }
    }

    public void ActivateGravity()
    {
        body.useGravity = true;
        body.isKinematic = false;
    }

    public void DesactivateGravity()
    {
        body.useGravity = false;
        body.isKinematic = true;
    }
}
