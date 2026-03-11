using UnityEngine;
using UnityEngine.UI;

public class grillMinigameNote : MonoBehaviour
{

    public float tempo = 120f;
    public bool started = false;
    private bool canPress = false;
    public KeyCode keyPress;


    void Start()
    {
        float scale = findScale();
        tempo = tempo * scale;
    }


    void Update()
    {

        if(grillMinigameManager.instance.startPlaying == true){
            //shift note left
            transform.position -= new Vector3(tempo*Time.deltaTime, 0f, 0f);
            //if key pressed while in hit area then call hitNote and make invisible
            if (Input.GetKeyDown(keyPress))
                {
                    if(canPress == true)
                    {
                        gameObject.SetActive(false);
                        grillMinigameManager.instance.hitNote();
                
                    }
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "NoteZone")
        {
            //allow note to be pressed while in hit area
            canPress = true;
        }
        
    }


    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "NoteZone")
        {
            //once note leaves hit area make unpressable.
            canPress = false;
            //delete after off screen/1sec
            Invoke("delNote",1f);
        }
        
    }


    void delNote()
    {
        Destroy(gameObject);
        //call miss note from manager/logs deletes
        grillMinigameManager.instance.missNote();
    }

    float findScale()
    {
        float scale = grillMinigameManager.instance.canvasRef.transform.localScale.x;
        return scale;
    }
}
