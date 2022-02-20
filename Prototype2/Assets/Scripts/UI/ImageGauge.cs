using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageGauge : MonoBehaviour
{
    Image gaugeDisplay;
    [SerializeField] TMPro.TMP_Text textDisplay;

    public float maxValue = 1;
    [Range(0f, 1f)] public float delta = 0.01f;
    [Range(0f, 1f)] public float target = 0f;

    void Start() {
        gaugeDisplay = GetComponent<Image>();
    }

    void FixedUpdate() {
        if (gaugeDisplay.fillAmount == target) {
            this.enabled = false;
        }
        else {
            gaugeDisplay.fillAmount = Mathf.MoveTowards(gaugeDisplay.fillAmount, target, delta);
        }
    }

    public void SetTargetAndText(float newTarget) {
        SetTargetOnly(newTarget);
        textDisplay.text = newTarget.ToString() + " / " + maxValue.ToString();
    }

    public void SetTargetOnly(float newTarget) {
        this.enabled = true;

        if (newTarget > maxValue) {
            newTarget = maxValue;
            target = 1f;
        }
        else if (newTarget < 0) {
            target = 0f;
        }
        else {
            target = newTarget / maxValue;
        }
    }
}
