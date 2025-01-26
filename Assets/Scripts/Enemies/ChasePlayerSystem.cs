using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class ChasePlayerSystem : MonoBehaviour
{
    [SerializeField] EnemyStateHandler enemyStateHandler;

    public float chaseDistancePercentage = 0.2f;

    private Transform player;
    [SerializeField] private GameObject enemy;

    private Vector3 movementVector;
    private float originalChaseDistance;
    public bool isStartingToChase = true;

    
    void Start()
    {
        
    }


    public bool ChaseState()
    {   
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.SetDestination(movementVector);
        
        float currentDistance = Vector3.Distance(transform.position, player.position);
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            
            enemyStateHandler.OnChangeEnemyStatEvent?.Invoke(EnemyStat.AttackPlayer);
            return false;
        }
        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.gameObject.tag == "Player" && !isStartingToChase)
        {
            Debug.Log($"<color=green>The player has been watched</color>");
            player = other.gameObject.transform;
            
            isStartingToChase = true;
            Debug.Log(Vector3.Distance(transform.position, player.position));
            Debug.Log(Vector3.Distance(transform.position, player.position) * chaseDistancePercentage);
            enemy.GetComponent<NavMeshAgent>().isStopped = true;
            CalculateMovement();

            enemyStateHandler.OnChangeEnemyStatEvent?.Invoke(EnemyStat.chasePlayer);
            this.GetComponent<Collider>().enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.gameObject.tag == "Player")
        {
            ChasingAgain();
        }


    }
    private void ChasingAgain()
    {
        this.GetComponent<Collider>().enabled = true;
        isStartingToChase = false;
    }

    private void CalculateMovement()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        originalChaseDistance = Vector3.Distance(transform.position, player.position) * chaseDistancePercentage;
        movementVector = transform.position + direction * originalChaseDistance;
    }

}
