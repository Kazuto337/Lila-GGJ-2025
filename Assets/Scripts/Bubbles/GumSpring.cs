using UnityEngine;
using UnityEngine.Events;

public class GumSpring : MonoBehaviour
{
    [SerializeField] private UnityEvent onPlayerJumped;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();

            player.Bounce(1.5f);
        }
    }
}
