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

    // Tracks whether the health bar fill values are already settled so
    // UpdateHealthUI does not re-enter the lerp branches every frame.
    private bool healthBarDirty = false;

    [SerializeField]
    int currentExperience, maxExperience,
    currentLevel, currentSPoints;

    void Start()
    {
        health = maxHealth;
        player = GetComponentInParent<Player>();
        playerUI = player.UI;

        // Initialise bars to full immediately so no spurious lerp fires on frame 1.
        if (frontHealthBar != null) frontHealthBar.fillAmount = 1f;
        if (backHealthBar != null)  backHealthBar.fillAmount  = 1f;
        cachedFill = 1f;

        if (CurrentLevelTxt != null)
            CurrentLevelTxt.text = "" + currentLevel;

        UpdateStatsUI();
        // Push the correct text to the HUD on start.
        if (playerUI != null) playerUI.UpdateHealthText(health);
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        // Only run the lerp logic when something has actually changed.
        if (healthBarDirty)
            UpdateHealthUI();
    }

    // -----------------------------------------------------------------------
    // XP / Level
    // -----------------------------------------------------------------------
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
            LevelUp();
    }

    // -----------------------------------------------------------------------
    // Stats UI (StatsMenu panel)
    // -----------------------------------------------------------------------
    [SerializeField] private TextMeshProUGUI CurrentLevelTxt;
    [SerializeField] private TextMeshProUGUI MaxHealthTxt;
    [SerializeField] private TextMeshProUGUI DamageMultiplierTxt;
    [SerializeField] private TextMeshProUGUI SkillPointsTxt;

    public void UpdateStatsUI()
    {
        if (MaxHealthTxt != null)
            MaxHealthTxt.text = "Max Health: " + maxHealth.ToString();

        if (DamageMultiplierTxt != null)
            DamageMultiplierTxt.text = "Damage Multiplier: " + damageMultiplier.ToString("F2");

        if (SkillPointsTxt != null)
            SkillPointsTxt.text = "Skill Points: " + currentSPoints.ToString();
    }

    public void IncreaseMaxHealth()
    {
        if (currentSPoints <= 0)
        {
            //Debug.LogWarning("[IncreaseMaxHealth] Not enough skill points!");
            return;
        }

        maxHealth += 10f;
        // Heal the player fully to the new max health.
        health = maxHealth;
        currentSPoints--;

        // Reset the lerp so the health bar animates upward from its current
        // fill to full, showing the heal visually.
        lerpTimer  = 0f;
        cachedFill = frontHealthBar != null ? frontHealthBar.fillAmount : 0f;
        healthBarDirty = true;

        UpdateStatsUI();
        // Force an immediate text update so the HUD reflects the new values.
        if (playerUI != null) playerUI.UpdateHealthText(health);
    }

    public void IncreaseDamageMultiplier()
    {
        if (currentSPoints <= 0)
        {
            //Debug.LogWarning("[IncreaseDamageMultiplier] Not enough skill points!");
            return;
        }

        damageMultiplier += 0.1f;
        currentSPoints--;
        UpdateStatsUI();
    }

    // -----------------------------------------------------------------------
    // Level Up
    // -----------------------------------------------------------------------
    private void LevelUp()
    {
        currentSPoints += 1;
        currentLevel++;
        currentExperience = 0;
        maxExperience += 100;

        UpdateStatsUI();

        if (CurrentLevelTxt != null)
            CurrentLevelTxt.text = "" + currentLevel;

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float growth = 0.15f;
        foreach (Enemy enemy in enemies)
        {
            float scaleValue = balance * (1 + growth * (currentLevel - 1));
            enemy.scaleEnemy(scaleValue);
        }
    }

    // -----------------------------------------------------------------------
    // Health bar UI
    // -----------------------------------------------------------------------
    public void UpdateHealthUI()
    {
        if (frontHealthBar == null || backHealthBar == null) return;

        float healthFraction = health / maxHealth;
        float fillFront = frontHealthBar.fillAmount;
        float fillBack  = backHealthBar.fillAmount;

        // --- Damage: front bar snaps down, back bar lags behind in red ---
        if (fillBack > healthFraction + 0.001f)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(cachedFill, healthFraction, percentComplete);

            // Once the lerp finishes, mark clean.
            if (backHealthBar.fillAmount <= healthFraction + 0.001f)
            {
                backHealthBar.fillAmount = healthFraction;
                healthBarDirty = false;
            }
        }
        // --- Heal: back bar snaps up, front bar lags behind in green ---
        else if (fillFront < healthFraction - 0.001f)
        {
            backHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.green;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(cachedFill, healthFraction, percentComplete);

            if (frontHealthBar.fillAmount >= healthFraction - 0.001f)
            {
                frontHealthBar.fillAmount = healthFraction;
                healthBarDirty = false;
            }
        }
        else
        {
            // Both bars are settled — nothing to animate.
            healthBarDirty = false;
        }

        // Always keep the HUD text in sync.
        if (playerUI != null) playerUI.UpdateHealthText(health);
    }

    // -----------------------------------------------------------------------
    // Damage / Heal (called externally)
    // -----------------------------------------------------------------------
    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        health -= damage;
        health  = Mathf.Clamp(health, 0, maxHealth);

        // Reset lerp so the back-bar slides from the current fill, not a stale one.
        lerpTimer  = 0f;
        cachedFill = backHealthBar != null ? backHealthBar.fillAmount : (health / maxHealth);
        healthBarDirty = true;

        if (playerUI != null) playerUI.UpdateHealthText(health);

        if (health <= 0f)
            OnDeath();
    }

    public void RestoreHealth(float heal)
    {
        if (IsDead) return;

        health += heal;
        health  = Mathf.Clamp(health, 0, maxHealth);

        lerpTimer  = 0f;
        cachedFill = frontHealthBar != null ? frontHealthBar.fillAmount : (health / maxHealth);
        healthBarDirty = true;

        if (playerUI != null) playerUI.UpdateHealthText(health);
    }

    private void OnDeath()
    {
        Debug.Log("Player has died.");
    }

    public float CurrentHealth => health;
    public bool  IsDead        => health <= 0f;
}