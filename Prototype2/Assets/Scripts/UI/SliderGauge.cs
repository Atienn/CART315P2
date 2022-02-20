using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderGauge : MonoBehaviour 
{
    [SerializeField] float delta;
    public float target;
    Slider slider;

    // Start is called before the first frame update
    void Start() {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(slider.value == target) {
            this.enabled = false;
        }
        else {
            slider.value = Mathf.MoveTowards(slider.value, target, delta);
        }
    }

    public void SetTarget(float newTarget) {
        this.enabled = true;

        if(newTarget > slider.maxValue) {
            target = slider.maxValue;
        }
        else if(newTarget < slider.minValue) {
            target = slider.minValue;
        }
        else {
            target = newTarget;
        }
    }

    public void OffsetTarget(float offset) {
        this.enabled = true;

        target += offset;

        if (target > slider.maxValue) {
            target = slider.maxValue;
        }
        else if (target < slider.minValue) {
            target = slider.minValue;
        }
    }
}
