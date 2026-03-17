using UnityEngine;

public class dieState : IState
{
    private Enemy enemy;
    private float timer;
    private float lieTimer = 5f;
    private bool hasDropped = false; // add this

    public dieState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void onEnter()
    {
        musicManager.Instance?.onEnemyLost();
        if (enemy.Agent != null && enemy.Agent.isActiveAndEnabled && enemy.Agent.isOnNavMesh)
            enemy.Agent.isStopped = true;
        
        foreach (Collider col in enemy.GetComponentsInChildren<Collider>())
            col.enabled = false;
        enemy.Animator.CrossFadeInFixedTime(enemy.dieClip.name, enemy.crossFadeAnimSpeed);
        timer = 0f;
        hasDropped = false;
    }

    public void update()
    {
        if (hasDropped) return; 
        timer += Time.deltaTime;
        if (timer >= enemy.dieClip.length + lieTimer)
        {
            hasDropped = true;
            enemy.enemyDrop(enemy.Drop, enemy.dropNum);
            enemy.destroyEnemy();
        }
    }

    public void onExit() { }
}