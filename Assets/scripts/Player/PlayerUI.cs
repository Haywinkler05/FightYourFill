using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
    [SerializeField]
    private TextMeshProUGUI healthText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //public void UpdateHealthText(string text)
    //{
    //    healthText.text = text;
    //}
    public string GetHealthText()
    {
        return healthText.text;
    }
    public void UpdateHealthText(float health)
    {
        // Catch edge case where low health would register as 0 despite not being dead
        if (health <= 0.5 && health > 0)
        {
            health = 1;
        }
        // Round health to nearest int (keeping as float) to get rid of nasty long float values
        float healthVisual = Mathf.Round(health);
        healthText.text = healthVisual.ToString();
    }
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
    

}
