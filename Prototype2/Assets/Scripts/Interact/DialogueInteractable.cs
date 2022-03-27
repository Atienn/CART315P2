using System;
using UnityEngine;
using UltEvents;
using UnityEngine.Events;

public class DialogueInteractable : Interactable {
    [Header("Dialogue")]
    [SerializeField] AudioClip theme;
    [SerializeField] float volume;
    [SerializeField] DialogueData data;

    Interactor target;
    bool active;

    private void Start() {
        active = false;
    }

    public override void Interact(Interactor sender) {
        if(active) {
            if (sender == target && DialogueManager.Instance.NextPage()) { DialogueEnd(); }
        }
        else {
            if(DialogueManager.Instance.StartDialogue(data)) {
                target = sender;
                target.locked = true;
                active = true;

                AudioManager.Instance.PlayTheme(theme, volume);
            }
        }
    }

    public override void Leave() {
        base.Leave();
        if(DialogueManager.Instance.TryEndDialogue(data)) { DialogueEnd(); }
    }

    public void DialogueEnd() {
        active = false;
        target.locked = false;
        target = null;
    }
}


