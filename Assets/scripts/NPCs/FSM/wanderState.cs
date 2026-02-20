using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class wanderState : IState
{
    public float wanderRadius;
    public float wanderTimer;


    private NavMeshAgent agent;
    private Transform target;
    private float timer;

    public wanderState(Transform target, NavMeshAgent agent,float radius = 10f, float timeDuration = 3f)
    {
        wanderRadius = radius; 
        wanderTimer = timeDuration;
        this.agent = agent;
        this.target = target;
    }
    


    public void onEnter()
    {
        Debug.Log("In the wander state!");

        timer = wanderTimer;
    }

    public void onExit()
    {
        
    }

    public void update()
    {
        Debug.Log("Moving to update!");
        timer += Time.deltaTime;

        if (timer >= wanderTimer) {


            
                Vector3 newPos = RandomNavSphere(target.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            
            
        }
    }


   public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * dist;

        randDir += origin; 

       NavMeshHit hit;
        NavMesh.SamplePosition(randDir, out hit, dist, layermask);

        return hit.position;
    }
}
