using UnityEngine;

public class spiderEnemy : Enemy
{
    [Header("Special Spider States")]
    [SerializeField] public GameObject spiderling;
    [SerializeField] public int spiderlingCount;
    [SerializeField] public float specialStateThreshold = 25f;
    [SerializeField] public float spiderDamageBuff = 1.5f;
    [SerializeField] public bool hasCocooned = false;
    [Header("State Machine")]
    [SerializeField] private string currentStateName;
    protected override void intializeStates()
    {
        currentState = new wanderState(this);
        currentStateName = currentState.GetType().Name;
    }

    private bool isPlayingWalkSound = false;

    protected override void Update()
    {
        base.Update();
        currentStateName = currentState.GetType().Name;

        if (currentState is wanderState || currentState is chaseState)
        {
            if (!isPlayingWalkSound)
            {
                audioPlayerSFX.loop = true;
                PlaySFX(wanderSFX);
                isPlayingWalkSound = true;
            }
        }
        else
        {
            if (isPlayingWalkSound)
            {
                audioPlayerSFX.Stop();
                audioPlayerSFX.loop = false;
                isPlayingWalkSound = false;
            }
        }

        if (!hasCocooned && Health <= specialStateThreshold) {
            hasCocooned = true;
            SetState(new spawnSpiderlingsState(this));
        }

    }
    protected override void Die()
    {
        base.Die();
    }

    [ContextMenu("Test Spawn Spiderlings")]
    public void TestSpawnSpiderlings()
    {
        SetState(new spawnSpiderlingsState(this));
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
