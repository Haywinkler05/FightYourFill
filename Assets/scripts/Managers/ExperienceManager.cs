using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    public delegate void ExperienceChangeHandler(int amount);
    public event ExperienceChangeHandler OnExperienceChange;

    //Singleton Check
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy the whole GameObject, not just the component
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddExperience(int amount)
    {
        OnExperienceChange?.Invoke(amount);
    }


}
