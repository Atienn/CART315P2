using System;
using UnityEngine.UI;
using UnityEngine;
using UltEvents;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] KeyCode actKey;

    [Header("Dialog Box")]
    [SerializeField] GameObject dialogBox;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text mainText;

    [Header("Action Button")]
    [SerializeField] Button actionBtn;
    [SerializeField] TMP_Text actionText;

    DialogueData data;
    int index;


    private void Update() {
        if(Input.GetKeyDown(actKey)) {
            actionBtn.onClick.Invoke();
        }
    }

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

        actionBtn.gameObject.SetActive(false);
        actionText.text = "";
        actionBtn.onClick.RemoveAllListeners();
    }

    //Returns if diaogue has ended.
    public bool TryEndDialogue(DialogueData check) {
        if(check == data) {
            EndDialogue();
            return true;
        }
        return false;
    }


    public void SetAction(ActionHolder action) {
        actionText.text = action.actionText;

        actionBtn.gameObject.SetActive(true);
        actionBtn.onClick.AddListener(() => action.onAct.Invoke());
    }


    public void SelectChoice() {
        actionBtn.gameObject.SetActive(false);
        actionText.text = "";
        actionBtn.onClick.RemoveAllListeners();
    }
}
