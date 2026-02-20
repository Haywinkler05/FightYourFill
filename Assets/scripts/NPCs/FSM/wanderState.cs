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
    string walkAnim;
    string turnRightAnim;
    string turnLeftAnim;

    public wanderState(Transform target, NavMeshAgent agent, Animator animator, float radius = 10f, float timeDuration = 3f, string walkAnim = "walk", string turnLeftAnim = "turn left 90", string turnRightAnim = "turn right 90")
    {
        wanderRadius = radius; 
        wanderTimer = timeDuration;
        this.walkAnim = walkAnim;
        this.turnLeftAnim = turnLeftAnim;
        this.turnRightAnim = turnRightAnim;
        this.agent = agent;
        this.target = target;
        this.animator = animator;
    }
    


    public void onEnter()
    {
        animator.Play(walkAnim);

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
            animator.Play(turnRightAnim);
        }else if(angle < -15f)
        {
            animator.Play(turnLeftAnim);
        }
        else
        {
            animator.Play(walkAnim);
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
