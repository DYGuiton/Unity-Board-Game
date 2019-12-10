using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CameraManager {

    public int mapSize;

    public GameObject cameraHolder;
    Dictionary<int, Camera> playerCameraMap;
    public GameObject playerCameraPrefab;
    public Camera currentCamera;

    public void GameStart(int playerCount) {

    }

    internal void SetupPlayerCamera(GameObject newPlayer) {
        GameObject nuPlayerCameraPrefab = GameObject.Instantiate(playerCameraPrefab);
        ref Camera playerCamera = ref newPlayer.GetComponentInChildren<PlayerControl>().viewCamera;

        nuPlayerCameraPrefab.transform.SetParent(cameraHolder.transform);
        playerCamera = nuPlayerCameraPrefab.GetComponent<Camera>();
        playerCamera.enabled = false;
        playerCamera.GetComponent<AudioListener>().enabled = false;

        PlayerControl playerController = newPlayer.transform.Find("PlayerController").GetComponent<PlayerControl>();
        playerController.onMyTurn += onMyTurnCallback;
        playerCamera.GetComponent<CameraController>().FocusCameraOnPlayer(playerController.id);
        playerCamera.GetComponent<CameraController>().panLimit = new Vector2(mapSize * 4, mapSize * 4);

    }

    private void onMyTurnCallback(object sender, EventArgs e) {
        if (currentCamera != null) {
            currentCamera.enabled = false;
            currentCamera.GetComponent<AudioListener>().enabled = false;
        }

        currentCamera = ((PlayerControl)sender).viewCamera;
        currentCamera.enabled = true;
        currentCamera.GetComponent<AudioListener>().enabled = true;

        // Will want to have the camera lookAt the town tile of the character whose turn it is here
    }

    private void endMyTurnCallback(object sender, EventArgs e) {

    }
}
