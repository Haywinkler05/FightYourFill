using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class wanderState : IState //Takes from the IState class
{

    private Enemy enemy;
    private float timer;
    private float wanderTime;

    public wanderState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public void onEnter()
    {
        timer = 0;
        wanderTime = UnityEngine.Random.Range(3f, 7f); //Gets a random range for wander time


        Vector3 newPos = RandomNavSphere(enemy.Agent.transform.position, enemy.wanderRadius, -1);
        enemy.Agent.SetDestination(newPos);

        enemy.Animator.Play(enemy.walkClip.name);
    }

    public void onExit()
    {
        
    }

    public void update()
    {
        timer += Time.deltaTime;

        bool arrived = !enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance;
        if (arrived || timer > wanderTime) {
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
