using UnityEngine;

public class StatsMenu : MonoBehaviour
{
    public GameObject container;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (container.activeSelf)
            {
                container.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                container.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
