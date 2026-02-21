using UnityEngine;
using UnityEngine.AI;

public class skeletonEnemy : FSM
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Enemy Details")]
    [SerializeField] private float startingHealth = 50f;
    [SerializeField] private float startingDamage = 5.0f;

    [Header("State Machine")]
    [SerializeField] private string currentStateName;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        Health = startingHealth;
        Damage = startingDamage;
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
