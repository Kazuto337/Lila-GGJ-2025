using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AttackPlayerSystem : MonoBehaviour
{
    [SerializeField] EnemyStateHandler enemyStateHandler;

    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public Transform player;


    public float projectileLaunchAngle = 45f;


    public float ChargeSpeed = 60f; 
    public float ChargeStopDistance = 1f; 
    public float pushForce = 45f;    

    
    public float attackCooldown = 2f;  
    public bool canAttack = false;    

    private NavMeshAgent agent;    


    private void Start()
    {
        

    }



    public void Parabolic()
    {
        if (!canAttack)
        {
            enemyStateHandler.OnChangeEnemyStatEvent?.Invoke(EnemyStat.Idle);
            return;
        }
        StartCoroutine(ParabolicAttack());
        Debug.Log($"<color=green>Parabolic Attack</color>");
    }
    public void Charge()
    {
        if (!canAttack)
        {
            enemyStateHandler.OnChangeEnemyStatEvent?.Invoke(EnemyStat.Idle);
            return;
        }
        StartCoroutine(ChargeAttack());
        Debug.Log($"<color=green>Charge Attack</color>");
    }

    
    public IEnumerator ParabolicAttack()
    {

        canAttack = false;

        LaunchProjectile();

        yield return new WaitForSeconds(attackCooldown);
        enemyStateHandler.OnChangeEnemyStatEvent?.Invoke(EnemyStat.Idle); 
    }

    private void LaunchProjectile()
    {
        Transform projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity).transform;
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 velocity = CalculateLaunchVelocity(spawnPoint.position, player.position, projectileLaunchAngle);
            rb.linearVelocity = velocity;
        }
    }

    private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float angle)
    {
        Vector3 direction = target - start;
        float heightDifference = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float radians = angle * Mathf.Deg2Rad;

        float initialVelocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * radians));
        float velocityY = Mathf.Sin(radians) * initialVelocity;
        float velocityXZ = Mathf.Cos(radians) * initialVelocity;

        Vector3 velocity = direction.normalized * velocityXZ;
        velocity.y = velocityY;

        return velocity;
    }

    private IEnumerator ChargeAttack()
    {
        agent = GetComponent<NavMeshAgent>();
        canAttack = false;

        float originalSpeed = agent.speed;
        bool originalAutoBraking = agent.autoBraking;
        agent.autoBraking = false;
        agent.speed = ChargeSpeed;

        Vector3 targetPosition = player.position;
        targetPosition.y = transform.position.y;

        agent.SetDestination(targetPosition);
        

        while (Vector3.Distance(transform.position, targetPosition) > ChargeStopDistance)
        {
            yield return null;
        }

        PushPlayer();

        agent.ResetPath();
        agent.speed = originalSpeed;

        yield return new WaitForSeconds(attackCooldown);

        enemyStateHandler.OnChangeEnemyStatEvent?.Invoke(EnemyStat.Idle);
    }

    private void PushPlayer()
    {
        CharacterController playerController = player.GetComponent<CharacterController>();
        if (playerController != null)
        {
            Vector3 pushDirection = (player.position - transform.position).normalized;
            pushDirection.y = 0;

            playerController.Move(pushDirection * pushForce * Time.deltaTime);
        }
    }
} 
