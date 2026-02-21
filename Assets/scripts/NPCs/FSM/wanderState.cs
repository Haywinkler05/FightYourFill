using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class wanderState : IState //Takes from the IState class
{
    public float wanderRadius; //These are our variables that help make it scalable
    private NavMeshAgent agent;
    private Animator animator;
    private FSM fsm; //FSM so I can switch states
    private float wanderTimer = 5f;
    private float timer;
    string walkAnim;
    string turnRightAnim;
    string turnLeftAnim;
    string idleAnim;
    private string currentAnim;

    public wanderState(NavMeshAgent agent, Animator animator, FSM fsm, float radius = 5f, string walkAnim = "root|walk forward ", 
        string turnLeftAnim = "root|Turn Left 90 Degrees", string turnRightAnim = "root|Turn Right 90 Degrees", string idleAnim = "root|combat idle") //Constructor class
    {
        wanderRadius = radius; 
        this.walkAnim = walkAnim;
        this.turnLeftAnim = turnLeftAnim;
        this.turnRightAnim = turnRightAnim;
        this.agent = agent;
        this.animator = animator;
        this.idleAnim = idleAnim;
        this.fsm = fsm;
       
    }
    
    private void playAnim(string anim) //Plays the animation once instead of restarting it every frame
    {
     
        if (currentAnim == anim) return;
        currentAnim = anim;
        animator.Play(anim);
    }

    public void onEnter() //Starts the animation and timer
    {
        Vector3 newPos = RandomNavSphere(agent.transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
        timer = 0;
    }

    public void onExit()
    {
        
    }

    public void update()
    {
        timer += Time.deltaTime; 
        if (timer >= wanderTimer) //If the NPC has wandered enough, they will stop and get a new pos
        {
            fsm.SetState(new idleState(agent,animator, fsm));
            timer = 0;
        }

        float angle = Vector3.SignedAngle(agent.transform.forward, agent.velocity, Vector3.up); //Fancy math that determines orientation of the NPC
        if (angle > 15f)
            playAnim(turnRightAnim);
        else if (angle < -15f)
            playAnim(turnLeftAnim);
        else
            playAnim(walkAnim);
    }


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) 
    {
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * dist; //Gets a random direction inside the unit sphere and mutiplies it by the distance assigned by the constructer
       
      

        randDir += origin; //Takes into the account of the NPC location

       NavMeshHit hit; 
        if (NavMesh.SamplePosition(randDir, out hit, dist, layermask)) //Places the pos on the navmesh
        {
            return hit.position; //If valid have the NPC get that position to navigate too
        }

        
        return origin; //Otherwise forces NPC to stay until its timer is up
    }
}
