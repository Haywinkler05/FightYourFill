using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private Player player;
    private float health;
    private float lerpTimer;
    private float cachedFill;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;
    private PlayerUI playerUI;

    private float balance = 0.85f;

    [SerializeField]
    int currentExperience, maxExperience,
    currentLevel, currentSPoints;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        player = GetComponentInParent<Player>();
        playerUI = player.UI;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }
    


    //XP Related
    private void OnEnable()
    {
        ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
    }

    private void OnDisable()
    {
        ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;
    }

    private void HandleExperienceChange(int newExperience)
    {
        currentExperience += newExperience;
        if(currentExperience >= maxExperience)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentSPoints += 1;
        Debug.Log("Level Up! Current SPoints: " + currentSPoints + " Current Level: " + currentLevel);

        currentLevel++;

        currentExperience = 0;
        maxExperience += 100;

        //calls ScaleEnemy() on all enemies in the scene
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float growth = 0.15f; // 15% per level
        foreach (Enemy enemy in enemies)
        {
            float scaleValue = balance * (1 + growth * (currentLevel - 1));
            enemy.scaleEnemy(scaleValue);
        }
    }

    //Health Related
    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        //Debug.Log(fillBack);
        float healthFraction = (health / maxHealth);

        if (fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(cachedFill, healthFraction, percentComplete);
        }
        if (fillFront < healthFraction)
        {
            backHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, healthFraction, percentComplete);
        }
        string healthText = (health.ToString() + " " + maxHealth.ToString());
        playerUI.UpdateHealthText(health);
        //Debug.Log(playerUI.GetHealthText());
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        cachedFill = backHealthBar.fillAmount;
        Debug.Log(health);
        if (health <= 0f)
        {
            health = 0f;
            OnDeath();
        }
    }


    private void OnDeath()
    {
        Debug.Log("Player has died.");
    }

    public void RestoreHealth(float heal)
    {
        health += heal;
        lerpTimer = 0f;
        cachedFill = frontHealthBar.fillAmount;
        Debug.Log(health);
        //playerUI.UpdateHealthText(health);
    }

    public float CurrentHealth => health;
    public bool IsDead => health <= 0f;
}
