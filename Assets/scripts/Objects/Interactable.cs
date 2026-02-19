using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Message delivered to player when looking at the interactable
    public string promptMessage;

    // Function called from player script
    public void BaseInteract()
    {
        Interact();
    }
    protected virtual void Interact()
    {
        // No code in this function definition
        // Simply acts as a template function for overriding by child classes
    }
}
