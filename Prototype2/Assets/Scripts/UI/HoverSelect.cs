using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class HoverSelect : MonoBehaviour, IPointerEnterHandler 
{
    public UnityEvent onSelect;
    [HideInInspector] public Toggle toggle;
    
    void Awake() {
        toggle = GetComponent<Toggle>();
    }

    public virtual void OnPointerEnter(PointerEventData data) {
        if(!toggle.isOn) {
            onSelect.Invoke();
            toggle.isOn = true;
        }
    }
}
