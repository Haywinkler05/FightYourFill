using UnityEngine;

public class golemEnemy : Enemy
{

    [Header("Experience")]
    public int expAmount = 300;

    [Header("Item Drops")]
    [SerializeField] private GameObject golemDrop;
    [SerializeField] public int golemDropNum = 2;

    [Header("Golem Slam State")]
    [SerializeField]
    private float slamHealthThreshold = 60f;
    [SerializeField]
    bool hasSlammed = false;
    public AnimationClip slamState;
    public AnimationClip slamStateAttack;
    public float golemSlamForce = 16f;
    public float golemSlamDamage = 15f;
    public float slamRadius = 6f;
    public float slamUpwardBias = 0.65f; // Range between 0 and 1, inclusive. 0 means no vertical momentum, and 1 means only straight up.
    public LayerMask playerLayer;

    [Header("StateMachine")]
    [SerializeField]
    private string CurrentStateName;

    protected override void Start()
    {
        base.Start();
        Drop = golemDrop;
        dropNum = golemDropNum;
    }

    protected override void intializeStates()
    {
        currentState = new spawnState(this);
        CurrentStateName = currentState.GetType().Name;
    }

    protected override void Update()
    {
        base.Update();
        CurrentStateName = currentState.GetType().Name;
        if (!hasSlammed && Health <= slamHealthThreshold)
        {
            hasSlammed = true;
            SetState(new golemSlamState(this));
        }
    }

    [ContextMenu("Test Slam")]
    public void TestSlam()
    {
        hasSlammed = false;
        SetState(new golemSlamState(this));
    }

    protected override void Die()
    {   
        ExperienceManager.Instance.AddExperience(expAmount);//To add XP
        base.Die();
    }

    private void onFootFall()
    {
        if(wanderSFX == null)
        {
            return;
        }
        audioPlayerSFX.pitch = Random.Range(pitchMin, pitchMax);
        audioPlayerSFX.PlayOneShot(wanderSFX);
    }

}
