using UnityEngine;

public class zombieEnemy : Enemy
{
    [Header("Zombie State Info")]
    [SerializeField] public bool zombieRevived = false;
    [SerializeField] public AnimationClip zombieRevive;
    [SerializeField] public float zombieReviveHealth;
    [SerializeField] public float lieDelay = 3f;

    [Header("Zombie Drops")]
    [SerializeField] private GameObject zombieDrop;
    [SerializeField] public int zombieDropNum = 2;

    [Header("State Machine")]
    [SerializeField] private string currentStateName;
    protected override void intializeStates()
    {
        currentState = new spawnState(this);
        currentStateName = currentState.GetType().Name;  
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        currentStateName = currentState.GetType().Name;
    }

    protected override void Die()
    {
        if (!zombieRevived)
        {
            // First death � revive instead
            zombieRevived = true;
            SetState(new zombieReviveState(this));
        }
        else
        {
            // Already revived � actually die
            base.Die();
        }
    }

    [ContextMenu("Test Revive")]
    public void TestRevive()
    {
        zombieRevived = false; // reset so it can revive again
        Health = 0;            // trigger death condition
        Die();                 // manually call Die()
    }

}
