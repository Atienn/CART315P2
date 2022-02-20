using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverSelectParent : HoverSelect
{
    [SerializeField] HoverSelect[] children;

    public override void OnPointerEnter(PointerEventData data) {
        base.OnPointerEnter(data);
        foreach (HoverSelect select in children) {
            if (select.toggle.isOn) {
                select.onSelect.Invoke();
            }            
        }
    }
}
