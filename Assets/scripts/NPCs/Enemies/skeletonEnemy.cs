using UnityEngine;
using UnityEngine.AI;

public class skeletonEnemy : Enemy
{

    [Header("Experience")]

    public int expAmount = 100;

    [Header("Skeleton Drops")]
    [SerializeField] private GameObject skeletonDrop;
    [SerializeField] public int skeletonDropNum = 2;

    [Header("Skeleton Special State")]
    [SerializeField] private float screamHealthThreshold = 30f;
    [SerializeField] private float screamDamageBuff = 10f;
    [SerializeField] bool hasScreamed = false;
    [SerializeField] public AnimationClip skeletonScream;
    [SerializeField] public AudioClip scream;

   
    [Header("State Machine")]
    [SerializeField] private string currentStateName;
    protected override void Start()
    {
        base.Start();
        Drop = skeletonDrop;
        dropNum = skeletonDropNum;
    }
    protected override void intializeStates()
    {
        currentState = new spawnState(this);
        currentStateName = currentState.GetType().Name;
    }

    protected override void Update() { 
    
        base.Update();
        currentStateName = currentState.GetType().Name;
        if(Health == screamHealthThreshold)
        {
            setScreamState();
        }

        
    }

   
    public void setScreamState()
    {
        hasScreamed = false;
        
        SetState(new skeletonScreamState(this));
    }

    protected override void Die()
    {
        ExperienceManager.Instance.AddExperience(expAmount);//To add XP
        base.Die();
    }

    private void onFootStep()
    {
        if (wanderSFX == null) return;
        audioPlayerSFX.pitch = Random.Range(pitchMin, pitchMax);
        audioPlayerSFX.PlayOneShot(wanderSFX);
    }
    private void OnDrawGizmos()
    {
        // Only draw if the game is playing and the Agent actually exists and has a path
        if (Application.isPlaying && Agent != null && Agent.hasPath)
        {
            // 1. Draw a green line connecting the skeleton to its destination
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, Agent.destination);

            // 2. Draw a red sphere at the EXACT target coordinate
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Agent.destination, 0.2f);

            // 3. Draw a blue wire sphere showing your Stopping Distance!
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Agent.destination, Agent.stoppingDistance);
        }
    }
}
