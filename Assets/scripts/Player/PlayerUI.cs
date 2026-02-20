using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
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

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
