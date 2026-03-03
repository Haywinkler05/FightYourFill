using UnityEngine;
using UnityEngine.UI;

public class grillMinigameManager : MonoBehaviour
{

    public bool startPlaying;
    public AudioSource music;
    public int score;

    public static grillMinigameManager instance;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //reference this manager
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;

            }
        }
    }

    public void missNote()
    {
        Debug.Log("noteMiss");
        
    }

    public void hitNote()
    {
        Debug.Log("noteHit");
        score += 100;
    }
}
