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
        GameObject playerCamera = GameObject.Instantiate(playerCameraPrefab);
        playerCamera.transform.SetParent(cameraHolder.transform);
        newPlayer.GetComponentInChildren<PlayerControl>().viewCamera = playerCamera.GetComponent<Camera>();
        newPlayer.GetComponentInChildren<PlayerControl>().viewCamera.enabled = false;
        newPlayer.transform.Find("PlayerController").GetComponent<PlayerControl>().onMyTurn += CameraManager_onMyTurn;

    }

    private void CameraManager_onMyTurn(object sender, EventArgs e) {
        if(currentCamera != null)
        currentCamera.enabled = false;


        ((PlayerControl)sender).viewCamera.enabled = true;
        currentCamera = ((PlayerControl)sender).viewCamera;
    }
}
