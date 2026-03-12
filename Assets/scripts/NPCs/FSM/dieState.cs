using UnityEngine;
public class dieState : IState
{
    private Enemy enemy;
    private float timer;
    private float lieTimer = 5f;
    public dieState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void onEnter()
    {
        musicManager.Instance?.onEnemyLost();
        enemy.Animator.CrossFadeInFixedTime(enemy.dieClip.name, enemy.crossFadeAnimSpeed);
        timer = 0f;
    }
    public void onExit()
    {
        
    }
    public void update()
    {
        timer += Time.deltaTime;
        if (timer >= enemy.dieClip.length && timer >= lieTimer)
        {
            enemy.enemyDrop(enemy.Drop, enemy.dropNum);
            enemy.destroyEnemy();
            
        }
    }

}