using UnityEngine;

public class musicManager : MonoBehaviour
{
    public static musicManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource ambientSource;
    [SerializeField] private AudioSource combatSource;


    [Header("Music Clips")]
    [SerializeField] private AudioClip ambientDaytimeMusic;
    [SerializeField] private AudioClip combatMusic;
    [SerializeField] private AudioClip searchMusic;
    [SerializeField] private AudioClip endofDayMusic;

    private int enemiesChasing = 0;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (combatSource != null && combatMusic != null)
        {
            combatSource.clip = combatMusic;
            combatSource.loop = true;
            combatSource.volume = 0f; // start silent
            combatSource.Play(); // playing but silent
        }

        if (ambientSource != null && ambientDaytimeMusic != null)
        {
            ambientSource.clip = ambientDaytimeMusic;
            ambientSource.loop = true;
            ambientSource.volume = 1f;
            ambientSource.Play();
        }
    }

    public void onEnemyChase()
    {
        enemiesChasing++;
        if(enemiesChasing == 1)
        {
            StartCoroutine(CrossFade(ambientSource, combatSource));
        }
    }

    public void onEnemyLost()
    {
        enemiesChasing = Mathf.Max(0, enemiesChasing - 1);
        if (enemiesChasing == 0)
            StartCoroutine(CrossFade(combatSource, ambientSource));
    }
    private System.Collections.IEnumerator CrossFade(AudioSource from, AudioSource to)
    {
        float duration = 1.5f;
        float timer = 0f;
        float fromStartVolume = from.volume;
        float toStartVolume = to.volume;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            from.volume = Mathf.Lerp(fromStartVolume, 0f, t);
            to.volume = Mathf.Lerp(toStartVolume, 1f, t);
            yield return null;
        }

        from.volume = 0f;
        to.volume = 1f;
    }
}
