using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingMapGameManager : MonoBehaviour {
    //This is a GameManager script that is only used to expedite testing of the MapScene

    MapController mapController;
    public int mapSize = 10;
    public int playerCount = 1;
    public GameObject currentPlayer;
    public CameraManager cameraManager;
    public PlayerManager playerManager;

    void Start() {
        DontDestroyOnLoad(transform.gameObject);
        cameraManager.cameraHolder = GameObject.Find("Cameras");
        playerManager.playerHolder = GameObject.Find("Players");

        mapController = GameObject.Find("Map").GetComponent<MapController>();
        mapController.setMapTransform();
        mapController.generateFieldBlueprint(mapSize);
        mapController.generateField();

        for(int i = 0; i < playerCount; i++) {
            if(i < 4) {
                registerPlayer(mapController.townTileList[i]);
            }
        }

        playerManager.playerObjectsList[0].transform.Find("PlayerController").GetComponent<PlayerControl>().setMyTurn(true);
    }

    private void registerPlayer(Tile townTile) {
        GameObject newPlayer = playerManager.CreatePlayer(townTile);
        cameraManager.SetupPlayerCamera(newPlayer);
    }
}
