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
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        combatSource.clip = combatMusic;
        DontDestroyOnLoad(gameObject);
        if (ambientSource != null && ambientDaytimeMusic != null)
        {
            ambientSource.clip = ambientDaytimeMusic;
            ambientSource.loop = true;
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
        to.Play();

        while (timer < duration)
        {
            timer += Time.deltaTime;
            from.volume = Mathf.Lerp(1f, 0f, timer / duration);
            to.volume = Mathf.Lerp(0f, 1f, timer / duration);
            yield return null;
        }

        from.Stop();
        from.volume = 1f;
    }
}
