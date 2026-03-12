using UnityEngine;

public class chaseState : IState
{
    private Enemy enemy;


    public chaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void onEnter()
    {
        musicManager.Instance?.onEnemyChase();
        enemy.Animator.Play(enemy.runClip.name);
        enemy.Agent.speed = enemy.sprintSpeed;
        
        
        
        
    }

    public void onExit()
    {
        musicManager.Instance?.onEnemyLost();
        enemy.Agent.speed = enemy.normalSpeed;
    }

    public void update()
    {
        if (enemy.HasLineOfSightToPlayer(isChasing: true))
        {
            enemy.Agent.SetDestination(enemy.player.transform.position);
            if (!enemy.Agent.pathPending)
            {
                if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
                { 
                    enemy.SetState(new attackState(enemy));
                }
            }
        }
        else
        {
           enemy.SetState(new searchState(enemy, enemy.player.transform.position));
            return;
        }
       
    }

   
}
