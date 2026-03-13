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
    public float damageMultiplier = 1.0f;

    [SerializeField]
    int currentExperience, maxExperience,
    currentLevel, currentSPoints;

    void Start()
    {
        health = maxHealth;
        player = GetComponentInParent<Player>();
        playerUI = player.UI;

        if (CurrentLevelTxt != null)
        {
            CurrentLevelTxt.text = "" + currentLevel;
        }

        UpdateStatsUI();
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

    // XP Related
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
        if (currentExperience >= maxExperience)
        {
            LevelUp();
        }
    }

    [SerializeField]
    private TextMeshProUGUI CurrentLevelTxt;
    [SerializeField] private TextMeshProUGUI MaxHealthTxt;
    [SerializeField] private TextMeshProUGUI DamageMultiplierTxt;
    [SerializeField] private TextMeshProUGUI SkillPointsTxt;

    public void UpdateStatsUI()
    {
        Debug.Log($"[UpdateStatsUI] Instance ID: {gameObject.GetInstanceID()} | maxHealth: {maxHealth} | damageMultiplier: {damageMultiplier} | currentSPoints: {currentSPoints}");
        Debug.Log($"[UpdateStatsUI] MaxHealthTxt null? {MaxHealthTxt == null} | DamageMultiplierTxt null? {DamageMultiplierTxt == null} | SkillPointsTxt null? {SkillPointsTxt == null}");

        if (MaxHealthTxt != null)
        {
            MaxHealthTxt.text = "Max Health: " + maxHealth.ToString();
            Debug.Log($"[UpdateStatsUI] Set MaxHealthTxt to: {MaxHealthTxt.text} on object: {MaxHealthTxt.gameObject.name}");
        }
        if (DamageMultiplierTxt != null)
        {
            DamageMultiplierTxt.text = "Damage Multiplier: " + damageMultiplier.ToString("F2");
            Debug.Log($"[UpdateStatsUI] Set DamageMultiplierTxt to: {DamageMultiplierTxt.text} on object: {DamageMultiplierTxt.gameObject.name}");
        }
        if (SkillPointsTxt != null)
        {
            SkillPointsTxt.text = "Skill Points: " + currentSPoints.ToString();
            Debug.Log($"[UpdateStatsUI] Set SkillPointsTxt to: {SkillPointsTxt.text} on object: {SkillPointsTxt.gameObject.name}");
        }
    }

    public void IncreaseMaxHealth()
    {
        Debug.Log($"[IncreaseMaxHealth] Instance ID: {gameObject.GetInstanceID()} | currentSPoints: {currentSPoints} | maxHealth before: {maxHealth}");
        if (currentSPoints > 0)
        {
            maxHealth += 10f;
            health = Mathf.Clamp(health, 0, maxHealth);
            currentSPoints--;
            Debug.Log($"[IncreaseMaxHealth] maxHealth after: {maxHealth} | currentSPoints after: {currentSPoints}");
            UpdateStatsUI();
            UpdateHealthUI();
        }
        else
        {
            Debug.LogWarning("[IncreaseMaxHealth] Not enough skill points!");
        }
    }

    public void IncreaseDamageMultiplier()
    {
        Debug.Log($"[IncreaseDamageMultiplier] Instance ID: {gameObject.GetInstanceID()} | currentSPoints: {currentSPoints} | damageMultiplier before: {damageMultiplier}");
        if (currentSPoints > 0)
        {
            damageMultiplier += 0.1f;
            currentSPoints--;
            Debug.Log($"[IncreaseDamageMultiplier] damageMultiplier after: {damageMultiplier} | currentSPoints after: {currentSPoints}");
            UpdateStatsUI();
        }
        else
        {
            Debug.LogWarning("[IncreaseDamageMultiplier] Not enough skill points!");
        }
    }

    private void LevelUp()
    {
        currentSPoints += 1;
        Debug.Log("Level Up! Current SPoints: " + currentSPoints + " Current Level: " + currentLevel);
        currentLevel++;
        currentExperience = 0;
        maxExperience += 100;
        UpdateStatsUI();

        if (CurrentLevelTxt != null)
        {
            CurrentLevelTxt.text = "" + currentLevel;
        }

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float growth = 0.15f;
        foreach (Enemy enemy in enemies)
        {
            float scaleValue = balance * (1 + growth * (currentLevel - 1));
            enemy.scaleEnemy(scaleValue);
        }

        if (CurrentLevelTxt != null)
        {
            CurrentLevelTxt.text = "" + currentLevel;
        }
    }

    // Health Related
    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
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
    }

    public float CurrentHealth => health;
    public bool IsDead => health <= 0f;
}