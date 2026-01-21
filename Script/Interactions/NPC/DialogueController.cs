using UnityEngine;
using TMPro;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine.UI;

//controller so all files don't have to manage dialogue UI separately
public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance{get; private set;}
    
    public GameObject dialogueUI;
    public TMP_Text dialogueText, nameText;
    public Transform ChoicePanel;
    public GameObject choiceButtonPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDialogueUI(bool show)
    {
        dialogueUI.SetActive(show);
    }

    public void SetNPCInfo(string npcName)
    {
        nameText.SetText(npcName);
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach(Transform child in ChoicePanel) Destroy(child.gameObject);
    }

    public GameObject CreateChoiceButton(string ChoiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, ChoicePanel);
        choiceButton.GetComponentInChildren<TMP_Text>().text = ChoiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
        return choiceButton;
    }
}
