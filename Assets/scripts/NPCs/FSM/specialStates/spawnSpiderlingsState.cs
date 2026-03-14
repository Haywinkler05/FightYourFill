using UnityEngine;

public class spawnSpiderlingsState : IState
{

    private spiderEnemy enemy;
    private float timer;

    public spawnSpiderlingsState(spiderEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void onEnter()
    {
        timer = 0f;
        enemy.SetInvulnerable(true);
        enemy.Agent.isStopped = true;
        enemy.Animator.CrossFadeInFixedTime(enemy.idleClip.name, enemy.crossFadeAnimSpeed);

        // Spawn spiderlings
        for (int i = 0; i < enemy.spiderlingCount; i++)
        {
            Vector3 spawnPos = enemy.RandomNavSphere(enemy.transform.position, 3f, -1);
            GameObject.Instantiate(enemy.spiderling, spawnPos, Quaternion.identity);
        }
    
}

    public void onExit()
    {
        enemy.SetInvulnerable(false);
        enemy.Agent.isStopped = false;
    }

    public void update()
    {
        timer += Time.deltaTime;

        // Wait briefly then chase
        if (timer >= enemy.idleClip.length)
        {
            enemy.buffDamage(enemy.spiderDamageBuff);
            enemy.SetState(new chaseState(enemy));
        }
    }
}
