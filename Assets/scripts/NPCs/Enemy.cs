using UnityEngine;
using UnityEngine.AI;


public abstract class Enemy : FSM
{
    
    [Header("Universal Stats")]
    [SerializeField] protected float startingHealth = 50f;
    [SerializeField] protected float startingDamage = 5f;
   
    [SerializeField] private Ray sight;

    [Header("State Specific Info")]
    
    [SerializeField] protected float startingWanderRadius = 5f;
    [SerializeField] protected float baseSpeed = 2f;
    [SerializeField] protected float baseSprint = 5f;
    [SerializeField] protected float startingSearchTime = 2f;
    [SerializeField] protected float startingEyeOffset = 0.5f;
    [SerializeField] protected float startingIdleMinTime = 2f;
    [SerializeField] protected float startingIdleMaxTime = 10f;

    [Header("Detecton")]
    [SerializeField] protected float startingWanderSightRange = 5f;
    [SerializeField] protected float startingChaseSightRange = 10f;
    [SerializeField] protected float startingWanderFOV = 60f;
    [SerializeField] protected float startingChaseFOV = 120f;
    [field: SerializeField] public NavMeshAgent Agent { get; protected set; }
    [field: SerializeField] public Animator Animator { get; protected set; }


    [Header("Player")]
    [SerializeField] public GameObject player;


    [Header("Game Scripts")]
   [SerializeField] protected AttackRanger combat;

    [Header("SFX")]
    [SerializeField] private float startingMaxSFXDelay = 1f;
    [SerializeField] private float startingFadeOutTime = 0.5f;
    public AudioSource audioPlayer;
    public AudioClip spawnSFX;
    public AudioClip idleSFX;
    public AudioClip wanderSFX;

    public float Health { get; protected set; }
    public float Damage { get; protected set; }

    public float wanderSightRange { get; protected set; }

    public float chaseSightRange { get; protected set; }

    public float wanderFOV { get; protected set; }

    public float chaseFOV { get; protected set; }

    public float wanderRadius { get; protected set; }

    public float eyeOffset { get; protected set; } = 0.5f;

    public float idleMinTime { get; protected set; } = 2f;

    public float idleMaxTime { get; protected set; } = 10f;

    public float normalSpeed { get; protected set; }
    public float sprintSpeed { get; protected set; }

    public float searchTime { get; protected set; }

    public float maxSFXDelay { get; protected set; }

    public float fadeOutTime { get; protected set; }
    public GameObject Drop {  get; protected set; }





    [Header("Universal Animatons")]
    public AnimationClip spawnClip;
    public AnimationClip dieClip;
    public AnimationClip walkClip;
    public AnimationClip idleClip;
    public AnimationClip runClip;
    public AnimationClip Attack1Clip;
    public AnimationClip Attack2Clip;
    public AnimationClip Attack3Clip;
    public float crossFadeAnimSpeed;



    protected override void Start()
    {

        Health = startingHealth;
        Damage = startingDamage;
        wanderSightRange = startingWanderSightRange;
        chaseSightRange = startingChaseSightRange;
        wanderFOV = startingWanderFOV;
        chaseFOV = startingChaseFOV;
        wanderRadius = startingWanderRadius;

        eyeOffset = startingEyeOffset;
        idleMinTime = startingIdleMinTime;
        idleMaxTime = startingIdleMaxTime;
        normalSpeed = baseSpeed;
        sprintSpeed = baseSprint;
        searchTime = startingSearchTime;

        fadeOutTime = startingFadeOutTime;
        maxSFXDelay = startingMaxSFXDelay;
        

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
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
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
    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
    public bool HasLineOfSightToPlayer(bool isChasing = false)
    {
        float range = isChasing ? chaseSightRange : wanderSightRange;
        float fov = isChasing ? chaseFOV : wanderFOV;

        Vector3 eyePosition = new Vector3(transform.position.x,
            transform.position.y + eyeOffset, transform.position.z);
        Vector3 targetPosition = player.transform.position;
        Vector3 directionToPlayer = (targetPosition - eyePosition).normalized;
        float distanceToPlayer = Vector3.Distance(eyePosition, targetPosition);

        // Distance check
        if (distanceToPlayer > range) return false;

        // FOV check
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > fov * 0.5f) return false;

        // Raycast
        Debug.DrawRay(eyePosition, directionToPlayer * distanceToPlayer, Color.yellow);
        RaycastHit hit;
        if (Physics.Raycast(eyePosition, directionToPlayer, out hit, range))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }

    public void destoryEnemy()
    {
        Destroy(this.gameObject);
    }
    protected virtual void Die()
    {
        Agent.isStopped = true;
    }


    public void PlaySFX(AudioClip clip)
    {
        if (audioPlayer != null && clip != null)
            audioPlayer.PlayOneShot(clip);
    }

    public void PlaySFXDelayed(AudioClip clip, float delay)
    {
        StartCoroutine(PlaySFXDelayedCoroutine(clip, delay));
    }

    public void FadeOutAudio(float duration = 0.5f)
    {
        StartCoroutine(FadeOutAudioCoroutine(duration));
    }

    private System.Collections.IEnumerator PlaySFXDelayedCoroutine(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySFX(clip);
    }

    private System.Collections.IEnumerator FadeOutAudioCoroutine(float duration)
    {
        float startVolume = audioPlayer.volume;
        while (audioPlayer.volume > 0)
        {
            audioPlayer.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }
        audioPlayer.Stop();
        audioPlayer.volume = startVolume;
    }

}
