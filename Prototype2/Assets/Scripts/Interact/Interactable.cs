using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltEvents;

public abstract class Interactable : MonoBehaviour {
    [Header("Interactable")]
    [SerializeField] UltEvent onEnter;
    [SerializeField] UltEvent onLeave;

    public abstract void Interact(Interactor sender);

    public virtual void Enter() {
        onEnter.Invoke();
    }

    public virtual void Leave() {
        onLeave.Invoke();
    }
}
