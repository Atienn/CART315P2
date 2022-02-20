using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventList : MonoBehaviour
{
    [SerializeField] UnityEvent onStart;
    [SerializeField] UnityEvent[] onProgress;
    int count = 0;

    public void Start() {
        onStart.Invoke();
    }

    public void InvokeNext() {
        onProgress[count++].Invoke();

        if(count > onProgress.Length) {
            this.enabled = false;
        }
    }
}
