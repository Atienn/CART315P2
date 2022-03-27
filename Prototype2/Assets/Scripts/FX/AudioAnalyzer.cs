using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;
using UnityEngine.UI;

public class AudioAnalyzer : MonoBehaviour {

    [Header("Source")]
    public AudioSource source;
    [SerializeField] short samplesFetchAmt;
    [SerializeField] short samplesDisplayAmt;
    [SerializeField] byte sampleChannel;
    //[SerializeField] float maxAmplitude;


    float[] spectrum;
    float[] spectrumRedux;
    Vector3[] wfVerticiesBase;
    Vector3[] wfVerticiesCurr;
    byte reduceFactor;
    private float amplitude;
    public float Amplitude { get { return amplitude; } }

    [Header("Waveform")]
    [SerializeField] LineRenderer waveform;
    [SerializeField] float visualScaling;
    [SerializeField] [Range(1f, 10f)] float baseSize;

    VoidStrategy WaveFormStrat;


    void Start() {

        //Replace samples amount with closest power of 2 (required for spectrum data).
        samplesFetchAmt = (short)Mathf.ClosestPowerOfTwo(samplesFetchAmt);
        samplesDisplayAmt = (short)Mathf.ClosestPowerOfTwo(samplesDisplayAmt);

        reduceFactor = (byte)(samplesFetchAmt / samplesDisplayAmt);

        SwitchChannel(sampleChannel);

        //Initialize spectrum data array.
        spectrum = new float[samplesFetchAmt];
        //Top half never gets used >:(
        //samplesFetchAmt /= (short)Mathf.ClosestPowerOfTwo(ignoreMultiplier);

        spectrumRedux = new float[samplesDisplayAmt];
        //spectrumReduxMax = new float[samplesDisplayAmt];
        wfVerticiesBase = new Vector3[samplesDisplayAmt];
        wfVerticiesCurr = new Vector3[samplesDisplayAmt];

        if(baseSize > 1) {
            WaveFormStrat = WaveFormStable;
        }
        else {
            baseSize = 1;
            WaveFormStrat = WaveFormDirect;
        }

        //Initialize the base array of verticies (forms a circle).
        for (short i = 0; i < wfVerticiesBase.Length; i++) {
            wfVerticiesBase[i] =
                new Vector3(
                    baseSize * Mathf.Cos(2 * Mathf.PI * i / samplesDisplayAmt),
                    -baseSize * Mathf.Sin(2 * Mathf.PI * i / samplesDisplayAmt),
                    0
                );
        }

        Array.Copy(wfVerticiesBase, wfVerticiesCurr, wfVerticiesBase.Length);

        waveform.positionCount = samplesDisplayAmt;
        waveform.SetPositions(wfVerticiesBase);
    }



    void Update() {
        amplitude = 0;
        WaveFormStrat();
    }


    int j = 0; 
    float total;

    void WaveFormDirect() {

        //Get data from audio clip.
        source.GetSpectrumData(spectrum, sampleChannel, FFTWindow.Blackman);

        j = 0;
        //Make verticies match position 
        for (short i = 0; i < samplesDisplayAmt; i++) {

            total = 0;
            for (j = i; j < reduceFactor * (i+1); j++) {
                total += spectrum[i];
            }
            spectrumRedux[i] = total / reduceFactor;
            amplitude += total;

            wfVerticiesCurr[i] = (wfVerticiesCurr[i] + wfVerticiesBase[i] * spectrumRedux[i] * visualScaling) * 0.5f;
        }
        waveform.SetPositions(wfVerticiesCurr);
    }

    void WaveFormStable() {
        //Get data from audio clip.
        source.GetSpectrumData(spectrum, sampleChannel, FFTWindow.Blackman);

        j = 0;
        //Make verticies match position 
        for (short i = 0; i < samplesDisplayAmt; i++) {

            total = 0;
            for (j = i; j < reduceFactor * (i + 1); j++) {
                total += spectrum[i];
            }
            spectrumRedux[i] = total / reduceFactor;
            amplitude += total;

            wfVerticiesCurr[i] = wfVerticiesBase[i] + (wfVerticiesCurr[i] + wfVerticiesBase[i] * spectrumRedux[i] * visualScaling) * 0.5f;
        }
        waveform.SetPositions(wfVerticiesCurr);
    }

    public void SwitchChannel(int switchTo) {
        sampleChannel = switchTo > 1 ? (byte)1 : (byte)switchTo;
    }
}
