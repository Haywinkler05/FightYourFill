using UnityEngine;

public class zombieReviveState : IState
{
    private zombieEnemy enemy;
    private float timer;
    private bool hasStartedRevive = false;

    public zombieReviveState(zombieEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void onEnter()
    {
        timer = 0f;
        enemy.SetInvulnerable(true);
        enemy.Agent.isStopped = true;
        enemy.Animator.CrossFadeInFixedTime(enemy.dieClip.name, enemy.crossFadeAnimSpeed);
    }

    public void onExit()
    {
        enemy.Agent.isStopped = false;
        enemy.SetInvulnerable(false);
    }

    public void update()
    {
        timer += Time.deltaTime;

        // Wait on ground, then play revive animation
        if (!hasStartedRevive && timer >= enemy.dieClip.length + enemy.lieDelay)
        {
            hasStartedRevive = true;
            enemy.Heal(enemy.zombieReviveHealth);
            enemy.Animator.CrossFadeInFixedTime(enemy.zombieRevive.name, enemy.crossFadeAnimSpeed);
        }

        // Phase 2 - wait for revive animation then chase
        if (hasStartedRevive && timer >= enemy.dieClip.length + enemy.lieDelay + enemy.zombieRevive.length)
        {
            enemy.SetState(new chaseState(enemy));
        }
    }



}
