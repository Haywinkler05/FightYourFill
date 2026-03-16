using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;   

public class PauseMenu : MonoBehaviour
{

    public GameObject container;
    public GameObject Inventory;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        container.SetActive(true);
        Time.timeScale = 0;

        Inventory.SetActive(false);
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;       

        }

    }

    public void ResumeButton()
    {
        container.SetActive(false);
        Time.timeScale = 1;

        Inventory.SetActive(true);
        Time.timeScale = 1;

        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("VillageDemoTitleTest1");
    }

    public void QuitButton()
    {
        
    }

}
