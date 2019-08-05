﻿using UnityEngine;
using System;

[Serializable]
public class UserInterfaceManager : MonoBehaviour{

    public GameObject userInterfaceHolder;
    public GameObject playerUIPrefab;

    public void SetupPlayerUI(GameObject newPlayer) {
        GameObject newPlayerUI = GameObject.Instantiate(playerUIPrefab);
        newPlayerUI.transform.SetParent(gameObject.transform);

        newPlayer.GetComponentInChildren<PlayerControl>().playerUIControl = newPlayerUI.GetComponent<PlayerUIControl>();
        newPlayerUI.GetComponent<PlayerUIControl>().Setup(newPlayer);

    }

}