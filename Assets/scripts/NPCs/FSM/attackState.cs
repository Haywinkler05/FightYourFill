using UnityEngine;
public class attackState : IState
{
    private Enemy enemy;
    private float attackDuration;
    private float timer;
    private int randomIndex;
    private AnimationClip[] attacks;
    private AnimationClip chosenClip;
    private string chosenStateName; // Add this
    private bool hasDealtDamage = false;

    public attackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void onEnter()
    {
        attacks = new AnimationClip[] { enemy.Attack1Clip, enemy.Attack2Clip, enemy.Attack3Clip };
        randomIndex = Random.Range(0, attacks.Length);
        chosenClip = attacks[randomIndex];

        
        chosenStateName = chosenClip.name;

        enemy.Animator.Play(chosenStateName);
        attackDuration = chosenClip.length;
        timer = 0f;
        hasDealtDamage = false;

        if (enemy.player == null)
        {
            enemy.player = GameObject.FindWithTag("Player");
            if (enemy.player == null)
                Debug.LogError("[attackState] Could not find player with tag 'Player'.");
            else
                Debug.Log("[attackState] Found player by tag in onEnter.");
        }
    }

    public void update()
    {
        timer += Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        Vector3 directionToPlayer = (enemy.player.transform.position - enemy.transform.position).normalized;
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            enemy.transform.rotation = Quaternion.RotateTowards(
                enemy.transform.rotation,
                targetRotation,
                enemy.roatationSpeed * Time.deltaTime
            );
        }

        if (distanceToPlayer > enemy.Agent.stoppingDistance + 2f)
        {
            enemy.SetState(new chaseState(enemy));
            return;
        }

       
        if (!hasDealtDamage && timer >= attackDuration * 0.5f)
        {
            DealDamageToPlayer();
        }

        if (timer >= attackDuration)
        {
            if (distanceToPlayer <= enemy.Agent.stoppingDistance + 0.5f)
                enemy.SetState(new attackState(enemy));
            else
                enemy.SetState(new chaseState(enemy));
        }
    }

    private void DealDamageToPlayer()
    {
        hasDealtDamage = true;
        enemy.PlaySFX(enemy.attackSFX);

        if (enemy.player == null)
        {
            enemy.player = GameObject.FindWithTag("Player");
            if (enemy.player == null) return;
        }

        var playerHealth = enemy.player.GetComponent<PlayerStats>();
        if (playerHealth == null)
        {
            return;
        }

        playerHealth.TakeDamage(enemy.Damage);
    }

    public void onExit() { }
}