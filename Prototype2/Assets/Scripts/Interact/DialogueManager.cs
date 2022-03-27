using System;
using UnityEngine.UI;
using UnityEngine;
using UltEvents;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] KeyCode NextKey;
    [SerializeField] KeyCode ExitKey;

    [SerializeField] GameObject dialogBox;

    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text mainText;

    [SerializeField] Button choiceBtn1;
    [SerializeField] Button choiceBtn2;
    [SerializeField] TMP_Text choiceName1;
    [SerializeField] TMP_Text choiceName2;

    DialogueData data;
    int index;
    bool onChoice;


    public bool StartDialogue(DialogueData newData) {
        if(this.enabled) {
            return false;
        }

        titleText.text = newData.title;
        data = newData;
        index = -1;
        NextPage();

        dialogBox.SetActive(true);
        this.enabled = true;

        return true;
    }

    //Returns if dialogue has ended.
    public bool NextPage() {
        if (onChoice) { return false; } 
        
        //Advance to next page and check if still within page boundaries.
        if(++index < data.pages.Length) {
            //Invoke the attached event if there is one.           
            data.pages[index].ev.InvokeX();
            //Set the page's text.
            mainText.text = data.pages[index].line;
            return false;
        }
        //If not, end dialogue.
        else {
            EndDialogue();
            return true;
        }
    }


    public void EndDialogue() {
        this.enabled = false;
        dialogBox.SetActive(false);

        AudioManager.Instance.ResumeBackground();

        titleText.text = "";
        mainText.text = "";
        data = null;
    }

    //Returns if diaogue has ended.
    public bool TryEndDialogue(DialogueData check) {
        if(check == data) {
            EndDialogue();
            return true;
        }
        return false;
    }


    public void SetChoice(ChoiceHolder choice) {
        choiceName1.text = choice.choiceName1;
        choiceName2.text = choice.choiceName2;

        choiceBtn1.gameObject.SetActive(true);
        choiceBtn2.gameObject.SetActive(true);

        choiceBtn1.onClick.AddListener(() => choice.onChoose1.Invoke());
        choiceBtn2.onClick.AddListener(() => choice.onChoose2.Invoke());

        onChoice = true;
    }


    public void SelectChoice() {
        choiceName1.text = "";
        choiceName2.text = "";

        choiceBtn1.gameObject.SetActive(false);
        choiceBtn2.gameObject.SetActive(false);

        choiceBtn1.onClick.RemoveAllListeners();
        choiceBtn2.onClick.RemoveAllListeners();

        onChoice = false;
        NextPage();
    }
}
