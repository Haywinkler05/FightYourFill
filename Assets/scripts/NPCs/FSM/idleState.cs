using UnityEngine;
using UnityEngine.AI;

public class idleState : IState
{
    private NavMeshAgent agent;
    private Animator animator;
    private FSM fsm;
    private float idleTime;
    private float timer;
    private string idleAnim;
    

    public idleState(NavMeshAgent agent, Animator animator, FSM fsm, string idleAnim = "root|combat Idle", float minTime = 15f, float maxTime = 20f )
    {
        this.agent = agent;
        this.animator = animator;
        idleTime = Random.Range(minTime, maxTime);
        this.idleAnim = idleAnim;
        this.fsm = fsm;
    }

    public void onEnter()
    {
        agent.ResetPath();
        animator.Play(idleAnim, 0);
        timer = 0;
    }

    public void onExit()
    {
        
    }

    public void update()
    {
        timer += Time.deltaTime;
        if(timer >= idleTime)
        {
            fsm.SetState(new wanderState(agent, animator, fsm));
        }
    }

   
}
