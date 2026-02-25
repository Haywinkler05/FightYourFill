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
        enemy.Animator.Play(enemy.runClip.name);
        enemy.Agent.speed = enemy.sprintSpeed;
        
        
        
        
    }

    public void onExit()
    {
        enemy.Agent.speed = enemy.normalSpeed;
    }

    public void update()
    {
        if (enemy.HasLineOfSightToPlayer())
        {
            enemy.Agent.SetDestination(enemy.player.transform.position);
            if (!enemy.Agent.pathPending)
            {
                if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
                {

                }
            }
        }
        else
        {
           enemy.SetState(new idleState(enemy));
            return;
        }
       
    }

   
}
