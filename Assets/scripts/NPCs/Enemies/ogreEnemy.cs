using UnityEngine;

public class ogreEnemy : Enemy
{

    [Header("Experience")]
    public int expAmount = 300;

    [Header("Item Drops")]
    [SerializeField] private GameObject ogreDrop;
    [SerializeField] public int ogreDropNum = 2;

    [Header("Ogre Special State")]
    [SerializeField]
    private float rageHealthThreshold = 1f;
    [SerializeField]
    bool hasRaged = false;
    public AnimationClip rageState;
    public AnimationClip rageStateAttack;
    private float ogreRageHealthBonus = 50f;
    public float rageHealthBonus => ogreRageHealthBonus;
    public GameObject ring1Prefab;
    public GameObject ring2Prefab;
    public GameObject ring3Prefab;
    public float rageRing1Radius = 3f;
    public float rageRing2Radius = 6f;
    public float rageRing3Radius = 9f;
    public float ogreRingDamage = 12f;
    [Header("StateMachine")]
    [SerializeField]
    private string CurrentStateName;

    protected override void Start()
    {
        base.Start();
        Drop = ogreDrop;
        dropNum = ogreDropNum;
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
        Health = rageHealthThreshold - 1f; // force the condition
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
            SetState(new ogreRageState(this));
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
