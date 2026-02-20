using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class wanderState : IState
{
    public float wanderRadius;
    public float wanderTimer;
    

    private NavMeshAgent agent;
    private Animator animator;   
    private Transform target;
    private float timer;

    public wanderState(Transform target, NavMeshAgent agent, Animator animator, float radius = 10f, float timeDuration = 3f)
    {
        wanderRadius = radius; 
        wanderTimer = timeDuration;
        this.agent = agent;
        this.target = target;
        this.animator = animator;
    }
    


    public void onEnter()
    {
        animator.Play("Walk");

        timer = wanderTimer;
    }

    public void onExit()
    {
        
    }

    public void update()
    {
   
        timer += Time.deltaTime;

        if (timer >= wanderTimer) {
                Vector3 newPos = RandomNavSphere(target.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
        }

        float angle = Vector3.SignedAngle(agent.transform.forward, agent.velocity, Vector3.up);

        if(angle > 15f)
        {
            animator.Play("turn right 90");
        }else if(angle < -15f)
        {
            animator.Play("turn left 90");
        }
        else
        {
            animator.Play("walk");
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
