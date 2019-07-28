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

    void Start() {
        DontDestroyOnLoad(transform.gameObject);
        //Set Defaults for the mapSize and playerCount
        mapSize = 1;
        playerCount = 1;
    }

    public void NewGame(Scene scene, LoadSceneMode mode) {

        cameraManager.cameraHolder = GameObject.Find("Cameras");

        mapController = GameObject.Find("Map").GetComponent<MapController>();
        playerManager = GameObject.Find("Players").GetComponent<PlayerManager>();

        //this should be abstracted
        mapController.playerCount = playerCount;
        mapController.setMapTransform();
        mapController.generateFieldBlueprint(mapSize);
        mapController.generateField();

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
    }
}
