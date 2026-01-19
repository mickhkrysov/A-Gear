using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialogueUI;   //dialogue UI panel
    public TMP_Text dialogueText, nameText;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    void Update()
    {
        if (isDialogueActive && !isTyping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextLine();
            }
        }

        if (isDialogueActive && isTyping)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipTyping();
        }
}
    }

    void SkipTyping()
    {
        StopAllCoroutines();
        dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
        isTyping = false;
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if(dialogueData == null || (PauseController.isGamePaused && !isDialogueActive))
            return;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex=0;
        nameText.SetText(dialogueData.npcName);
        
        dialogueUI.SetActive(true);
        PauseController.SetPause(true);

        StartCoroutine(TypeLine());

    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            
        }
        else if (dialogueIndex + 1 < dialogueData.dialogueLines.Length)
        {
            dialogueIndex++;
            StartCoroutine(TypeLine());
        }
        
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
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
        dialogueText.SetText("");
        dialogueUI.SetActive(false);
        PauseController.SetPause(false);
    }
}
