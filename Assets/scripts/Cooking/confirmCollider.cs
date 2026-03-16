using UnityEngine;

public class confirmCollider : MonoBehaviour
{

    public GameObject confirmButton;

    private void OnTriggerEnter(Collider other)
    {
    if (other.gameObject.CompareTag("Food"))
        {
            confirmButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Food"))
        {
            confirmButton.SetActive(false);
        }
    }
}
