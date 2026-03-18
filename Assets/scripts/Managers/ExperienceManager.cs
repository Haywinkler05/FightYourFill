using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    public delegate void ExperienceChangeHandler(int amount);
    public event ExperienceChangeHandler OnExperienceChange;

    // Store XP and skill points here so they persist across scenes
    private int totalExperience = 0;
    private int skillPoints = 0;
    private int healthUpPoints = 0;
    private int damageUpPoints = 0;

    public int TotalExperience => totalExperience;
    public int SkillPoints => skillPoints;
    public int HealthUpPoints => healthUpPoints;
    public int DamageUpPoints => damageUpPoints;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddExperience(int amount)
    {
        totalExperience += amount;
        OnExperienceChange?.Invoke(amount);
    }

    public void AddSkillPoints(int amount)
    {
        skillPoints += amount;
    }

    public void SpendSkillPoint()
    {
        if (skillPoints > 0)
            skillPoints--;
    }

    public void ResetExperience()
    {
        totalExperience = 0;
        skillPoints = 0;
    }
}