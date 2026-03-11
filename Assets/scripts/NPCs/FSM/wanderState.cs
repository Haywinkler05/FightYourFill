using UnityEngine;
using UnityEngine.AI;

public class wanderState : IState
{
    private Enemy enemy;
    private float stuckTimer;
    private string currentAnim;

    public wanderState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void onEnter()
    {
        stuckTimer = 0;
        enemy.Agent.speed = enemy.normalSpeed;
        Vector3 newPos = enemy.RandomNavSphere(enemy.Agent.transform.position, enemy.wanderRadius, -1);
        enemy.Agent.SetDestination(newPos);
    }

    public void onExit() {
        enemy.audioPlayer.Stop();
    
    }

    public void update()
    {
        if (enemy.HasLineOfSightToPlayer(isChasing: false))
        {
            enemy.SetState(new chaseState(enemy));
            return;
        } 
        if (enemy.Agent.velocity.sqrMagnitude > 0.1f)
        {
            playAnim(enemy.walkClip.name);
        }

        stuckTimer += Time.deltaTime;

        if (!enemy.Agent.pathPending)
        {
            if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
            {
                if (!enemy.Agent.hasPath || enemy.Agent.velocity.sqrMagnitude == 0f)
                {
                    enemy.SetState(new idleState(enemy));
                    return;
                }
            }
        }

        if (stuckTimer > 10f)
        {
            enemy.SetState(new idleState(enemy));
        }
    }
    private void playAnim(string clipName)
    {
        if (currentAnim == clipName) return;
        currentAnim = clipName;
        enemy.Animator.CrossFadeInFixedTime(clipName, enemy.crossFadeAnimSpeed);
    }
    
}