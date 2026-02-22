using UnityEngine;
using UnityEngine.AI;


public abstract class Enemy : FSM
{
    [Header("Universal Stats")]
    [SerializeField] protected float startingHealth = 50f; 
    [SerializeField] protected float startingDamage = 5f;
    [SerializeField] protected float startingSightRange = 10f;
    [SerializeField] protected float startingWanderRadius = 5f;


    public float Health { get; protected set; }
    public float Damage { get; protected set; }

    public float SightRange { get; protected set; }

    public float wanderRadius { get; protected set; }

    public GameObject Drop {  get; protected set; }

    [Header("Unviersal Components")]
    [field: SerializeField] public NavMeshAgent Agent { get; protected set; }
    [field: SerializeField] public Animator Animator { get; protected set; }


    [Header("Universal Animatons")]
    public AnimationClip walkClip;
    public AnimationClip idleClip;
    public AnimationClip turnLeftClip;
    public AnimationClip turnRightClip;
    public float crossFadeAnimSpeed;

   


    protected override void Start()
    {

        Health = startingHealth;
        Damage = startingDamage;
        SightRange = startingSightRange;
        wanderRadius = startingWanderRadius;
        if(Agent == null)
        {
            Agent = GetComponent<NavMeshAgent>();
        }
        if (Animator == null)
        {
            Animator = GetComponent<Animator>();
        }
        base.Start();
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if(Health<= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    
}
