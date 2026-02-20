using UnityEngine;
using UnityEngine.AI;

public class FSM : MonoBehaviour
{
    private IState currentState;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] private string currentStateName;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentState = new wanderState(agent.transform, agent, animator, 10, 3);
        currentState.onEnter();
        currentStateName = currentState.GetType().Name;
    }
    public void SetState(IState newState)
    {
        currentState.onExit();
        currentState = newState;
        currentState.onEnter(); 
    }


    private void Update()
    {
        currentState.update();
    }
}
