using UnityEngine;

public class ogreRageState : IState
{
    private ogreEnemy enemy;
    public ogreRageState(ogreEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void onEnter()
    {
        throw new System.NotImplementedException();
    }

    public void onExit()
    {
        throw new System.NotImplementedException();
    }

    public void update()
    {
        enemy.Heal(enemy.ogre_rage_health_bonus);
    }
}
