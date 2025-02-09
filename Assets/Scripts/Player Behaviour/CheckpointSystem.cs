using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointSystem : MonoBehaviour
{
    Rigidbody m_rigidBody;
    CharacterController characterController;

    [SerializeField] Vector3 checkpointPosition;

    [SerializeField] UnityEvent<int> onBoundaryFound;

    bool dealingDamage;

    public UnityEvent<int> OnBoundaryFound { get => onBoundaryFound;}

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

    private void UpdateCheckpoint(Vector3 newCheckpointPosition)
    {
        checkpointPosition = newCheckpointPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathBoundary"))
        {
            StartCoroutine(DamagePlayer());
        }

        if (other.CompareTag("Checkpoint"))
        {
            UpdateCheckpoint(other.transform.position);
            Destroy(other.gameObject);
        }
    }
    private IEnumerator DamagePlayer()
    {
        if (dealingDamage)
        {
            yield break;
        }
        dealingDamage = true;

        characterController.enabled = false;
        m_rigidBody.Sleep();

        yield return null;

        transform.position = checkpointPosition;
        onBoundaryFound.Invoke(1);

        yield return new WaitForSeconds(1f);

        characterController.enabled = true;
        m_rigidBody.WakeUp();

        dealingDamage = false;
    }

    private void OnDisable()
    {
        onBoundaryFound?.RemoveAllListeners();
    }
}
