using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public MapController mapController;
    public int mapSize {get; set;}
    public int playerCount { get; set; }
    public GameObject currentPlayer;
    public CameraManager cameraManager;
    public PlayerManager playerManager;
    public UserInterfaceManager userInterfaceManager;

    private void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start() {
        //Set Defaults for the mapSize and playerCount
        mapSize = 1;
        playerCount = 1;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void NewGame() { 

        cameraManager.cameraHolder = GameObject.Find("Cameras");


        mapController = GameObject.Find("Map").GetComponent<MapController>();
        playerManager = GameObject.Find("Players").GetComponent<PlayerManager>();
        userInterfaceManager = GameObject.Find("UserInterface").GetComponent<UserInterfaceManager>();

        //this should be abstracted
        mapController.playerCount = playerCount;
        mapController.setMapTransform();
        //mapController.GenerateFieldBlueprint(mapSize);
        //mapController.GenerateFieldFromBlueprint();
        mapController.GenerateFlatHexagonBlueprint(mapSize);
        mapController.GenerateFlatHexField();

        //This should be abstracted
        for (int i = 0; i < playerCount; i++) {
            if(i < 4) {
                registerPlayer(mapController.townTileList[i]);
            }
        }

        //This should be simplified
        playerManager.playerObjectsList[0].transform.Find("PlayerController").GetComponent<PlayerControl>().setMyTurn(true);
    }

    private void registerPlayer(Tile townTile) {
        GameObject newPlayer = playerManager.CreatePlayer(townTile);
        cameraManager.SetupPlayerCamera(newPlayer);
        userInterfaceManager.SetupPlayerUI(newPlayer);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode aMode) {
        if(SceneManager.GetActiveScene().buildIndex == 1) {
            NewGame();
        }
    }

    public void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
