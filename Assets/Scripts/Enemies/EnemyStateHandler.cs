using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum EnemyStat
{
    Idle,
    Patrol,
    chasePlayer,
    AttackPlayer,
    Death,

}
public enum TypeEnemy
{
    Duck,
    Ship
}

public class EnemyStateHandler : MonoBehaviour
{
    public UnityEvent<EnemyStat> OnChangeEnemyStatEvent = new UnityEvent<EnemyStat>();

    [SerializeField] ChasePlayerSystem chasePlayerSystem;
    [SerializeField] AttackPlayerSystem attackPlayerSystem;

    public EnemyStat currentState;
    public TypeEnemy typeEnemy;

    public Vector3 mainPosition;
    private Vector3 patrolPosition;
    public float maxRadius;

    private NavMeshAgent agent;
    private Collider chaseCollider;


    private bool isMoving = false;
    private bool isAttacking = false;
    private bool dealingDamage;

    private void Awake()
    {
        mainPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        chaseCollider = chasePlayerSystem.GetComponent<Collider>();
    }
    private void OnEnable()
    {
        OnChangeEnemyStatEvent.AddListener(ChageEnemyState);
    }
    private void OnDisable()
    {
        OnChangeEnemyStatEvent.RemoveAllListeners();
    }
    void Update()
    {
        if (currentState == EnemyStat.Idle)
        {
            chaseCollider.enabled = false;
            patrolPosition = PatrolSystem.GenerateRandomPatrolPoint(mainPosition, transform.position, maxRadius);
            currentState = EnemyStat.Patrol;
            isMoving = false;
            isAttacking = false;
        }
        else if (currentState == EnemyStat.Patrol)
        {
            if (!isMoving)
            {
                agent.isStopped = false;
                agent.SetDestination(patrolPosition);
                Invoke("ChaseDetection", .4f);
                isMoving = true;
            }
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    currentState = EnemyStat.Idle;
                    isMoving = false;
                }
            }
        }
        else if (currentState == EnemyStat.chasePlayer)
        {
            if (!isMoving)
            {
                Debug.Log("Chassing the player");
                agent.isStopped = false;
                isMoving = chasePlayerSystem.ChaseState();
            }

        }
        else if (currentState == EnemyStat.AttackPlayer)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                agent.isStopped = false;
                attackPlayerSystem.canAttack = true;
                switch (typeEnemy)
                {
                    case TypeEnemy.Duck:
                        attackPlayerSystem.Parabolic();
                        break;
                    case TypeEnemy.Ship:
                        Debug.Log($"<color=red>666</color>");
                        attackPlayerSystem.Charge();
                        
                        break;
                    default:
                        break;
                }

                Debug.Log($"<color=red>Attack  player</color>");
            }

        }
    }
    private void ChaseDetection()
    {
        chaseCollider.enabled = true;
    }
    public void ChageEnemyState(EnemyStat _enemyStat)
    {
        currentState = _enemyStat;
        StopAgent(agent);
        isMoving = false;
    }
    public void StopAgent(NavMeshAgent _agent)
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            StartCoroutine(DamagePlayer(playerStats));
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = hit.gameObject.GetComponent<PlayerStats>();
            StartCoroutine(DamagePlayer(playerStats));
        }
    }

    private IEnumerator DamagePlayer(PlayerStats playerStats)
    {
        if (dealingDamage)
        {
            yield break;
        }
        dealingDamage = true;

        yield return null;
        playerStats.TakeDamage(1);

        yield return new WaitForSeconds(1f);

        dealingDamage = false;
    }
}
