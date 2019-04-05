using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    MapController mapController;
    public int mapSize = 10;
    public GameObject currentPlayer;
    public CameraManager cameraManager;
    public PlayerManager playerManager;

    void Start() {
        mapController = GameObject.Find("Map").GetComponent<MapController>();
        mapController.generateFieldBlueprint(mapSize);
        mapController.generateField();

        registerPlayer();

        playerManager.playerObjects[0].transform.Find("PlayerController").GetComponent<PlayerControl>().setMyTurn(true);
    }

    private void registerPlayer() {
        GameObject newPlayer = playerManager.CreatePlayer();
        cameraManager.SetupPlayerCamera(newPlayer);
    }
}
