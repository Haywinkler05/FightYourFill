using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Used to add or remove an InteractionEvent component to a gameObject
    public bool useEvents;
    // Message delivered to player when looking at the interactable
    [SerializeField]
    public string promptMessage;

    public virtual string OnLook()
    {
        return promptMessage;
    }

    // Function called from player script
    public void BaseInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }
        Interact();
    }
    protected virtual void Interact()
    {
        // No code in this function definition
        // Simply acts as a template function for overriding by child classes
    }
}
