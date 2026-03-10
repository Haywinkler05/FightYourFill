using UnityEngine;

public class spawnState : IState
{
    private Enemy enemy;
    private float timer;
    public spawnState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void onEnter()
    {

        enemy.Agent.isStopped = true;
        enemy.Animator.Play(enemy.spawnClip.name);
        timer = 0f;
       
    }

    public void onExit()
    {
     enemy.Agent.isStopped = false;   
    }

    public void update()
    {
        timer += Time.deltaTime;  
        if(timer >= enemy.spawnClip.length)
        {
            enemy.Agent.isStopped = false;
            enemy.SetState(new wanderState(enemy));
        }
    }

  
}
