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
        enemy.Animator.CrossFadeInFixedTime(enemy.dieClip.name, enemy.crossFadeAnimSpeed);
        timer = 0f;
    }
    public void onExit()
    {
        throw new System.NotImplementedException();
    }
    public void update()
    {
        timer += Time.deltaTime;
        if (timer >= enemy.dieClip.length && timer >= lieTimer)
        {
            enemy.destoryEnemy();
            //This will drop the cooking item
        }
    }

}