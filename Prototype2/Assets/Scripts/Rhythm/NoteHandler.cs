using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteHandler : MonoBehaviour {
    static Color buttonDefault = new Color(1f, 1f, 1f, 0.25f);
    static Color buttonHit = Color.white;

    [Header("General")]
    [SerializeField] AudioSource music;
    [SerializeField] GameObject notePrefab;

    [Header("Gameplay")]
    [SerializeField] int hitReward = 1;
    [SerializeField] int failPenality = 1;
    [SerializeField] int hitWindow;
    [SerializeField] int slowFactor;
    int score;

    [Header("FX")]
    [SerializeField] Flasher hitFeedback;
    [SerializeField] Flasher failFeedback;
    [SerializeField] ImageGauge timeRemaining;
    [SerializeField] TextFlasher[] text = new TextFlasher[2];

    [Header("Chart")]
    [SerializeField] RhythmButton[] buttons = new RhythmButton[4];

    // Start is called before the first frame update
    void Start() {
        for (byte i = 0; i < buttons.Length; i++) {

            Note last = null;
            foreach (Note note in buttons[i].notes) {
                if (last != null && note.time <= last.time) {
                    Debug.LogWarning($"Invalid timed notes ordering in button {i}.");
                }
                note.display = Instantiate(notePrefab, buttons[i].arrow.transform).transform;
                last = note;
            }
        }
        timeRemaining.maxValue = music.clip.samples;

        DisplayNotes();
    }

    void Update() {

        #region Hit React

        for (byte i = 0; i < buttons.Length; i++) {
            if (Input.GetKeyDown(buttons[i].key)) {
                buttons[i].arrow.color = buttonHit;
                buttons[i].hit = true;
            }
            else if (Input.GetKeyUp(buttons[i].key)) {
                buttons[i].arrow.color = buttonDefault;
            }
        }

        #endregion


        #region Note Check

        //Not merged with the above since buttons should work even if they have no notes left.
        foreach (RhythmButton btn in buttons) {
            //Skip if has no more notes.
            if (btn.notes.Count <= 0) { continue; }

            if (btn.hit && Math.Abs(btn.notes[0].time - music.timeSamples) < hitWindow) {
                Destroy(btn.notes[0].display.gameObject);
                btn.notes.RemoveAt(0);

                score += hitReward;
                hitFeedback.Flash();
            }

            else if (music.timeSamples - btn.notes[0].time > hitWindow) {
                Destroy(btn.notes[0].display.gameObject);
                btn.notes.RemoveAt(0);

                score -= failPenality;
                failFeedback.Flash();
            }
        }

        #endregion

        //Reset the buttons.
        for (byte i = 0; i < buttons.Length; i++) {
            buttons[i].hit = false;
        }

        DisplayNotes();
        timeRemaining.SetTargetOnly(music.timeSamples);
    }

    //void FixedUpdate() {  }

    //Positions notes where they shoule be on screen.
    void DisplayNotes() {
        for (byte i = 0; i < buttons.Length; i++) {
            foreach (Note note in buttons[i].notes) {
                note.display.localPosition = buttons[i].noteDirection * ((note.time - music.timeSamples) / slowFactor);
            }
        }
    }


    public void DisplayText(string input, int threshold, int textIndex) {
        if (score > threshold) {
            text[textIndex].Flash(input);
        }
        else {
            //Hide random characters from the string.
            int amtToHide = (int)(input.Length * ((threshold - score) / (float)threshold));
            for (int i = 0; i < amtToHide; i++) {
                input = input.Replace(input[(int)(UnityEngine.Random.value * input.Length)], '-');
            }
            text[textIndex].Flash(input);
        }
    }
    

    public void ResetScore() {
        //Debug.Log("Score: " + score);
        score = 0;
    }



    public void SetNotes(List<Note>[] notes, bool clear) {
        if(buttons.Length != notes.Length) {
            Debug.LogWarning("NON-MATCHING AMOUNT OF NOTE LISTS");
        }

        if(clear) {
            for (int i = 0; i < notes.Length; i++) {
                buttons[i].notes = notes[i];
            }
            Debug.Log("REPLACED CURRENT NOTE LISTS");
        }
        else {
            for (int i = 0; i < notes.Length; i++) {
                buttons[i].notes.AddRange(notes[i]);
            }
            Debug.Log("ADDED TO CURRENT NOTE LISTS");
        }
    }
}

[Serializable]
public class RhythmButton {
    public KeyCode key;
    public Image arrow;
    public Vector2 noteDirection;
    public List<Note> notes;
    internal bool hit;
}