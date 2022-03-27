using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    [SerializeField] KeyCode interactKey;
    protected Interactable selected;
    [HideInInspector] public bool locked;

    [SerializeField] protected UltEvent onInteract;
    [SerializeField] protected UltEvent onEnter;
    [SerializeField] protected UltEvent onLeave;

    private void Start() {
        locked = false;
    }

    void Update() {

        if(Input.GetKeyDown(interactKey)) {
            if(locked) {
                selected.Interact(this);
            }
            else if(selected != null) {
                selected.Interact(this);
                onInteract.Invoke();
            }
        }
    }
}
