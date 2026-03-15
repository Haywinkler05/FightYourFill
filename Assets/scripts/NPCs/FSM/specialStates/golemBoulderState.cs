using UnityEngine;
public class golemBoulderState : IState
{
    private golemEnemy enemy;
    private float timer;

    private float[] ringTimes;
    private bool[] ringFired;

    public golemBoulderState(golemEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void onEnter()
    {
        enemy.Agent.isStopped = true;
        enemy.SetInvulnerable(true);
        enemy.Animator.CrossFadeInFixedTime(enemy.rageState.name, enemy.crossFadeAnimSpeed);
        enemy.Heal(enemy.rageHealthBonus);
        timer = 0f;

        int ringCount = enemy.ringPrefabs.Length;
        ringTimes = new float[ringCount];
        ringFired = new bool[ringCount];

        float animLength = enemy.rageStateAttack.length;
        float offset = enemy.rageState.length;

        for (int i = 0; i < ringCount; i++)
        {
            ringTimes[i] = offset + animLength * ((i + 1f) / (ringCount + 2f));
            ringFired[i] = false;
        }
    }

    public void onExit()
    {
        enemy.Agent.isStopped = false;
        enemy.SetInvulnerable(false);
    }

    public void update()
    {
        timer += Time.deltaTime;

        for (int i = 0; i < ringTimes.Length; i++)
        {
            if (!ringFired[i] && timer >= ringTimes[i])
            {
                ringFired[i] = true;
                FireRingAttack(i);
            }
        }

        if (timer >= (enemy.rageState.length + enemy.rageStateAttack.length))
            enemy.SetState(new chaseState(enemy));
    }

    private void FireRingAttack(int index)
    {
        if (index >= enemy.ringPrefabs.Length) return;

        GameObject prefab = enemy.ringPrefabs[index];
        if (prefab == null) return;

        RingAttack prefabAttack = prefab.GetComponent<RingAttack>();
        if (prefabAttack == null) return;

        GameObject ring = Object.Instantiate(prefab, enemy.transform.position, Quaternion.identity);
        RingAttack attack = ring.GetComponent<RingAttack>();
        if (attack != null)
        {
            attack.damage = enemy.ogreRingDamage;
            attack.Activate();
        }
    }
}