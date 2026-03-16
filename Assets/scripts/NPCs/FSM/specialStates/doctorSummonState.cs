using UnityEngine;
public class doctorSummonState : IState
{
    private doctorEnemy enemy;
    private float timer;
    private float animLength = 0f;
    private float animOffset = 0f;
    private bool hasSpawned = false;
    private const float spawnTime = 53 / 30f;

    public doctorSummonState(doctorEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void onEnter()
    {
        enemy.Agent.isStopped = true;
        enemy.Animator.CrossFadeInFixedTime(enemy.summonStateHit.name, enemy.crossFadeAnimSpeed);
        timer = 0f;
        hasSpawned = false;

        animLength = enemy.summonStateCast.length;
        animOffset = enemy.summonStateHit.length;
    }

    public void onExit()
    {
        enemy.Agent.isStopped = false;
        enemy.SetInvulnerable(false);
    }

    public void update()
    {
        timer += Time.deltaTime;

        if (!hasSpawned && timer >= animOffset + spawnTime)
        {
            hasSpawned = true;
            SummonAll();
        }

        if (timer >= (enemy.summonStateHit.length + enemy.summonStateCast.length))
        {
            enemy.SetState(new idleState(enemy));
        }
    }

    private void SummonAll() // SummonAll is used to iterate through prefab list, so prefabs can be dynamically added and removed in editor
    {
        int totalCount = enemy.summonPrefabs.Length;

        for (int i = 0; i < totalCount; i++)
        {
            if (enemy.summonPrefabs[i] == null)
            {
                continue;
            }

            // Summons are spawned in an evenly-spread ring around the Doctor
            float angle = i * (360f / totalCount) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * enemy.summonRadius,
                0f,
                Mathf.Sin(angle) * enemy.summonRadius
            );

            Summon(enemy.summonPrefabs[i], enemy.transform.position + offset);
        }
    }

    private void Summon(GameObject prefab, Vector3 position)
    {
        GameObject minion = Object.Instantiate(prefab, position, Quaternion.identity);

        // Set summons' facing direction to be outward from the Plague Doctor
        Vector3 facingAngle = (position - enemy.transform.position).normalized;
        if (facingAngle != Vector3.zero)
        {
            minion.transform.rotation = Quaternion.LookRotation(facingAngle);
        }
    }
}