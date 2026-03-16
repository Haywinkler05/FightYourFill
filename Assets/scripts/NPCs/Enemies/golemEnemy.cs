using UnityEngine;

public class golemEnemy : Enemy
{

    [Header("Experience")]
    public int expAmount = 300;

    [Header("Item Drops")]
    [SerializeField] private GameObject golemDrop;
    [SerializeField] public int golemDropNum = 2;

    [Header("Golem Special State")]
    [SerializeField]
    private float specialHealthThreshold = 60f;
    [SerializeField]
    bool hasRaged = false;
    public AnimationClip rageState;
    public AnimationClip rageStateAttack;
    private float ogreRageHealthBonus = 40f;
    public float rageHealthBonus => ogreRageHealthBonus;
    public GameObject[] ringPrefabs;
    public float ogreRingDamage = 12f;
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
        if (Health == 0)
        {
            Die();
        }
    }

    [ContextMenu("Test Rage")]
    public void TestRage()
    {
        hasRaged = false;
        Health = specialHealthThreshold - 1f; // force the condition
    }

    protected override void Die()
    {
        if (hasRaged)
        {
            ExperienceManager.Instance.AddExperience(expAmount);//To add XP
            base.Die();
        }
        else
        {
            SetState(new golemBoulderState(this));
            hasRaged = true;
        }
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
