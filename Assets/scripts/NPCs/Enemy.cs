using UnityEngine;
using UnityEngine.AI;


public abstract class Enemy : FSM
{
    [Header("Universal Stats")]
    [SerializeField] protected float startingHealth = 50f;
    [SerializeField] protected float startingDamage = 5f;
    [SerializeField] protected float startingSprintSpeed = 5f;
    [SerializeField] protected float startingSightRange = 10f;
    [SerializeField] protected float startingWanderRadius = 5f;
    [SerializeField] private Ray sight;
    [field: SerializeField] public NavMeshAgent Agent { get; protected set; }
    [field: SerializeField] public Animator Animator { get; protected set; }

    [Header("Enemy Characteristics")]
    [SerializeField] protected float startingEyeOffset = 0.5f;
    [SerializeField] protected float startingIdleMinTime = 2f;
    [SerializeField] protected float startingIdleMaxTime = 10f;


    [Header("Player Specific Information")]
    public GameObject player;

    [Header("SFX")]
    public AudioSource audioPlayer;
    public AudioClip idleSFX;
    public AudioClip wanderSFX;

    public float Health { get; protected set; }
    public float Damage { get; protected set; }

    public float SightRange { get; protected set; }

    public float wanderRadius { get; protected set; }

    public float eyeOffset { get; protected set; } = 0.5f;

    public float idleMinTime { get; protected set; } = 2f;

    public float idleMaxTime { get; protected set; } = 10f;

    public float sprintSpeed { get; protected set; }
    public GameObject Drop {  get; protected set; }

   
  


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

        eyeOffset = startingEyeOffset;
        idleMinTime = startingIdleMinTime;
        idleMaxTime = startingIdleMaxTime;
        sprintSpeed = startingSprintSpeed;
        if(audioPlayer == null)
        {
            audioPlayer = GetComponent<AudioSource>();
        }
        if(Agent == null)
        {
            Agent = GetComponent<NavMeshAgent>();
        }
        if (Animator == null)
        {
            Animator = GetComponent<Animator>();
        }
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
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

    public bool seePlayer(float range)
    {
        sight.origin = new Vector3(transform.position.x, transform.position.y + eyeOffset, transform.position.z);
        sight.direction = transform.forward;

        RaycastHit rayHit;
        Debug.DrawRay(sight.origin, sight.direction * range, Color.red);

        if (Physics.Raycast(sight, out rayHit, range)){
                if(rayHit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        
        return false;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    
}
