using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveWithAmplitude : MonoBehaviour
{
    [SerializeField] AudioAnalyzer analyzer;
    [SerializeField] float angle;
    [SerializeField] float multiplier;
    RectTransform position;
    Vector2 basePosition;
    Vector2 moveDir;

    void Awake() {
        position = GetComponent<RectTransform>();
        basePosition = position.anchoredPosition;
        moveDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    void Update() {
        Vector2 distance = basePosition - position.anchoredPosition;
        position.anchoredPosition += distance * distance.magnitude * 0.001f;

        position.anchoredPosition += moveDir * analyzer.amplitude * multiplier;
    }
}
