using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource bgSource;
    [SerializeField] AudioSource themeSource;

    [SerializeField] [Range(0f, 1f)] float fadeSpeed;
    [SerializeField] [Range(0f, 1f)] float bgVolumeMin;
    float bgVolumeMax;

    [SerializeField] AudioAnalyzer waveform;

    protected override void Start() {
        base.Start();
        bgVolumeMax = bgSource.volume;
    }

    public void PlayTheme(AudioClip theme, float maxVolume) {
        StopAllCoroutines();

        if (theme == themeSource.clip) {
            themeSource.UnPause();
        }
        else {
            themeSource.clip = theme;
            themeSource.Play();
        }


        StartCoroutine(FadeTo(themeSource, bgSource, maxVolume));
        waveform.source = themeSource;
    }
    public void PlayTheme(AudioClip clip) {
        PlayTheme(clip, bgVolumeMax);
    }


    public void ResumeBackground() {
        StopAllCoroutines();

        bgSource.UnPause();

        StartCoroutine(FadeTo(bgSource, themeSource, bgVolumeMax));
        waveform.source = bgSource;
    }


    IEnumerator<WaitForFixedUpdate> FadeTo(AudioSource fadeIn, AudioSource fadeOut, float volumeTarget) {
        float outDelta = Mathf.Abs(bgVolumeMin - fadeOut.volume) * fadeSpeed;
        float inDelta = Mathf.Abs(volumeTarget - fadeIn.volume) * fadeSpeed;

        while (fadeIn.volume != volumeTarget || fadeOut.volume != bgVolumeMin) {
            fadeIn.volume = Mathf.MoveTowards(fadeIn.volume, volumeTarget, inDelta);
            fadeOut.volume = Mathf.MoveTowards(fadeOut.volume, bgVolumeMin, outDelta);

            yield return new WaitForFixedUpdate();
        }
        if(fadeOut != bgSource) fadeOut.Pause();
    }
}
