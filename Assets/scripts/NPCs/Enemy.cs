using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;


public abstract class Enemy : FSM
{
    
    [Header("Universal Stats")]
    [SerializeField] protected float startingHealth = 50f;
    [SerializeField] protected float startingDamage = 5f;
    [SerializeField] protected float IFrameDuration = 0.3f;
    [SerializeField] private Ray sight;
    [Header("Drops")]
    [SerializeField] private int startingDropNum = 1;
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
    public float roatationSpeed = 300f;
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
    [Range(0f,1f)] public float walkSFXVolume = 0.5f;
    public float pitchMin = 0.9f;
    public float pitchMax = 1.1f;
    public AudioSource audioPlayerSFX;
    public AudioClip spawnSFX;
    public AudioClip idleSFX;
    public AudioClip wanderSFX;
    public AudioClip attackSFX;
    public AudioClip takeDamageSFX;


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

    public bool isInvulnerable { get; protected set; }
    public float normalSpeed { get; protected set; }
    public float sprintSpeed { get; protected set; }

    public float searchTime { get; protected set; }

    public float maxSFXDelay { get; protected set; }

    public float fadeOutTime { get; protected set; }
    public GameObject Drop {  get; protected set; }
    public int dropNum { get; protected set; }

    public bool isDead { get; private set; }




    [Header("Universal Animatons")]
    public AnimationClip spawnClip;
    public AnimationClip dieClip;
    public AnimationClip walkClip;
    public AnimationClip idleClip;
    public AnimationClip runClip;
    public AnimationClip Attack1Clip;
    public AnimationClip Attack2Clip;
    public AnimationClip Attack3Clip;
    public AnimationClip takeDamageClip;
    public float crossFadeAnimSpeed;

    //Experience related
    int expAmount = 100;


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

        dropNum = startingDropNum;


        if (audioPlayerSFX == null)
        {
            audioPlayerSFX = GetComponent<AudioSource>();
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
        
        base.Start();
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;
        if (isDead) return;
        Health -= damage;
        PlaySFX(takeDamageSFX);
        if (Health <= 0)
        {
            Die();
            return;
        }
        if (!(currentState is attackState))
            SetState(new attackState(this));
        if (takeDamageClip != null) { 
            Animator.CrossFadeInFixedTime(takeDamageClip.name, crossFadeAnimSpeed);
            StartCoroutine(InvulnerabilityWindow(takeDamageClip.length));
        }
        else
        {
           
            StartCoroutine(InvulnerabilityWindow(IFrameDuration));
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

    public void destroyEnemy()
    {
        Destroy(this.gameObject);
    }
    protected virtual void Die()
    {
        isDead = true;
        SetInvulnerable(true);
        Agent.isStopped = true;
        ExperienceManager.Instance.AddExperience(expAmount);//To add XP
        SetState(new dieState(this));
    }


    public void PlaySFX(AudioClip clip)
    {
        if (audioPlayerSFX != null && clip != null)
            audioPlayerSFX.PlayOneShot(clip);
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
        float startVolume = audioPlayerSFX.volume;
        while (audioPlayerSFX.volume > 0)
        {
            audioPlayerSFX.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }
        audioPlayerSFX.Stop();
        audioPlayerSFX.volume = startVolume;
    }
    public void scaleEnemy(float scaleMutiplier)
    {
        Health *= scaleMutiplier;
        Damage *= scaleMutiplier;
    }
    public void Heal(float amount)
    {
        Health = Mathf.Min(Health + amount, startingHealth);
    }

    public void buffDamage(float amount)
    {
        Damage += amount;
    }
  
    public void enemyDrop(GameObject drop, int dropNum)
    {
        if (drop == null) return;
        for (int i = 0; i < dropNum; i++)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * 0.5f;
            spawnPos.y = transform.position.y; 
            Instantiate(drop, spawnPos, Quaternion.identity);
        }
    }

    public void SetInvulnerable(bool state)
    {
        isInvulnerable = state;
    }

    private System.Collections.IEnumerator InvulnerabilityWindow(float duration)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        if(!isDead) isInvulnerable = false;
    }
}
