using UnityEngine;
using UnityEngine.AI;

public class skeletonEnemy : Enemy
{
    [Header("State Machine")]
    [SerializeField] private string currentStateName;
    protected override void intializeStates()
    {
        currentState = new wanderState(this);
        currentStateName = currentState.GetType().Name;
    }

    protected override void Update() { 
    
        base.Update();
        currentStateName = currentState.GetType().Name;
    }
}
