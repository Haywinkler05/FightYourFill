using UnityEngine;
using UnityEngine.AI;

public class idleState : IState
{
    private NavMeshAgent agent; //Makes the state scalable
    private Animator animator;
    private FSM fsm;
    private float idleTime;
    private float timer;
    private string idleAnim;
    

    public idleState(NavMeshAgent agent, Animator animator, FSM fsm, string idleAnim = "root|combat idle", float minTime = 15f, float maxTime = 20f ) //Constructor
    {
        this.agent = agent;
        this.animator = animator;
        idleTime = Random.Range(minTime, maxTime);
        this.idleAnim = idleAnim;
        this.fsm = fsm;
    }

    public void onEnter() //Clears the path and has the NPC idle
    {
        agent.ResetPath();
        timer = 0;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(idleAnim))
        {
            animator.CrossFadeInFixedTime(idleAnim, 0.2f);
        }
    }

    public void onExit()
    {
        
    }

    public void update() //Waits for the timer to run up then switches back to the wander state. Will be modifed when attack state is added
    {
        timer += Time.deltaTime;
        if(timer >= idleTime)
        {
            fsm.SetState(new wanderState(agent, animator, fsm));
        }
    }

   
}
