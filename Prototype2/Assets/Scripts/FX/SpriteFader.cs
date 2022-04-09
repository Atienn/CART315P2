using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFader : MonoBehaviour {
    SpriteRenderer render;

    [SerializeField] [Range(0f, 1f)] float delta;

    [SerializeField] bool startHidden;
    float currentAlpha;

    //Strategy pattern, I hate using if statements.
    VoidStrategy RampStrategy;


    void Start() {
        render = GetComponent<SpriteRenderer>();
        RampStrategy = StandardMethods.None;


        if (startHidden) {
            render.color = new Color(render.color.r, render.color.g, render.color.b, 0f);

            //Hide the light.
            render.enabled = false;
            //Prevents update functions from being called.
            this.enabled = false;
        }
    }

    void FixedUpdate() {
        //Ensures the ramp up/down is at fixed speed.
        RampStrategy();
    }

    void Update() {
        //Only modify sprite color if the change will be visible.
        render.color = new Color(render.color.r, render.color.g, render.color.b, currentAlpha);
    }


    //Public methods to be invoked from events.
    public void FadeIn() {
        //Show the light.
        render.enabled = true;
        //Allows update function to take effect.
        this.enabled = true;

        RampStrategy = RampUp;
    }
    public void FadeOut() {
        RampStrategy = RampDown;
    }


    //Ramp strategies.
    void RampUp() {
        currentAlpha += delta;

        if (currentAlpha >= 1f) {
            currentAlpha = 1f;
            RampStrategy = StandardMethods.None;
        }
    }
    void RampDown() {
        currentAlpha -= delta;

        if (currentAlpha <= 0f) {
            currentAlpha = 0f;
            RampStrategy = StandardMethods.None;

            render.enabled = false;
            this.enabled = false;
        }
    }
}
