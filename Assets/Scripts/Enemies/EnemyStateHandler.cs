using UnityEngine;
using UnityEngine.AI;

public enum EnemyStat
{
    Idle,
    Patrol,
    chasePlayer,
    AttackPlayer,
    Death,

}

public class EnemyStateHandler : MonoBehaviour
{
    public EnemyStat currentState;
    public Vector3 mainPosition;
    private Vector3 patrolPosition;
    public float maxRadius;
    private NavMeshAgent agent;
    private bool isMoving = false;

    private void Awake()
    {
        mainPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == EnemyStat.Idle)
        {
            patrolPosition = PatrolSystem.GenerateRandomPatrolPoint(mainPosition, transform.position, maxRadius);
            currentState = EnemyStat.Patrol;
        }
        else if (currentState == EnemyStat.Patrol)
        {
            if (!isMoving)
            {
                agent.SetDestination(patrolPosition);
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



    }
}
