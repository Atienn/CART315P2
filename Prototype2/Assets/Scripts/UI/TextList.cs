using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TextList
{
    [SerializeField] TMP_Text[] textList = new TMP_Text[5];
    int count;

    //public void AddFront(string text) {
    //    displayList[count].text = text;
    //    count = (count + 1) % textList.Length;
    //}

    public void AddLast(string text) {
        textList[count].text = text;
        count = (count + 1) % textList.Length;
    }

    public void ClearDisplay() {
        foreach (TMP_Text text in textList) {
            text.text = "";
        }
        count = 0;
    }
}
