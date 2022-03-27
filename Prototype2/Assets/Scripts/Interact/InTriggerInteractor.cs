using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InTriggerInteractor : Interactor {
    List<Interactable> inRangeOf;

    void Start() {
        inRangeOf = new List<Interactable>();
    }

    private void OnTriggerEnter(Collider other) {
        Interactable entered;

        //Check if the trigger collider is an interactable.
        if(other.TryGetComponent<Interactable>(out entered)) {
            inRangeOf.Add(entered);
            selected = inRangeOf[inRangeOf.Count - 1];

            onEnter.Invoke();
            entered.Enter();
        }
        //otherwise, be angry >:(
    }

    private void OnTriggerExit(Collider other) {
        Interactable left;

        //Check if the trigger collider is an interactable.
        if (other.TryGetComponent<Interactable>(out left)) {
            if (inRangeOf.Remove(left)) {
                onLeave.Invoke();
                left.Leave();

                selected = inRangeOf.Count > 0 ? inRangeOf[inRangeOf.Count - 1] : null;
            }
            else { Debug.LogWarning("Interactor left an interactable that can't be found."); }
        }
    }
}
