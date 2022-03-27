using System;
using System.Collections.Generic;
using UnityEngine;


public class NoteRecorder : MonoBehaviour
{
    [SerializeField] AudioSource music;
    //Having an array of the same type as the note hanlder allows copying to clipboard
    //from play mode and pasting in edit mode to 'save' the recording.
    [SerializeField] RhythmButton[] recorders = new RhythmButton[4];

    void Update()
    {
        foreach (RhythmButton record in recorders) {
            if(Input.GetKeyDown(record.key)) {
                record.notes.Add(new Note { time = music.timeSamples });
            }
        }
    }


    public List<Note>[] GetNotes() {
        List<Note>[] notes = new List<Note>[recorders.Length];
        for (int i = 0; i < recorders.Length; i++) {
            notes[i] = recorders[i].notes;
        }
        return notes;
    } 
}
