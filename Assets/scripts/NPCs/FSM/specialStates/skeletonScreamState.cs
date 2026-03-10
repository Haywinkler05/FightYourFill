using UnityEngine;

public class skeletonScreamState : IState
{
    private skeletonEnemy enemy;
    private float timer;
    public skeletonScreamState(skeletonEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void onEnter()
    {
        enemy.Agent.isStopped = true;
        enemy.Animator.CrossFadeInFixedTime(enemy.skeletonScream.name, enemy.crossFadeAnimSpeed);

        timer = 0f;
    }

    public void onExit()
    {
        enemy.Agent.isStopped = false;
    }

    public void update()
    {
        timer += Time.deltaTime;

        if (timer >= enemy.skeletonScream.length) {
                enemy.SetState(new chaseState(enemy));        
        
        }
    }


}
