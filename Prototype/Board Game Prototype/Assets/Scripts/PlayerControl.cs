using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public Camera ViewCamera;
    TownTile myTown;
    public GameObject selectedObject;
    public bool myTurn { get; set; }

    void Start() {

    }

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

                //Resets the previously selected object to it's original view
                if (selectedObject != null) {
                    selectedObject.GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard");
                }

                //This block of code handles how selection is indicated on an object.
                selectedObject = hit.collider.gameObject;
                selectedObject.GetComponent<MeshRenderer>().material.shader = Shader.Find("Outlined/UltimateOutline");
                selectedObject.GetComponent<MeshRenderer>().material.SetColor("_FirstOutlineColor", new Color(255, 207, 0, 1));
                selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_FirstOutlineWidth", 0.1f);
                selectedObject.GetComponent<MeshRenderer>().material.SetColor("_SecondOutlineColor", new Color(255, 207, 0, 0));
                selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_SecondOutlineWidth", 0.0f);
            }
        }
    }

    public void setMyTurn(bool isMyTurn) {
        myTurn = isMyTurn;
        Camera.main.enabled = !isMyTurn;
        ViewCamera.enabled = isMyTurn;
    }
}
