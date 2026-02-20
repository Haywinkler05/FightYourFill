using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class wanderState : IState
{
    public float wanderRadius;

    

    private NavMeshAgent agent;
    private Animator animator;
    private FSM fsm;
    private float wanderTimer;
    private float timer;
    string walkAnim;
    string turnRightAnim;
    string turnLeftAnim;
    string idleAnim;
    private string currentAnim;

    public wanderState(NavMeshAgent agent, Animator animator, FSM fsm, float radius = 10f, float time = 5f, string walkAnim = "root|walk forward ", 
        string turnLeftAnim = "root|Turn Left 90 Degrees", string turnRightAnim = "root|Turn Right 90 Degrees", string idleAnim = "root|combat idle")
    {
        wanderRadius = radius; 
        wanderTimer = time;
        this.walkAnim = walkAnim;
        this.turnLeftAnim = turnLeftAnim;
        this.turnRightAnim = turnRightAnim;
        this.agent = agent;
        this.animator = animator;
        this.idleAnim = idleAnim;
        this.fsm = fsm;
       
    }
    
    private void playAnim(string anim)
    {
     
        if (currentAnim == anim) return;
        currentAnim = anim;
        animator.Play(anim);
    }
    
    public void onEnter()
    {
      
        Vector3 newPos = RandomNavSphere(agent.transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
        playAnim(walkAnim);
        timer = 0;

        
    }

    public void onExit()
    {
        
    }

    public void update()
    {

        timer += Time.deltaTime;
        if (timer >= wanderTimer)
        {
            fsm.SetState(new idleState(agent, animator, fsm, idleAnim));
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
