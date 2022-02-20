using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class CombatLog : Singleton<CombatLog>
{
    [SerializeField] TMP_Text[] displayList = new TMP_Text[10];
    List<LogText> logTexts = new List<LogText>(10);

    [SerializeField] int removeDelay = 150;

    protected override void Start() {
        base.Start();
    }

    private void FixedUpdate() {
        if(logTexts.RemoveAll(o => --o.time < 0) > 0) {
            GenerateList();
        }
    }

    public void Log(string toDisplay) {
        logTexts.Insert(0, new LogText { text = toDisplay, time = removeDelay });
        if(logTexts.Count > displayList.Length) {
            logTexts.RemoveRange(displayList.Length, logTexts.Count - displayList.Length);
        }
        GenerateList();
    }

    public void Clear() {
        logTexts.Clear();

        //Erase the information shown.
        foreach (TMP_Text text in displayList) {
            text.text = "";
        }
    }

    public void GenerateList() {
        //Erase the information shown.
        foreach (TMP_Text text in displayList) {
            text.text = "";
        }

        //Write the text of each log.
        for (int i = 0; i < logTexts.Count; i++) {
            displayList[i].text = logTexts[i].text;
        }
    }
}

[System.Serializable]
public class LogText {
    public string text;
    public int time;
}