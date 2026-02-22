using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class wanderState : IState //Takes from the IState class
{

    private Enemy enemy;
    private float stuckTimer;

    public wanderState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void onEnter()
    {
        Debug.Log("In the wander state!!!");
        stuckTimer = 0;


        Vector3 newPos = RandomNavSphere(enemy.Agent.transform.position, enemy.wanderRadius, -1);
        enemy.Agent.SetDestination(newPos);

        enemy.Animator.CrossFadeInFixedTime(enemy.walkClip.name, enemy.crossFadeAnimSpeed);
    }

    public void onExit()
    {
        
    }

    public void update()
    {
        stuckTimer += Time.deltaTime;
        if (!enemy.Agent.pathPending)
        {
            // 2. Check if we are within the stopping distance
            if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
            {
     
                if (!enemy.Agent.hasPath || enemy.Agent.velocity.sqrMagnitude < 0.1f)
                {
                    enemy.SetState(new idleState(enemy));
                    return; // Exit the loop immediately so nothing else runs
                }
            }
        }

        // Fail-safe in case he gets stuck behind a wall
        if (stuckTimer > 10f)
        {
            enemy.SetState(new idleState(enemy));
        }

    }
    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
