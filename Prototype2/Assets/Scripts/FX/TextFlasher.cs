using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TextFlasher : MonoBehaviour
{
    TMP_Text element;
    [SerializeField] float fade = 0.005f;


    void Start() {
        element = GetComponent<TMP_Text>();
    }

    public void Flash() {
        this.enabled = true;
        element.alpha = 1f;
    }

    public void Flash(string text) {
        element.text = text;
        Flash();
    }

    void FixedUpdate()
    {
        element.alpha -= fade;
        if(element.alpha <= 0) {
            this.enabled = false;
        }
    }
}
