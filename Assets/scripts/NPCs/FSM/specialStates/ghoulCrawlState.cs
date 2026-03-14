using UnityEngine;

public class ghoulCrawlState : IState
{
    private ghoulEnemy enemy;
    private float timer;
    private float stuckTimer;

    public ghoulCrawlState(ghoulEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void onEnter()
    {
        timer = 0f;
        stuckTimer = 0f;
        enemy.SetInvulnerable(true);
        enemy.Agent.speed = enemy.sprintSpeed * enemy.crawlSpeedMutiplier;
        
        enemy.Animator.CrossFadeInFixedTime(enemy.crawl.name, enemy.crossFadeAnimSpeed);
        SetRandomDestination();
    }

    public void update()
    {
        timer += Time.deltaTime;
        stuckTimer += Time.deltaTime;

        enemy.Heal(enemy.regenHealth * Time.deltaTime);

        // Pick new destination when reached or stuck
        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            SetRandomDestination();
            stuckTimer = 0f;
        }

        // Force new destination if stuck for 2 seconds
        if (stuckTimer >= 2f)
        {
            SetRandomDestination();
            stuckTimer = 0f;
        }

        if (timer >= enemy.frenzyDuration)
            enemy.SetState(new chaseState(enemy));
    }

    public void onExit()
    {
        enemy.SetInvulnerable(false);
        enemy.Agent.speed = enemy.normalSpeed;
        
        enemy.Animator.CrossFadeInFixedTime(enemy.runClip.name, enemy.crossFadeAnimSpeed);
    }

    private void SetRandomDestination()
    {
        Vector3 randomPos = enemy.RandomNavSphere(enemy.transform.position, enemy.wanderRadius * enemy.wanderRadiusMutiplier, -1);
        enemy.Agent.SetDestination(randomPos);
    }
}