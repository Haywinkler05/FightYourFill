using UnityEngine;
using UnityEngine.SceneManagement;   

public class Exit : MonoBehaviour
{

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            SceneManager.LoadScene("Combat Scene");
        }
    }

    public void ExitButton()
    {
        Debug.Log("Exit Clicked");
        SceneManager.LoadScene("Combat Scene");
    }
}
