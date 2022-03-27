using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtInteractor : Interactor
{
    [SerializeField] Transform camLookAt;
    [SerializeField] float interactDistance;


    void Start() {
        selected = null;
    }

    void Update() {
        RaycastHit hitInfo;
        Interactable check;

        //If the camera points to an object that is an interactable, set it as the current selected.
        if (Physics.Raycast(new Ray(camLookAt.position, camLookAt.rotation.eulerAngles), out hitInfo, interactDistance) 
            && hitInfo.collider.TryGetComponent<Interactable>(out check)) {
            selected = check;
        }
        else {
            selected = null;
        }
    }
}
