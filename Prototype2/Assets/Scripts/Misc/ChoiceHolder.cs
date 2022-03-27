using System;
using UltEvents;
using UnityEngine;

[Serializable]
public class ChoiceHolder : MonoBehaviour
{
    public string choiceName1;
    public UltEvent onChoose1;
    [Space]
    public string choiceName2;
    public UltEvent onChoose2;
}
