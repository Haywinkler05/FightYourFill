using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    //This means only one instance of this class will exist
    public static gameManager Instance;


    
    [Header("Level Settings")]
    [SerializeField] private int currentLevel;
    [SerializeField] private float levelDuration;

    [Header("Day Tracker")]
    public float dayMinutes = 8f; // How many minutes should a day be
    private float dayDuration = 60f; // Set to 60 by default to multiply by Minutes easily
    private int dayCount = 0;
    private float timeRemaining = 0f;
    private bool timerActive = false;
    [SerializeField] private string cookingScene;

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

    [Header("Player")]
    public GameObject playerGameObject;

    [Header("Scripts")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private Player player;
    [SerializeField] private ExperienceManager experienceManager;


    [Header("Game state Conditions")]
    [SerializeField] private bool gameOver = false;
    [SerializeField] private bool gamePaused = false;
    [SerializeField] private bool gameStart = false;
    [SerializeField] private bool moveToNextLevel = false;

    [Header("Enemy Modifiers")]
    // A % of change that enemies receive to various stats per consecutive day survived
    [SerializeField] private float enemyScaleModifier = 1.1f; // should be >1 so that enemies scale and don't get weaker
    private float enemyScaleTotal = 1f;


    public GameObject PlayerObject => playerGameObject;
    public Player Player => player;
    public int CurrentLevel => currentLevel;

    // Sets timer values and presets timer format (minutes:seconds)
    public int MinutesRemaining => Mathf.FloorToInt(timeRemaining / 60f);
    public int SecondsRemaining => Mathf.FloorToInt(timeRemaining % 60f);
    public int DayCount => dayCount;
    public string FormattedTime => string.Format("{0:0}:{1:00}", MinutesRemaining, SecondsRemaining);

    void Start()
    {
       
        if (playerGameObject == null)
            playerGameObject = GameObject.FindWithTag("Player");
        if (player == null)
            player = playerGameObject?.GetComponent<Player>();
        if (experienceManager == null)
            experienceManager = FindFirstObjectByType<ExperienceManager>();

        StartDay();
    }

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

    void Update()
    {
        if (!timerActive || gamePaused)
        {
            return;
        }

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            timerActive = false;
            EndDay();
        }
    }

    public void ResetAllSpawners()
    {
        spawnEnemy[] allSpawners = FindObjectsByType<spawnEnemy>(FindObjectsSortMode.None);
        foreach (spawnEnemy spawner in allSpawners)
            spawner.ResetSpawner();
    }

    public void StartDay()
    {
        // Scale Enemeis by ScaleTotal
        Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in allEnemies)
        {
            enemy.scaleEnemy(enemyScaleTotal);
        }
        // Each day, immediately set ScaleTotal for the following day
        enemyScaleTotal *= enemyScaleModifier;

        dayCount++;
        dayDuration = dayMinutes * 60f;
        timeRemaining = dayDuration;
        timerActive = true;
    }

    private void EndDay()
    {
        Inventory inventory = playerGameObject.GetComponentInChildren<Inventory>();

        // Remove items from scene that aren't picked up by player
        Item[] groundItems = FindObjectsByType<Item>(FindObjectsSortMode.None);
        foreach (Item item in groundItems)
        {
            // Catch and skip currently held hand item if there is one
            if (inventory != null && item.gameObject == inventory.GetHandItemInstance())
                continue;

            Destroy(item.gameObject);
        }

        ResetAllSpawners();

        // Disable movement for player
        PlayerMotor motor = playerGameObject.GetComponentInChildren<PlayerMotor>();
        if (motor != null)
        {
            motor.enabled = false;
        }

        SceneManager.LoadScene(cookingScene);
    }

}
