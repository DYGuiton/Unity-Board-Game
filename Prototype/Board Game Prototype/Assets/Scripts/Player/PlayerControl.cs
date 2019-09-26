using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour {

    public int id { get; set; }
    public Material playerMaterial { get; set; }
    public Tile townTile { get; set; }
    public List<HeroControl> heroControllersList;

    public Camera viewCamera;
    public PlayerUIControl playerUIControl;
    public PlayerPathFinder playerPathFinder;
    public event EventHandler onMyTurn;

    public GameObject selectedObject;
    public GameObject movingHero;

    public bool myTurn { get; set; }
    public bool MoveButtonPressed = false;
    public bool cancelButtonPressed = false;

    void Start() {
        SubscribeToPlayerUIEvents();
        playerPathFinder = transform.GetComponentInChildren<PlayerPathFinder>();
    }

    void Update() {
        HandleUserInterfacing();
    }

    #region HandlingCode

    private void HandleUserInterfacing() {
        if (!myTurn)
            return;
        if (cancelButtonPressed) {
            CancelAll();
        }

        HandlePlayerMovement();

        HandlePlayerSelection();
    }

    private void HandlePlayerMovement() {
        if (MoveButtonPressed) {
            HeroControl heroControl = movingHero.GetComponent<HeroControl>();
            playerPathFinder.PathFindForHero(movingHero.GetComponent<HeroControl>(), viewCamera);
            if (selectedObject.tag == "Tile") {
                heroControl.MoveHeroAlongPath(playerPathFinder.pathSelected());
                MoveButtonPressed = false;
                CancelAll();
            }
        }
    }

    private void HandlePlayerSelection() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Resets the previously selected object to it's original view whether or not we clicked something
            if (selectedObject != null) {
                DeselectSelectedObject();
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

    #endregion

    #region Selection Handling

    private void TileSelected(GameObject tileMeshObject) {
        selectedObject = tileMeshObject.transform.parent.parent.gameObject;
        selectedObject.GetComponent<Tile>().Highlight();

        playerUIControl.TileSelected(selectedObject.GetComponent<Tile>());
    }

    private void HeroSelected(GameObject heroMeshObject) {
        HeroControl heroControl = heroMeshObject.GetComponent<HeroControl>();
        if (heroControl.Highlight()) {
            selectedObject = heroMeshObject;
            bool isMyHero = false;
            if (heroControllersList.Contains(heroControl)) {
                isMyHero = true;
            }
            playerUIControl.HeroSelected(heroControl, isMyHero);
        }
    }



    private void CancelAll() {
        DeselectSelectedObject();
        playerPathFinder.Disable();
        EndEvents();
        movingHero = null;
        selectedObject = null;
    }

    private void DeselectSelectedObject() {
        selectedObject.GetComponent<BoardObject>().Unhighlight();
        selectedObject = null;
        playerUIControl.Deselection();
    }

    #endregion

    #region Setters

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
        if (nuTownTile.GetComponent<TownTileControl>().myPlayer == null) {
            townTile = nuTownTile;
            playerMaterial = nuPlayerMaterial;
            //townTile.transform.Find("TileBase").GetComponent<MeshRenderer>().material = playerMaterial;
            townTile.transform.GetChild(0).transform.Find("TileBase").GetComponent<MeshRenderer>().material = playerMaterial;

        }
        else {
            Debug.Log("townTile was already occupied");
        }
    }

    public void SetNewHero(HeroControl hero) {
        hero.setMaterial(playerMaterial);
        heroControllersList.Add(hero);
        Vector3 startPosition = townTile.transform.position;
        startPosition.y += 0.25f;
        hero.transform.position = startPosition;

        hero.currentTile = townTile;
    }

    #endregion

    #region Event Handling

    private void SubscribeToPlayerUIEvents() {
        playerUIControl.moveHero += onMoveHero;
        playerUIControl.cancel += onCancel;
    }

    public void onMoveHero(GameObject selectedHero) {
        movingHero = selectedHero;
        MoveButtonPressed = true;
    }

    public void onCancel() {
        cancelButtonPressed = true;
    }

    private void EndEvents() {
        cancelButtonPressed = false;
        MoveButtonPressed = false;
    }

    #endregion
}
