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
        Vector3 newPos = RandomNavSphere(enemy.Agent.transform.position, enemy.wanderRadius, -1);
        enemy.Agent.SetDestination(newPos);
    }

    public void onExit() {
        enemy.audioPlayer.Stop();
    
    }

    public void update()
    {
        if (enemy.seePlayer())
        {
            enemy.SetState(new chaseState(enemy));
            return;
        } 
        if (enemy.Agent.velocity.sqrMagnitude > 0.1f)
        {
            if (!enemy.audioPlayer.isPlaying)
            {
                enemy.audioPlayer.clip = enemy.wanderSFX;
                enemy.audioPlayer.loop = true;
                enemy.audioPlayer.Play();
            }
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
    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}