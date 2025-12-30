using UnityEngine;

public class DialogueTrig : MonoBehaviour
{
    public Dialogue dialogue;
    [System.Obsolete]
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
