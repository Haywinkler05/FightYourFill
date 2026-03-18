using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;   

public class PauseMenuCooking : MonoBehaviour
{

    public GameObject container;
    

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        container.SetActive(true);
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;       

        }

    }

    public void ResumeButton()
    {
        container.SetActive(false);
        Time.timeScale = 1;

        bool stateGrill = grillMinigameManager.instance.getState();
        int notesActive = grillMinigameManager.instance.getActiveNotes();
        Debug.Log((stateGrill) + " " + (notesActive));
        if((stateGrill) && (notesActive > 0))
        {
            Debug.Log("resume rhythm");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
        }
        else
        {
            Debug.Log("resume mouse");
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;   
        }

        

    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("GameSceneTitle");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

}
