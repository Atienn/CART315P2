using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TransferNotes : MonoBehaviour
{
    [SerializeField] NoteRecorder transferFrom;
    [SerializeField] NoteHandler transferTo;
    [SerializeField] int offset = 0;
    [SerializeField] bool clearOnTransfer = true; 

    void Start() {
        enabled = false;
    }

    void Update() {

        if (offset != 0) {
            List<Note>[] from = transferFrom.GetNotes();
            List<Note>[] to = new List<Note>[from.Length];

            //Painstakingly (not really) offset every note.
            for (int i = 0; i < from.Length; i++) {
                to[i] = new List<Note>(from[i].Count);

                for (int j = 0; j < from[i].Count; j++) {
                    to[i][j] = new Note { time = from[i][j].time + offset };
                }
            }

            transferTo.SetNotes(to, clearOnTransfer);
        }
        else {
            transferTo.SetNotes(transferFrom.GetNotes(), clearOnTransfer);
        }

        Debug.Log("NOTES TRANSFERRED");
        enabled = false;
    }
}
