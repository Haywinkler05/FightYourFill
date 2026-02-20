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
    private float timer;
    string walkAnim;
    string turnRightAnim;
    string turnLeftAnim;
    private string currentAnim;

    public wanderState( NavMeshAgent agent, Animator animator, float radius = 10f, float timeDuration = 3f, string walkAnim = "walk forward ", string turnLeftAnim = "Turn Left 90 Degrees", string turnRightAnim = "Turn Right 90 Degrees")
    {
        wanderRadius = radius; 
        wanderTimer = timeDuration;
        this.walkAnim = walkAnim;
        this.turnLeftAnim = turnLeftAnim;
        this.turnRightAnim = turnRightAnim;
        this.agent = agent;
        this.animator = animator;
       
    }
    
    private void playAnim(string anim)
    {
        Debug.Log("Playing anim");
        if (currentAnim == anim) return;
        currentAnim = anim;
        animator.Play(anim);
    }

    public void onEnter()
    {
        playAnim(walkAnim);

        timer = wanderTimer;
    }

    public void onExit()
    {
        
    }

    public void update()
    {
   
        timer += Time.deltaTime;

        if (timer >= wanderTimer) {
                Vector3 newPos = RandomNavSphere(agent.transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
        }

        float angle = Vector3.SignedAngle(agent.transform.forward, agent.velocity, Vector3.up);

        if(angle > 15f)
        {
            playAnim(turnRightAnim);
        }else if(angle < -15f)
        {
            playAnim(turnLeftAnim);
        }
        else
        {
            playAnim(walkAnim);
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
