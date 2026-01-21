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
        dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
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

        dialogueUI.SetNPCInfo(dialogueData.npcName);
        dialogueUI.ShowDialogueUI(true);
        PauseController.SetPause(true);

        DisplayCurrentLine();

    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
            
        }

        //clear choices
        dialogueUI.ClearChoices();

        //Check end Dialogue line
        if(dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        //check if choices&gisplay
        if (dialogueData.dialogueChoices != null)
        {
            foreach(DialogueChoice dialogueChoice in dialogueData.dialogueChoices)
            {
                if (dialogueChoice.dialogueIndex == dialogueIndex)
                {
                    DisplayChoice(dialogueChoice);
                    return;
                }
            }
        }

        else if (dialogueIndex + 1 < dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
            
        }
        
        else
        {
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
    

    void DisplayChoice(DialogueChoice choice)
    {
        for(int i=0; i<choice.choiceTexts.Length; i++)
        {
            int nextIndex = choice.nextDialogueIndexes[i];
            dialogueData.CreateChoiceButton(choice.choiceTexts[i], ()=> ChooseOption);
        }
        
    }

    void ChooseOption(int nextIndex)
    {
        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
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