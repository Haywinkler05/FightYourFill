using UnityEngine;
using UnityEngine.SceneManagement;   
using UnityEngine.InputSystem;

public class Exit : MonoBehaviour
{

    [SerializeField]
    private InputAction actionExit;
    [SerializeField]
    string sceneName = "";

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
            Debug.Log("Exit Scene not specified");
        }
        else
        {
            Debug.Log("Exit Clicked");
            SceneManager.LoadScene(sceneName);
        }
        
    }
}
