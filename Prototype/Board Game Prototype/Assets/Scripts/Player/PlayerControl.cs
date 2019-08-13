using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour {
    public int id { get; set; }
    public Material playerMaterial { get; set; }
    public Camera viewCamera;
    public Tile townTile { get; set; }
    public GameObject selectedObject;
    public GameObject movingHero;
    public Shader selectedObjectShader;
    public bool myTurn { get; set; }
    public bool MoveButtonPressed = false;


    public PlayerUIControl playerUIControl;

    public PlayerPathFinder playerPathFinder;

    public List<HeroControl> heroControllersList;

    public event EventHandler onMyTurn;

    void Start() {
        SubscribeToHeroMovement();
        playerPathFinder = transform.GetComponentInChildren<PlayerPathFinder>();
    }

    void Update() {
        HandleUserInterfacing();
    }

    private void HandleUserInterfacing() {
        if (!myTurn)
            return;

        HandlePlayerMovement();

        HandlePlayerSelection();
    }

    private void HandlePlayerMovement() {
        if (MoveButtonPressed) {
            HeroControl heroControl = movingHero.GetComponent<HeroControl>();
            playerPathFinder.PathFindForHero(movingHero.GetComponent<HeroControl>(), viewCamera);
            if (selectedObject.tag == "Tile") {
                movingHero.GetComponent<HeroControl>().MoveToTile(selectedObject);
                MoveButtonPressed = false;
                DeselectAll();
            }
        }
    }

    #region Selection Handling
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

    private void TileSelected(GameObject tileMeshObject) {
        //This block of code handles how selection is indicated on a tile.
        //Ultimately this will be moved into a selectionManager class
        selectedObject = tileMeshObject.transform.parent.gameObject;
        selectedObjectShader = selectedObject.GetComponentInChildren<MeshRenderer>().material.shader;
        selectedObject.GetComponentInChildren<MeshRenderer>().material.shader = Shader.Find("Outlined/UltimateOutline");
        selectedObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_FirstOutlineColor", new Color(255, 207, 0, 1));
        selectedObject.GetComponentInChildren<MeshRenderer>().material.SetFloat("_FirstOutlineWidth", 0.1f);
        selectedObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_SecondOutlineColor", new Color(255, 207, 0, 0));
        selectedObject.GetComponentInChildren<MeshRenderer>().material.SetFloat("_SecondOutlineWidth", 0.0f);

        playerUIControl.TileSelected(selectedObject.GetComponent<Tile>());
    }

    private void HeroSelected(GameObject heroMeshObject) {
        //This block of code handles how selection is indicated on a tile.
        //Ultimately this will be moved into a selectionManager class
        selectedObject = heroMeshObject;
        HeroControl heroControl = heroMeshObject.GetComponent<HeroControl>();
        selectedObjectShader = selectedObject.GetComponentInChildren<MeshRenderer>().material.shader;
        selectedObject.GetComponentInChildren<MeshRenderer>().material.shader = Shader.Find("Outlined/UltimateOutline");
        selectedObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_FirstOutlineColor", new Color(0, 255, 255, 1));
        selectedObject.GetComponentInChildren<MeshRenderer>().material.SetFloat("_FirstOutlineWidth", 0.1f);
        selectedObject.GetComponentInChildren<MeshRenderer>().material.SetColor("_SecondOutlineColor", new Color(0, 255, 255, 0));
        selectedObject.GetComponentInChildren<MeshRenderer>().material.SetFloat("_SecondOutlineWidth", 0.0f);

        bool isMyHero = false;
        if (heroControllersList.Contains(heroControl)) {
            isMyHero = true;
        }
        playerUIControl.HeroSelected(heroControl, isMyHero);

    }

    private void DeselectAll() {
        selectedObject.GetComponentInChildren<MeshRenderer>().material.shader = selectedObjectShader;
        movingHero = null;
        selectedObject = null;
        playerUIControl.Deselection();
    }

    private void DeselectSelectedObject() {
        selectedObject.GetComponentInChildren<MeshRenderer>().material.shader = selectedObjectShader;
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
            townTile.transform.GetChild(0).GetComponent<MeshRenderer>().material = playerMaterial;

        }
        else {
            Debug.Log("townTile was already occupied");
        }
    }

    #endregion


    private void SubscribeToHeroMovement() {
        playerUIControl.moveHero += onMoveHero;
    }

    public void addHero(HeroControl hero) {
        hero.setMaterial(playerMaterial);
        heroControllersList.Add(hero);
        Vector3 startPosition = townTile.transform.position;
        startPosition.y += 0.25f;
        hero.transform.position = startPosition;

        hero.currentTile = townTile;
    }

    public void onMoveHero(GameObject selectedHero) {
        movingHero = selectedHero;
        MoveButtonPressed = true;
    }
}
