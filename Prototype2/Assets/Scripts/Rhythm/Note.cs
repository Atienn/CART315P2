using System;
using UnityEngine;

[Serializable]
public class Note { 
    public int time;
    [HideInInspector] public Transform display;
}

[Serializable]
public class LongNote : Note {
    public int length;
}
