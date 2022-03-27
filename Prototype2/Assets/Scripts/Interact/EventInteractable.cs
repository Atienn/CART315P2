using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltEvents;

[RequireComponent(typeof(BoxCollider))]
public class EventInteractable : Interactable
{
    [SerializeField] UltEvent action;

    public override void Interact(Interactor sender) {
        action.Invoke();
    }
}
