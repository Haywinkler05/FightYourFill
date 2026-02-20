using UnityEngine;
using UnityEngine.AI;

public class skeletonEnemy : FSM
{
    private NavMeshAgent agent;
    private Animator animator;
    [SerializeField] private string currentStateName;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        base.Start();
       
    }


    protected override void intializeStates()
    {
        currentState = new wanderState(agent, animator, this);
        currentState.onEnter();
        currentStateName = currentState.GetType().Name;
    }

    protected override void Update()
    {
        currentStateName = currentState.GetType().Name;
        base.Update();
    }
    
}
