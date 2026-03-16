using UnityEngine;

public class gameManager : MonoBehaviour
{
    //This means only one instance of this class will exist
    public static gameManager Instance;

    [Header("Level Settings")]
    [SerializeField] private int currentLevel;
    [SerializeField] private float levelDuration;


    [Header("Zombie Spawners")]
    [SerializeField] private spawnEnemy[] zombieSpawners;


    [Header("Ghoul Spawners")]
    [SerializeField] private spawnEnemy[] ghoulSpawners;

    [Header("Skeleton Spawners")]
    [SerializeField] private spawnEnemy[] skeletonSpawners;

    [Header("Plague Doctor Spawner")]
    [SerializeField] private spawnEnemy[] plaugeDoctorSpawners;

    [Header("Spider Spawners")]
    [SerializeField] private spawnEnemy[] spiderSpawners;

    [Header("Orc Spawners")]
    [SerializeField] private spawnEnemy[] orcSpawners;

    [Header("Golem Spawners")]
    [SerializeField] private spawnEnemy[] golemSpawners;
    [Header("Scripts")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private Player player;
    [SerializeField] private ExperienceManager experienceManager;


    [Header("Game state Conditions")]
    [SerializeField] private bool gameOver = false;
    [SerializeField] private bool gamePaused = false;
    [SerializeField] private bool gameStart = false;
    [SerializeField] private bool moveToNextLevel = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
       
    }

    
}
