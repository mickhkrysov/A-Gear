using System.Collections;
using TMPro;
using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue Data")]
    public NPCDialogue dialogueData;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;

    private int dialogueIndex;
    private bool isTyping;
    private bool isDialogueActive;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null) return;

        // If game is paused for other reasons, block starting a NEW dialogue.
        // But allow progressing if THIS dialogue is already active.
        if (PauseController.isGamePaused && !isDialogueActive) return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        if (dialoguePanel == null || dialogueText == null || nameText == null) return;
        if (dialogueData.dialogueLines == null || dialogueData.dialogueLines.Length == 0) return;

        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        dialoguePanel.SetActive(true);

        PauseController.SetPause(true);

        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            return;
        }

        dialogueIndex++;

        if (dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        string line = dialogueData.dialogueLines[dialogueIndex];

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        isTyping = false;

        if (dialogueText != null) dialogueText.SetText("");
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        PauseController.SetPause(false);
    }
}
