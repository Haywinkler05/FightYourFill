using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;
public class golemSlamState : IState
{
    private golemEnemy enemy;
    private float timer;

    private float animLength = 0f;
    private float animOffset = 0f;

    private float slamTiming = 12 / 30f; // 12th frame activation
    private bool hasSlammed = false;

    public golemSlamState(golemEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void onEnter()
    {
        enemy.Agent.isStopped = true;
        enemy.SetInvulnerable(true);
        enemy.Animator.CrossFadeInFixedTime(enemy.slamState.name, enemy.crossFadeAnimSpeed);
        timer = 0f;

        animLength = enemy.slamStateAttack.length;
        animOffset = enemy.slamState.length;

        hasSlammed = false;
    }

    public void onExit()
    {
        enemy.Agent.isStopped = false;
        enemy.SetInvulnerable(false);
    }

    public void update()
    {
        timer += Time.deltaTime;

        if (!hasSlammed && timer >= animOffset + slamTiming)
        {
            hasSlammed = true;
            Slam();
        }

        if (timer >= (enemy.slamState.length + enemy.slamStateAttack.length))
            enemy.SetState(new chaseState(enemy));
    }

    private void Slam()
    {
        Collider[] hits = Physics.OverlapSphere(
            enemy.transform.position,
            enemy.slamRadius,
            enemy.playerLayer
        );

        // Tracks if a player has already been hit to prevent hitting the same player more than once
        System.Collections.Generic.HashSet<Player> alreadyHit = new System.Collections.Generic.HashSet<Player>();

        foreach (Collider hit in hits)
        {
            Player playerRoot = hit.GetComponentInChildren<Player>();

            // Debug.Log($"[GolemSlam] Hit: {hit.name}");

            PlayerMotor motor = hit.GetComponentInChildren<PlayerMotor>();
            if (motor == null)
            {
                // Debug.Log($"[GolemSlam] No PlayerMotor found on {hit.name}");
                continue;
            }

            if (alreadyHit.Contains(playerRoot))
            {
                Debug.Log("Player Already Hit");
                continue;
            }
            alreadyHit.Add(playerRoot);

            Debug.Log("Player Getting Hit");

            // Get direction of knockback, set y to 0 for customizable vertical knockback
            Vector3 flatDirection = hit.transform.position - enemy.transform.position;
            flatDirection.y = 0;

            // Get distance between player and golem for knockback and damage falloff
            float distance = flatDirection.magnitude;
            flatDirection.Normalize();

            // Falloff determines the final fraction of knockback and damage to apply, squared for sharper change
            float falloff = 1f - Mathf.Clamp01(distance / enemy.slamRadius);
            falloff = Mathf.Sqrt(falloff);

            // Set actual knockback vector for use with ApplyKnockback, multiply by SlamForce and falloff
            Vector3 knockbackDir = (flatDirection + Vector3.up * enemy.slamUpwardBias).normalized;
            motor.ApplyKnockback(knockbackDir * enemy.golemSlamForce * falloff);

            // Damage player
            PlayerStats stats = hit.GetComponentInChildren<PlayerStats>();
            if (stats != null)
            {
                float damageActual = enemy.golemSlamDamage * falloff;
                stats.TakeDamage(Mathf.Ceil(damageActual));
            }
        }
    }
}