using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [TextArea]
    public string promptMessage = "Interact";

    public string GetDescription()
    {
        return promptMessage;
    }

    public abstract void Interact();
}