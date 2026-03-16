using UnityEngine;

public class doctorEnemy : Enemy
{

    [Header("Experience")]
    public int expAmount = 300;

    [Header("Item Drops")]
    [SerializeField] private GameObject doctorDrop;
    [SerializeField] public int doctorDropNum = 2;

    [Header("Summon State")]
    public GameObject[] summonPrefabs; // drag in skeleton and zombie prefabs in any order
    public float summonRadius = 3f;
    [SerializeField]
    private float summonHealthThreshold = 70f;
    [SerializeField]
    public bool hasSummoned = false;
    public AnimationClip summonStateHit;
    public AnimationClip summonStateCast;
    [Header("StateMachine")]
    [SerializeField]
    private string CurrentStateName;

    protected override void Start()
    {
        base.Start();
        Drop = doctorDrop;
        dropNum = doctorDropNum;
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
        if (!hasSummoned && Health <= summonHealthThreshold)
        {
            SetInvulnerable(true);
            hasSummoned = true;
            SetState(new doctorSummonState(this));
        }
    }

    [ContextMenu("Test Summoning")]
    public void TestSummoning()
    {
        hasSummoned = false;
        SetState(new doctorSummonState(this));
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
