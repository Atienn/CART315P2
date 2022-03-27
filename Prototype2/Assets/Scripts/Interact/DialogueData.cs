using System;
using UnityEngine;
using UltEvents;


[Serializable]
public class DialogueData {
    [SerializeField] public string title;
    [SerializeField] public DialoguePages[] pages;
}

[Serializable]
public class DialoguePages {
    [TextArea]
    public string line;
    public UltEvent ev;
}