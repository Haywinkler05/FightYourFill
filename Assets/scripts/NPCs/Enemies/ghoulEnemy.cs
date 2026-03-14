using UnityEngine;

public class ghoulEnemy : Enemy
{
    [Header("Special State Info")]
    public AnimationClip crawl;
    public float crawlSpeedMutiplier;
    public float regenHealth;
    public float wanderRadiusMutiplier;
    public float frenzyDuration = 10f;
    [SerializeField] private float frenzyHealthThreshold = 30f;
    [SerializeField] private bool hasFrenzied = false;
    public AudioClip frenzySFX;

    [Header("State Machine")]
    [SerializeField] private string currentStateName;
    protected override void intializeStates()
    {
        currentState = new wanderState(this);
        currentStateName = currentState.GetType().Name;
    }

    protected override void Update()
    {

        base.Update();
        currentStateName = currentState.GetType().Name;
        if (!hasFrenzied && Health <= frenzyHealthThreshold)
        {
            hasFrenzied = true;
            SetState(new ghoulCrawlState(this));
        }
    }
    [ContextMenu("Test Frenzy")]
    public void TestFrenzy()
    {
        hasFrenzied = false;
        SetState(new ghoulCrawlState(this));
    }
}