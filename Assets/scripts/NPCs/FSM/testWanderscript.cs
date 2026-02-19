using UnityEngine;
using UnityEngine.AI;

public class testWanderscript : MonoBehaviour
{

    public float wanderRadius = 20f;
    private NavMeshAgent agent;
    public float moveSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        setNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            setNewDestination() ;
        }
    }

    void setNewDestination()
    {
        Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
        randomDir += transform.position;

        NavMeshHit hit;

        NavMesh.SamplePosition(randomDir, out hit, wanderRadius, 1);

        agent.SetDestination(hit.position);
    }
}
