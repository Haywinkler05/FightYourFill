using UnityEngine;

public class chaseState : IState
{
    //Enemy has spotted player
    //Give the player a 1 second grace period to hide
    //We need to increase the movement speed and switch to a running animation
    //Do math to navigate enemy to player
    //Switch to the attack state
    //Enter into search state
    private Enemy enemy;

    public chaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void onEnter()
    {
        Debug.Log("In the chase state");
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (distanceToPlayer <= enemy.Agent.stoppingDistance) {
            Debug.Log("Entering Attack State");
        }
        
        
    }

    public void onExit()
    {
        
    }

    public void update()
    {
        
    }

   
}
