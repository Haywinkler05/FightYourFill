using UnityEngine;
using UnityEngine.AI;

public class skeletonEnemy : FSM
{
    private NavMeshAgent agent; //This is specifically for the skeleton
    private Animator animator;

    [Header("Enemy Details")]
    [SerializeField] private float startingHealth = 50f; //Lets us modify the starting health and damage in the inspector for easier balancing
    [SerializeField] private float startingDamage = 5.0f;

    [Header("State Machine")]
    [SerializeField] private string currentStateName; //Lets us see the current state the skeleton is in
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        Health = startingHealth;
        Damage = startingDamage;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.Warp(transform.position);
        base.Start();
       
    }


    protected override void intializeStates()
    {
        currentState = new wanderState(agent, animator, this);
        currentStateName = currentState.GetType().Name;
    }

    protected override void Update()
    {
        currentStateName = currentState.GetType().Name; //Updates the current state to the state the skeleton is in
        base.Update();
    }
    
}
