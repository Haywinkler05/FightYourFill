using UnityEngine;
public class attackState : IState
{
    private Enemy enemy;
    private float attackDuration;
    private float timer;
    private int randomIndex;
    private AnimationClip[] attacks;
    private AnimationClip chosenClip;

    public attackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void onEnter()
    {
        attacks = new AnimationClip[] { enemy.Attack1Clip, enemy.Attack2Clip, enemy.Attack3Clip };
        randomIndex = Random.Range(0, attacks.Length);
        chosenClip = attacks[randomIndex];

        enemy.Animator.CrossFadeInFixedTime(chosenClip.name, enemy.crossFadeAnimSpeed);
        attackDuration = chosenClip.length;
        timer = 0f; 
    }

    public void update()
    {
        
        timer += Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (timer >= attackDuration)
        {
            if (distanceToPlayer <= enemy.Agent.stoppingDistance + 0.5f)
                enemy.SetState(new attackState(enemy));
            else
                enemy.SetState(new chaseState(enemy));
        }
    }

    public void onExit() { }
}