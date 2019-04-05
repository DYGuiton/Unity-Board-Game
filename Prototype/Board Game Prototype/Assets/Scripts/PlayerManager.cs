using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerManager {

    public GameObject playerHolder;
    public List<GameObject> playerObjects = new List<GameObject>();
    public GameObject playerPrefab;

    internal GameObject CreatePlayer() {

        GameObject newPlayer = GameObject.Instantiate(playerPrefab);
        newPlayer.transform.SetParent(playerHolder.transform);
        PlayerControl newController = newPlayer.transform.Find("PlayerController").GetComponent<PlayerControl>();
        playerObjects.Add(newPlayer);
        newController.id = playerObjects.IndexOf(newPlayer);

        return newPlayer;
    }
}
