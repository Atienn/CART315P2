using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelectHandler : MonoBehaviour
{
    HoverSelect[] buttons;
    HoverSelect currSelected;

    void Awake()
    {
        //Bad practice, requires children of children to be disabled on start.
        buttons = GetComponentsInChildren<HoverSelect>();
        
        if(buttons.Length > 0) {
            currSelected = buttons[0];
        }
    }

    public void ResetSelect() {
        NewSelect(buttons[0]);
    }

    public void NewSelect(HoverSelect newSelected) {
        currSelected = newSelected;
    }
}
