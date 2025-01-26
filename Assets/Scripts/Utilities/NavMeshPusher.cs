using UnityEngine;
using UnityEngine.AI;

public class NavMeshPusher : MonoBehaviour
{
    public float pushForce = 5f; 

    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody otherRigidbody = collision.rigidbody;

        if (otherRigidbody != null && !otherRigidbody.isKinematic && collision.gameObject.tag == "Obstacle")
        {
            
            Vector3 pushDirection = collision.transform.position - transform.position;
            pushDirection.y = 0;

            
            otherRigidbody.AddForce(pushDirection.normalized * pushForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Rigidbody otherRigidbody = collision.rigidbody;

        if (otherRigidbody != null && !otherRigidbody.isKinematic && collision.gameObject.tag == "Obstacle")
        {
            
            Vector3 pushDirection = collision.transform.position - transform.position;
            pushDirection.y = 0;

            otherRigidbody.AddForce(pushDirection.normalized * pushForce, ForceMode.Force);
        }
    }
}
