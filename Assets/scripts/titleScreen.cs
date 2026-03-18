using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class titleScreen : MonoBehaviour
{
    [SerializeField]
    private InputAction actionStart;
    [SerializeField]
    private InputAction actionExit;
    [SerializeField]
    public AudioSource hitSound;
    [SerializeField]
    string sceneName;

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;

        actionStart.performed += _ => startGame();
        actionExit.canceled += _ => exitGame();
    }
    private void OnEnable() {
        actionStart.Enable();
        actionExit.Enable();
    }
    private void OnDisable() {
        actionStart.Disable();
        actionExit.Disable();
    }

    public void startGame()
    {
        hitSound.Play();
        SceneManager.LoadScene(sceneName);
    }

    public void exitGame()
    {
        Application.Quit();
    }


}
