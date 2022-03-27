using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Flasher : MonoBehaviour {
    CanvasGroup element;
    [SerializeField] float fade = 0.005f;


    void Start() {
        element = GetComponent<CanvasGroup>();
    }

    public void Flash() {
        this.enabled = true;
        element.alpha = 1f;
    }

    void FixedUpdate() {
        element.alpha -= fade;
        if (element.alpha <= 0) {
            this.enabled = false;
        }
    }
}
