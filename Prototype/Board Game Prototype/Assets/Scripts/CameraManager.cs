using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CameraManager {

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
        newPlayer.transform.Find("PlayerController").GetComponent<PlayerControl>().onMyTurn += CameraManager_onMyTurn;

    }

    private void CameraManager_onMyTurn(object sender, EventArgs e) {
        if (currentCamera != null) {
            currentCamera.enabled = false;
            currentCamera.GetComponent<AudioListener>().enabled = false;
        }


        ((PlayerControl)sender).viewCamera.enabled = true;
        ((PlayerControl)sender).viewCamera.GetComponent<AudioListener>().enabled = true;
        currentCamera = ((PlayerControl)sender).viewCamera;

        // Will want to have the camera lookAt the town tile of the character whose turn it is here
    }
}
