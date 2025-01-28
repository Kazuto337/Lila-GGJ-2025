using System.Collections;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    bool attakingPlayer;

    private IEnumerator DamagePlayer(PlayerStats playerStats)
    {
        if (attakingPlayer)
        {
            yield break;
        }

        attakingPlayer = true;

        yield return null;
        playerStats.TakeDamage(1);

        yield return new WaitForSeconds(1f);

        attakingPlayer = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            StartCoroutine(DamagePlayer(playerStats));
        }
    }
}
