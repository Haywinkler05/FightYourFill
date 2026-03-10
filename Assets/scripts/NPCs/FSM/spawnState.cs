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

        float delay = Random.Range(0, enemy.maxSFXDelay);
        enemy.PlaySFXDelayed(enemy.spawnSFX, delay);
       
    }

    public void onExit()
    {
      enemy.FadeOutAudio(enemy.fadeOutTime);
     enemy.Agent.isStopped = false;   
    }

    public void update()
    {
        timer += Time.deltaTime;  
        if(timer >= enemy.spawnClip.length)
        {
            enemy.SetState(new wanderState(enemy));
        }
    }
    
}
