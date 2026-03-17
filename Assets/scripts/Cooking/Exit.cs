using UnityEngine;
using UnityEngine.SceneManagement;   
using UnityEngine.InputSystem;
using System.Net;

public class Exit : MonoBehaviour
{

    [SerializeField]
    private InputAction actionExit;
    [SerializeField]
    string sceneName = "GameScene";

    private void Start() {
        actionExit.performed += _ => ExitButton();
    }

    public void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            SceneManager.LoadScene(sceneName);
        } */
    }

    private void OnEnable() {
        actionExit.Enable();
    }
    private void OnDisable() {
        actionExit.Disable();
    }

    public void ExitButton()
    {
        if(sceneName == "")
        {
            return;
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
        
    }
}
