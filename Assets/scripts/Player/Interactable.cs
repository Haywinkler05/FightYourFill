using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;   


public class Cart : MonoBehaviour
{

    public Transform interactPoint;
    public float interactRange = 2.5f;

    public InputActionReference interactCart;
    public LayerMask interactableLayers;

    // Update is called once per frame
    void Update()
    {

        
        Collider[] hitsCart = Physics.OverlapSphere(interactPoint.position, interactRange, interactableLayers);
        foreach (Collider interactable in hitsCart)
        {
            
            Debug.Log("Within!!!!");
            HandleInteract();
        }
    }

    void HandleInteract()
    {

        if (interactCart != null && interactCart.action != null && interactCart.action.triggered)//Best way to do this maybe????
        {
            //Loads player into cooking scene
            SceneManager.LoadScene("CookingDemo");
            Debug.Log("Cart interacted with");//LOG
        }

    }

void OnDrawGizmosSelected()
{
    if (interactPoint == null)
        return;

    Gizmos.color = Color.red;

    Gizmos.matrix = interactPoint.localToWorldMatrix;
    Gizmos.DrawWireSphere(Vector3.zero, interactRange);
}


}
