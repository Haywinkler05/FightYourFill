using UnityEngine;

public class zombieEnemy : Enemy
{
    [Header("Zombie State Info")]
    [SerializeField] public AnimationClip zombieRegen;

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
        if(Health == 0)
        {
            Die();
        }
    }
    protected override void Die()
    {
        SetState(new dieState(this));
    }
}
