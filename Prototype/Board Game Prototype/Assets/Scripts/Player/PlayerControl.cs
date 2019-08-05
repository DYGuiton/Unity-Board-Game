﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour {
    public int id { get; set; }
    public Material playerMaterial { get; set; }
    public Camera viewCamera;
    public Tile townTile { get; set; }
    public GameObject selectedObject;
    public Shader selectedObjectShader;
    public bool myTurn { get; set; }

    public PlayerUIControl playerUIControl;

    public List<HeroControl> heroControllersList;

    public event EventHandler onMyTurn;

    void Start() {

    }

    void Update() {
        handleUserInterfacing();
    }

    private void handleUserInterfacing() {
        if (!myTurn)
            return;

        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Resets the previously selected object to it's original view
            if (selectedObject != null) {
                selectedObject.GetComponent<MeshRenderer>().material.shader = selectedObjectShader;
                playerUIControl.Deselection();
                selectedObject = null;
            }

            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.GetComponentInParent<Tile>() != null) {
                    TileSelected(hit.collider.gameObject);
                }
                if (hit.collider.gameObject.GetComponentInParent<HeroControl>() != null) {
                    HeroSelected(hit.collider.gameObject);
                }
            }
        }
    }

    private void TileSelected(GameObject tileMeshObject) {
        //This block of code handles how selection is indicated on a tile.
        //Ultimately this will be moved into a selectionManager class
        selectedObject = tileMeshObject;
        selectedObjectShader = selectedObject.GetComponent<MeshRenderer>().material.shader;
        selectedObject.GetComponent<MeshRenderer>().material.shader = Shader.Find("Outlined/UltimateOutline");
        selectedObject.GetComponent<MeshRenderer>().material.SetColor("_FirstOutlineColor", new Color(255, 207, 0, 1));
        selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_FirstOutlineWidth", 0.1f);
        selectedObject.GetComponent<MeshRenderer>().material.SetColor("_SecondOutlineColor", new Color(255, 207, 0, 0));
        selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_SecondOutlineWidth", 0.0f);

        playerUIControl.TileSelected(tileMeshObject.transform.parent.GetComponent<Tile>());
    }

    private void HeroSelected(GameObject heroMeshObject) {
        //This block of code handles how selection is indicated on a tile.
        //Ultimately this will be moved into a selectionManager class
        selectedObject = heroMeshObject;
        HeroControl heroControl = heroMeshObject.transform.parent.GetComponent<HeroControl>();
        selectedObjectShader = selectedObject.GetComponent<MeshRenderer>().material.shader;
        selectedObject.GetComponent<MeshRenderer>().material.shader = Shader.Find("Outlined/UltimateOutline");
        selectedObject.GetComponent<MeshRenderer>().material.SetColor("_FirstOutlineColor", new Color(0, 255, 255, 1));
        selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_FirstOutlineWidth", 0.1f);
        selectedObject.GetComponent<MeshRenderer>().material.SetColor("_SecondOutlineColor", new Color(0, 255, 255, 0));
        selectedObject.GetComponent<MeshRenderer>().material.SetFloat("_SecondOutlineWidth", 0.0f);


        if (heroControllersList.Contains(heroControl)) {
            playerUIControl.HeroSelected(heroControl, true);
        } else {
            playerUIControl.HeroSelected(heroControl, false);
        }

    }

    public void setMyTurn(bool isMyTurn) {
        myTurn = isMyTurn;

        if (isMyTurn) {
            onMyTurn(this, new EventArgs());
            playerUIControl.TurnOn();
        }
        else {
            playerUIControl.gameObject.SetActive(false);
        }
    }

    public void setTownTile(Tile nuTownTile, Material nuPlayerMaterial) {
        if (nuTownTile.GetComponent<TownTile>().myPlayer == null) {
            townTile = nuTownTile;
            playerMaterial = nuPlayerMaterial;
            townTile.transform.GetChild(0).GetComponent<MeshRenderer>().material = playerMaterial;

        }
        else {
            Debug.Log("townTile was already occupied");
        }
    }

    public void addHero(HeroControl hero) {
        hero.setMaterial(playerMaterial);
        heroControllersList.Add(hero);
        hero.transform.position = townTile.transform.position;

    }
}