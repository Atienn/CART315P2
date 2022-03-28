using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor.Events;

[ExecuteInEditMode]
public class AudioTurnGenerator : MonoBehaviour
{
    [SerializeField] AudioEventTimer turnTimer;
    [SerializeField] AudioSource track;

    [SerializeField] int turnLength;
    [SerializeField] int startDelay;
    [SerializeField] bool startWith = false;
    [SerializeField] bool clearOnGenerate = true;

    private void Start() {
        this.enabled = false;
    }

    void Update() {
        Debug.Log("AUDIO TURNS GENERATED");

        if(clearOnGenerate) {
            turnTimer.events.Clear();
        }

        for(int i = startDelay; i < track.clip.samples; i += turnLength) {
            UnityEvent ev = new UnityEvent();
            UnityEventTools.AddBoolPersistentListener(ev, new UnityAction<bool>(TurnManager.Instance.TurnChange), startWith);

            turnTimer.events.Add(new TimedEvent { time = i, trigger = ev });

            startWith = !startWith;
        }

        this.enabled = false;
    }
}
