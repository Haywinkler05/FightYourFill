using UnityEngine;

public class ogreRageState : IState
{
    private ogreEnemy enemy;
    private float timer;
    private float ring1Time = 0f;
    private float ring2Time = 0f;
    private float ring3Time = 0f;
    private bool ring1Fired;
    private bool ring2Fired;
    private bool ring3Fired;
    public ogreRageState(ogreEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void onEnter()
    {
        enemy.Agent.isStopped = true;
        enemy.Animator.CrossFadeInFixedTime(enemy.rageState.name, enemy.crossFadeAnimSpeed);
        enemy.Heal(enemy.rageHealthBonus);

        ring1Fired = false;
        ring2Fired = false;
        ring3Fired = false;

        // Space the three rings evenly across the "attack" animation duration
        // Tweak the multipliers to sync with the animation later
        float animLength = enemy.rageStateAttack.length;
        ring1Time = animLength * 0.25f;
        ring2Time = animLength * 0.50f;
        ring3Time = animLength * 0.75f;
        // Add offset so timers only activate during the "attack" animation
        ring1Time += enemy.rageState.length;
        ring2Time += enemy.rageState.length;
        ring3Time += enemy.rageState.length;

        timer = 0f;
    }

    public void onExit()
    {
        enemy.Agent.isStopped = false;
    }

    public void update()
    {
        timer += Time.deltaTime;

        if (!ring1Fired && timer >= ring1Time)
        {
            ring1Fired = true;
            FireRingAttack(1, enemy.rageRing1Radius);
        }
        if (!ring2Fired && timer >= ring2Time)
        {
            ring2Fired = true;
            FireRingAttack(2, enemy.rageRing2Radius);
        }
        if (!ring3Fired && timer >= ring3Time)
        {
            ring3Fired = true;
            FireRingAttack(3, enemy.rageRing3Radius);
        }

        if (timer >= (enemy.rageState.length + enemy.rageStateAttack.length))
        {
            enemy.SetState(new chaseState(enemy));

        }
    }
    private void FireRingAttack(int ringIndex, float radius)
    {
        GameObject prefab = ringIndex switch
        {
            1 => enemy.ring1Prefab,
            2 => enemy.ring2Prefab,
            3 => enemy.ring3Prefab,
            _ => null
        };

        if (prefab == null) return;

        GameObject ring = Object.Instantiate(prefab, enemy.transform.position, Quaternion.identity);
        RingAttack attack = ring.GetComponent<RingAttack>();
        if (attack != null)
            attack.Activate(); // this activates both the warning and the stabbing animation
    }
}
