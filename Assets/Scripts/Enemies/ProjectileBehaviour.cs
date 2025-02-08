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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            StartCoroutine(DamagePlayer(playerStats));
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
