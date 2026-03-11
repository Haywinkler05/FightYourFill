using UnityEngine;
using UnityEngine.AI;

public class spawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int spawnCount = 3;
    [SerializeField] float spawnRadius = 5f;
    [SerializeField] bool spawnOnStart = true;
    void Start()
    {
        if (spawnOnStart)
        {
            spawnEnemies();
        }
    }

    public void spawnEnemies()
    {
        for (int i = 0; i < spawnCount; i++)
        {

            Vector2 random2D = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(random2D.x, 0, random2D.y);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPos, out hit, 2f, NavMesh.AllAreas))
                Instantiate(enemyPrefab, hit.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize spawn radius in scene view
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
