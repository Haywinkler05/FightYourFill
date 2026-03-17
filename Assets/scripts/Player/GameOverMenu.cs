using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject container;

    public void OpenMenu()
    {
        container.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartButton()
    {
        container.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reset player stats before reloading
        if (gameManager.Instance != null)
        {
            Player player = gameManager.Instance.Player;
            if (player != null)
            {
                PlayerStats stats = player.GetComponentInChildren<PlayerStats>();
                if (stats != null)
                    stats.ResetStats();
            }

            gameManager.Instance.ResetGame();
        }

        SceneManager.LoadScene("GameScene");
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("VillageDemoTitleTest1");
    }
}