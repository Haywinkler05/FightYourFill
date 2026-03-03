using UnityEngine;
using UnityEngine.UI;

public class grillMinigameNote : MonoBehaviour
{

    public float tempo = 120f;
    public bool started = false;
    private bool canPress = false;
    public KeyCode keyPress;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //tempo = tempo/60f;
    }

    // Update is called once per frame
    void Update()
    {
        //shift note left
        transform.position -= new Vector3(tempo*Time.deltaTime, 0f, 0f);
        if (Input.GetKeyDown(keyPress))
        {
            if(canPress == true)
            {
                //change to del later
                Destroy(gameObject);
                

                //gameObject.SetActive(false);

                grillMinigameManager.instance.hitNote();
                
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "NoteZone")
        {
            canPress = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "NoteZone")
        {
            canPress = false;
            grillMinigameManager.instance.missNote();
            //play miss sfx
            //play miss sprite
            //delete after off screen/1sec
        }
        
    }
}
