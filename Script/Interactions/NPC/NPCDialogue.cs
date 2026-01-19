using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    [Header("Info")]
    public string npcName;
    [TextArea(3, 5)]
    public string[] dialogueLines;

    [Header("Typing Settings")]
    public float typingSpeed = 0.01f;

    [Header("Auto Progress Settings")]
    // NOTE: name matches what NPC.cs expects
    public bool[] autoProgressLines;

}
