
using UnityEngine;
using UnityEngine.AI;

public class searchState : IState
{
    private Enemy enemy;
    private Vector3 lastKnownPOS;
    private float searchTimer;

    private float lookAroundTimer;
    private float maxLookAroundTime = 2f;
    private bool isLookingAround;
    private float searchSpeed = 3f;
    public searchState(Enemy enemy, Vector3 lastKnownPOS)
    {
        this.enemy = enemy;
        this.lastKnownPOS = lastKnownPOS;
    }

    public void onEnter()
    {
        enemy.Agent.SetDestination(lastKnownPOS);
        enemy.Agent.speed = searchSpeed;
        enemy.Animator.CrossFadeInFixedTime(enemy.walkClip.name, enemy.crossFadeAnimSpeed);

        searchTimer = 0f;
        isLookingAround = false;
    }

    public void onExit()
    {
       
    }

    public void update()
    {
        if (enemy.HasLineOfSightToPlayer()) { 
            enemy.SetState(new chaseState(enemy));
            return;
        }

        searchTimer += Time.deltaTime;
        if (searchTimer > enemy.searchTime) { 
            enemy.SetState(new wanderState(enemy));
            return;
        }

        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            if (!isLookingAround)
            {
                isLookingAround = true;
                lookAroundTimer = 0f;
                enemy.Animator.CrossFadeInFixedTime(enemy.idleClip.name, enemy.crossFadeAnimSpeed);
            }
            else
            {
               lookAroundTimer += Time.deltaTime;
                if(lookAroundTimer >= maxLookAroundTime)
                {
                    isLookingAround = false;
                    enemy.Animator.CrossFadeInFixedTime(enemy.walkClip.name, enemy.crossFadeAnimSpeed);


                    Vector3 newSearchPos = RandomNavSphere(lastKnownPOS, 5f, -1);
                    enemy.Agent.SetDestination(newSearchPos);
                }
            }
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
