using UnityEngine;
using UnityEngine.AI;

public class idleState : IState
{

    private Enemy enemy;
    private float idleTime;
    private float timer;


    public idleState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void onEnter()
    {
        Debug.Log("In the idle state!");
        enemy.Agent.isStopped = true;
        enemy.Agent.velocity = Vector3.zero;
        enemy.Agent.ResetPath();
        timer = 0;
        idleTime = Random.Range(15f, 20f);


        enemy.Animator.CrossFadeInFixedTime(enemy.idleClip.name, enemy.crossFadeAnimSpeed);
    }

    public void onExit()
    {
       
    }

    public void update()
    {
        timer += Time.deltaTime;
        if (timer > idleTime) {
            enemy.SetState(new wanderState(enemy));
        }
    }
}
