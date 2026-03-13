using UnityEngine;
public class attackState : IState
{
    private Enemy enemy;
    private float attackDuration;
    private float timer;
    private int randomIndex;
    private AnimationClip[] attacks;
    private AnimationClip chosenClip;
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

        enemy.Animator.CrossFadeInFixedTime(chosenClip.name, enemy.crossFadeAnimSpeed);
        attackDuration = chosenClip.length;
        timer = 0f; 
        hasDealtDamage = false;
        if (enemy.player == null)
        {
            Debug.LogWarning("[attackState] enemy.player is null in onEnter. Attempting to find player by tag.");
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
        directionToPlayer.y = 0; // keep rotation on Y axis only
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            enemy.transform.rotation = Quaternion.RotateTowards(
                enemy.transform.rotation,
                targetRotation,
                enemy.roatationSpeed * Time.deltaTime // rotation speed
            );
        }
        if (distanceToPlayer > enemy.Agent.stoppingDistance + 2f)
        {
            enemy.SetState(new chaseState(enemy));
            return;
        }
        //Timing to deal damage at halfway point of the attack animation
        if (!hasDealtDamage && timer >= attackDuration * 0.5f)
        {
            DealDamageToPlayer();
            hasDealtDamage = true;
        }
        if (timer >= attackDuration)
        {
            Debug.Log($"Attack finished. Distance: {distanceToPlayer}, StoppingDistance: {enemy.Agent.stoppingDistance + 0.5f}");
            if (distanceToPlayer <= enemy.Agent.stoppingDistance + 0.5f)
                enemy.SetState(new attackState(enemy));
            else
                enemy.SetState(new chaseState(enemy));
        }
    }

    //function to deal damage to the player
    private void DealDamageToPlayer()
    {
        if (enemy.player == null)
        {
            Debug.LogWarning("[DealDamageToPlayer] enemy.player is null. Attempting to find player by tag.");
            enemy.player = GameObject.FindWithTag("Player");
        }
        if (enemy.player != null)
        {
            var playerHealth = enemy.player.GetComponent<PlayerStats>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemy.Damage);
                Debug.Log("[DealDamageToPlayer] Player took damage: " + enemy.Damage);
            }
            else
            {
                Debug.LogError("[DealDamageToPlayer] PlayerHealth component not found on player object.");
            }
        }
        else
        {
            Debug.LogError("[DealDamageToPlayer] Player object is still null.");
        }
    }

    public void onExit() { }
}