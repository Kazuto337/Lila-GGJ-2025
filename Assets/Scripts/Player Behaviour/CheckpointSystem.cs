using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointSystem : MonoBehaviour
{
    [SerializeField] Vector3 checkpointPosition;

    [SerializeField] UnityEvent<int> onBoundaryFound;

    bool dealingDamage;

    public UnityEvent<int> OnBoundaryFound { get => onBoundaryFound;}

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

        yield return null;

        transform.position = checkpointPosition;

        yield return null;
        onBoundaryFound.Invoke(1);

        yield return new WaitForSeconds(1f);

        dealingDamage = false;
    }

    private void OnDisable()
    {
        onBoundaryFound?.RemoveAllListeners();
    }
}
