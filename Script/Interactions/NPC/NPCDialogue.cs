using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    [Header("Info")]
    public string npcName;
    [TextArea(3, 5)]
    public string[] dialogueLines;

    [Header("Typing Settings")]
    public float typingSpeed = 10f;

    [Header("Auto Progress Settings")]
    public bool[] autoProgressLines;
    public bool[] endDialogueLines;     //where dialogue ends

    public DialogueChoice[] dialogueChoices;

}

[System.Serializable]

public class DialogueChoice
{
    public int dialogueIndex;
    public string[] choice;
    public int[] nextDialogueIndexes;
}