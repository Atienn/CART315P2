#if (UNITY_EDITOR) 

using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class AudioEventGenerator : MonoBehaviour
{
    [SerializeField] AudioEventTimer turnTimer;
    [SerializeField] AudioSource source;

    [SerializeField] int measureLength;
    [SerializeField] int startDelay;
    [SerializeField] bool clearOnGenerate = true;

    [SerializeField] UnityEvent toAdd;

    private void Start() {
        this.enabled = false;
    }

    void Update() {
        Debug.Log("AUDIO EVENTS GENERATED");

        if(clearOnGenerate) {
            turnTimer.events.Clear();
        }

        bool nextTurn = false;
        for(int i = startDelay; i < source.clip.samples; i += measureLength) {

            turnTimer.events.Add(new TimedEvent { time = i, trigger = toAdd });
            nextTurn = !nextTurn;
        }

        this.enabled = false;
    }

    public void SampleMethod() { }
}

#endif