using UnityEngine;

public class Pause : MonoBehaviour
{

    public GameObject container;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            container.SetActive(true);
            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }        
    }

    public void ResumeButton()
    {
        container.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenuButton()
    {
        //Will be used upon creation of a main menu scene -phil
        //UnityEngine.Scene.Management.SceneManager.LoadScene("")'
    }
}
