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
    //amount of notes to spawn; def = 7
    public int amountNotes = 7;
    //all note prefabs
    public GameObject note1, note2, note3, note4; 
    // meat ref
    private GameObject meat;

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

    }

    // Update is called once per frame
    void Update()
    {
        if((startPlaying = true) && (finishPlay == true))
        {
            cookingCamera.refCam.cameraPriority(2);
            Destroy(meat);
            //turn back on recipe menu button
            recipeMenu.refR.toggleButtonListen();
            Cursor.visible = true;
            startPlaying = false;
            gameObject.SetActive(false);
            ingredientManager.refI.updateIngredientList();
            ingredientManager.refI.spawnIngredients();
        }
    }


    public void missNote()
    {
        if(notesActive.Count == 1)
        {
            notesActive.RemoveAt(0);
            finishPlay = true;
        }
        else notesActive.RemoveAt(0);
    }

    public void hitNote()
    {
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
        finishPlay = false;
        startPlaying = true;
        cookingCamera.refCam.cameraPriority(1);
        for(int i = 0; i < num; i++)
        {
            float delay = 1f*i + Random.Range(0,2)*0.5f;

            Invoke("randomNote", delay);
        }
    }

    public void playRecipe()
    {
        gameObject.SetActive(true);
        recipeMenu.refR.toggleButtonListen();
        Cursor.visible = false;
        meat = ingredientManager.refI.spawnGrillMeat();
        meat.GetComponent<Animation>().Play("Flip");
        recipeNotes(amountNotes);
    }

    public void setNoteAmount(int num)
    {
        amountNotes = num;
    }


    
}
