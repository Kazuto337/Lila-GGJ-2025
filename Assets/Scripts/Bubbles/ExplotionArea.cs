using System.Collections;
using UnityEngine;
using static Bubble;

public class ExplotionArea : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyCountdown());
    }

    private IEnumerator DestroyCountdown()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
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
