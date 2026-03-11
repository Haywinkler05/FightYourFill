using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class grillMinigameManager : MonoBehaviour
{

    public bool startPlaying = false;
    public bool finishPlay = false;
    //just reference this from a music object
    public AudioSource music;
    //hit sound
    public AudioSource hitSound;
    
    public int score;
    //amount of notes to spawn
    public int amountNotes = 7;
    //all note prefabs
    public GameObject note1, note2, note3, note4; 

    private List<GameObject> notesActive = new List<GameObject>();
    //reference to this manager for functions and such
    public static grillMinigameManager instance;

    [SerializeField]
    public Canvas canvasRef;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //reference this manager
        instance = this;
        if(hitSound == null) hitSound = GetComponent<AudioSource>();
        

        float firstTime = Time.time;
        //starts playing when UI button next is pressed

    }

    // Update is called once per frame
    void Update()
    {
        if((startPlaying = true) && (finishPlay == true))
        {
            Debug.Log("Finished Playing");
            //swap cam to assembly or other
            cookingCamera.refCam.cameraPriority(0);
            startPlaying = false;
            gameObject.SetActive(false);
        }
    }




    public void missNote()
    {
        if(notesActive.Count == 1)
        {
            finishPlay = true;
        }
        else notesActive.RemoveAt(0);
        
        Debug.Log("noteDel");
        Debug.Log(notesActive.Count + "active notes");
    }

    public void hitNote()
    {
        Debug.Log("noteHit");
        //increase score by --- amount
        score += 100;
        //play hit sound
        hitSound.Play();

    }

    private void spawnNote(GameObject notePrefab)
    {
        //mult by scale to spawn in canvas at location
        GameObject note = Instantiate(notePrefab, 
        new Vector2(2000*canvasRef.transform.localScale.x, 190*canvasRef.transform.localScale.y), Quaternion.identity, canvasRef.transform);
        notesActive.Add(note);
        Debug.Log("Spawn note");

    }

    private void randomNote()
    {
        int randomNum = Random.Range(0,4);
        //Debug.Log(randomNum);

        switch (randomNum)
        {
            case 0:
                spawnNote(note1);
                break;
            case 1:
                spawnNote(note2);
                break;
            case 2:
                spawnNote(note3);
                break;
            case 3:
                spawnNote(note4);
                break;
            default:
                spawnNote(note1);
                break;

        }
    }


    public void recipeNotes(int num)
    {
        Debug.Log("Start recipe notes");
        startPlaying = true;
        cookingCamera.refCam.cameraPriority(1);
        for(int i = 0; i < num; i++)
        {
            float delay = 1f*i + Random.Range(0,2)*0.5f;

            Invoke("randomNote", delay);
            //Debug.Log("spawn notes recipe");
        }
    }

    public void playAnim()
    {
        
    }

    
}
