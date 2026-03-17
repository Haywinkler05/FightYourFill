using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;   

//This script will probably be for the cart only but its set up to where if we
//want more interactable objects that will transfer a player to a new scene then we can
public class Cart : MonoBehaviour
{

    public GameObject InteractButton;

    public Transform interactPoint;
    public float interactRange = 2.5f;

    public InputActionReference interactCart;
    public LayerMask cartLayers;

    // Update is called once per frame
    void Update()
    {

        InteractButton.SetActive(false);
        
        //Collider[] hitsCart = Physics.OverlapSphere(interactPoint.position, interactRange, cartLayers);
        //foreach (Collider interactable in hitsCart)
        //{
        //    
        //    InteractButton.SetActive(true);
        //
        //    HandleInteract();
        //}
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
