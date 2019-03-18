using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public Camera ViewCamera;
    TownTile myTown;

    public GameObject currentSelection;

    public bool myTurn { get; set; }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        handleUserInterfacing();
    }

    private void handleUserInterfacing() {
        if (!myTurn) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Ray ray = ViewCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {

                Debug.Log("We Hit: " + hit.collider.name + "\n at position: " + hit.point);

                //If we hit nothing, set our selection to null

                //Highlight the clicked object and set it to our currentSelection
            }
        }
    }

    public void setMyTurn(bool isMyTurn) {
        myTurn = isMyTurn;
        Camera.main.enabled = !isMyTurn;
        ViewCamera.enabled = isMyTurn;
    }
}
