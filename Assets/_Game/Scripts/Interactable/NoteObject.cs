using UnityEngine;

public class NoteObject : InteractableObject
{
    [Header("Contenido de la Nota")]
    [TextArea(10, 20)]
    public string noteContent;

    public override void Interact()
    {
        NoteController.instance.ShowNote(noteContent);
    }
}