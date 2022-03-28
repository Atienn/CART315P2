using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAudio : Singleton<CombatAudio>
{
    public AudioSource source;
    [SerializeField] [Range(0f, 1f)] float fadeSpeed;

    public void PauseAudio() {
        StopAllCoroutines();
        StartCoroutine(ShiftTo(0f));
    }

    public void ResumeAudio() {
        StopAllCoroutines();
        source.UnPause();
        StartCoroutine(ShiftTo(1));
    }

    public IEnumerator<WaitForFixedUpdate> ShiftTo(float target) {
        while (source.pitch != target) {
            source.pitch = Mathf.MoveTowards(source.pitch, target, fadeSpeed);
            yield return new WaitForFixedUpdate();
        }

        if(source.pitch == 0f) {
            source.Pause();
        }
    }
}
