using UnityEngine;
using UnityEngine.AI;

public class spawnEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    public int spawnCount = 3;
    public float spawnRadius = 5f;
    public bool spawnOnStart = false;
    public float triggerRadius = 15f;
    [SerializeField] private bool hasSpawned = false;

    private Transform player;
    void Start()
    {
        if (gameManager.Instance != null)
            player = gameManager.Instance.Player.transform;
        else
            player = GameObject.FindWithTag("Player")?.transform;

        if (spawnOnStart)
            spawnEnemies();
    }

    public void spawnEnemies()
    {
        hasSpawned = true;
        this.enabled = false;
        for (int i = 0; i < spawnCount; i++)
        {

            Vector2 random2D = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(random2D.x, 0, random2D.y);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPos, out hit, 2f, NavMesh.AllAreas))
                Instantiate(enemyPrefab, hit.position, Quaternion.identity);
        }
    }

    private float checkTimer;
    [SerializeField] private float checkInterval = 0.2f; // check 5 times per second

    void Update()
    {
        if (hasSpawned) return; // early exit if already spawned

        checkTimer += Time.deltaTime;
        if (checkTimer < checkInterval) return; // only check every 0.2 seconds
        checkTimer = 0f;

        if (player != null)
        {
            Vector3 spawnerFlat = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 playerFlat = new Vector3(player.position.x, 0, player.position.z);
            float distance = Vector3.Distance(spawnerFlat, playerFlat);
            if (distance <= triggerRadius)
                spawnEnemies();
        }
    }
    public void ResetSpawner()
    {
        hasSpawned = false;
        this.enabled = true;

    }
    
    private void OnDrawGizmos()
    {
      
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }

    
}
