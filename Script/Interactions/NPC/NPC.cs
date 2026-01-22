using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    private DialogueController dialogueUI;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    void Start()
    {
        dialogueUI = DialogueController.Instance;
    }

    void Update()
    {
        // While dialogue is active:
        if (isDialogueActive)
        {
            // If line is still typing finish it on Space
            if (isTyping && Input.GetKeyDown(KeyCode.Space))
            {
                SkipTyping();
            }
            // If line finished typing go to next line on Space
            else if (!isTyping && Input.GetKeyDown(KeyCode.Space))
            {
                NextLine();
            }
        }
    }

    void SkipTyping()
    {
        StopAllCoroutines();
        dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
        isTyping = false;
    }

    public bool CanInteract()
    {
        // can only start interaction if dialogue isn’t already active
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null || (PauseController.isGamePaused && !isDialogueActive))
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
        dialogueIndex = 0;

        dialogueUI.SetNpcInfo(dialogueData.npcName);
        dialogueUI.ShowDialogueUI(true);
        PauseController.SetPause(true);

        DisplayCurrentLine();
    }

    void NextLine()
    {
        // If text is still typing, just finish the current line
        if (isTyping)
        {
            SkipTyping();
            return;
        }

        // Clear old choice buttons
        dialogueUI.ClearChoices();

        // 1) Check if this index should END dialogue
        if (dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        // 2) Check if this index has choices
        if (dialogueData.dialogueChoices != null)
        {
            foreach (DialogueChoice dialogueChoice in dialogueData.dialogueChoices)
            {
                if (dialogueChoice.dialogueIndex == dialogueIndex)
                {
                    DisplayChoice(dialogueChoice);
                    return;
                }
            }
        }

        // 3) Otherwise, move to the next line (normal linear dialogue)
        if (dialogueIndex + 1 < dialogueData.dialogueLines.Length)
        {
            dialogueIndex++;
            DisplayCurrentLine();
        }
        else
        {
            // No more lines then end dialogue
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text + letter);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
    }


    void ChooseOption(int nextIndex, string playerText)
    {
        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
    }

    void DisplayChoice(DialogueChoice choice)
    {
        for (int i = 0; i < choice.choices.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            string choiceText = choice.choices[i];
            dialogueUI.CreateChoiceButton(choiceText, ()=> ChooseOption(nextIndex, choiceText));
        }
    }


    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);
        PauseController.SetPause(false);
    }
}