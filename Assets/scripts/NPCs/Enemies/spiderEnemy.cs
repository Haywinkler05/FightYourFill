using UnityEngine;

public class spiderEnemy : Enemy
{

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
